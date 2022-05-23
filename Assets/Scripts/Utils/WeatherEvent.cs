using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontLetItFall.Utils
{
    [CreateAssetMenu(fileName = "WeatherEvent", menuName = "DLIF/Weather/WeatherEvent", order = 0)]
    public class WeatherEvent : ScriptableObject
    {
        [Header("About")]
        public string eventName;
        public WeatherEventType weatherEventType;
        public GameObject particle;
        public Sprite icon;
        
        [Space]
        [Header("Stats")]
        [Range(0, 100)]
        public float windForce;
        
        [Range(0, 25)]
        public float shipSwing;

        public Vector2 duration;
        
        [Range(0, 10)] 
        public int dangerLevel;

        public enum WeatherEventType
        {
            Rain,
            Snow,
            RainStorm,
            IceStorm,
            DustStorm,
            Gale,
            Clear
        }
        
        public int GetDuration()
        {
            return (int)duration.x + Random.Range(0, (int)duration.y);
        }
        
        public GameObject GetParticle()
        {
            if(particle != null)
                return particle;
            else 
                return new GameObject();
        }
        
        public Vector3 GetShipSwing()
        {
            return new Vector3(Random.Range(-shipSwing,shipSwing), Random.Range(-shipSwing,shipSwing), Random.Range(-shipSwing,shipSwing));
        }
    }
}
