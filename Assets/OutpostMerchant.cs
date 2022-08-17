using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutpostMerchant : MonoBehaviour
{
    public Image icon;

    private void Awake()
    {
        icon.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
            icon.gameObject.SetActive(true);
    }
    
    private void OnTriggerExit(Collider col)
    {
        if(col.CompareTag("Player"))
            icon.gameObject.SetActive(false);
    }
}
