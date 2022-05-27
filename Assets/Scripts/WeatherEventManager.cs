using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Utils;
using UnityEngine;

public class WeatherEventManager : MonoBehaviour
{
    [Header("Weather Event")]
    [SerializeField] 
    private WeatherEvent weatherEvent;
    [SerializeField] 
    private int eventDuration;
    
    [SerializeField] 
    [Range(0,150)]
    private float swingMultiplier = 100;
    
    [SerializeField] 
    [Range(0,2)]
    private float swingTimer = .5f;

    [Space(10)] 
    [SerializeField]
    private WeatherEvent clearWeather;
    
    private GameObject weatherParticle;
    private Vector3 swingDirection;
    
    [Space]
    [Header("Miscellany")] 
    [SerializeField]
    private Rigidbody playerShip;
    
    void Start()
    {
        clearWeather ??= Resources.Load<WeatherEvent>("Data/Weather/Clear");
        ChangeWeatherEvent(weatherEvent);
        StartCoroutine(ShipSwingCoroutine());
    }
    
    void FixedUpdate()
    {
        playerShip.AddTorque(swingDirection * swingMultiplier);
    }
    
    public void ChangeWeatherEvent(WeatherEvent newWeatherEvent)
    {
        weatherEvent = newWeatherEvent;
        eventDuration = weatherEvent.GetDuration();
        ChangeWeatherParticle();

        if(eventDuration > 0)
            StartCoroutine(WeatherEventCoroutine());
    }

    private void ChangeWeatherParticle()
    {
        if (weatherParticle != null)
            Destroy(weatherParticle);
        
        weatherParticle = Instantiate(weatherEvent.GetParticle(), transform);
    }

    public void ClearWeather()
    {
        ChangeWeatherEvent(clearWeather);
    }
    
    private IEnumerator WeatherEventCoroutine()
    {
        yield return new WaitForSeconds(eventDuration);
        ChangeWeatherEvent(clearWeather);
    }
    
    private IEnumerator ShipSwingCoroutine()
    {
        while (true)
        {
            swingDirection = weatherEvent.GetShipSwing();
            yield return new WaitForSeconds(swingTimer);
        }
    }
}
