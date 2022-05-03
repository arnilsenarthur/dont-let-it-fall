using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCenterOfMass : MonoBehaviour
{
    #region Public Fields
    [Header("SETTINGS")]
    public Vector3 centerOfMass = new Vector3(0f, 0f, 0f);
    #endregion

    public bool Working = true;

    private bool _lastWorking = false;

    void Start()
    {
        _lastWorking = !Working;
    }

    private void FixedUpdate()
    {
        if (_lastWorking == Working)
            return;

        _lastWorking = Working;

        if (Working)
            GetComponent<Rigidbody>().centerOfMass = centerOfMass;
        else
            GetComponent<Rigidbody>().ResetCenterOfMass();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!Working)
            return;

        if (other.impulse.magnitude > 3f)
        {
            Working = false;
            StartCoroutine(WaitForSeconds(2f));
        }
    }

    //Set working after n seconds
    IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Working = true;
    }
}
