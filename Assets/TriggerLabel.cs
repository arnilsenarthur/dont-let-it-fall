using System;
using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Entity.Player;
using DontLetItFall.Inventory;
using UnityEngine;

namespace DontLetItFall.UI
{
    public class TriggerLabel : MonoBehaviour
    {
        public GameObject labelInfo;

        private void OnTriggerEnter(Collider col)
        {
            if(col.CompareTag("Player"))
                labelInfo.SetActive(true);
        }

        private void OnTriggerExit(Collider col)
        {
            if(col.CompareTag("Player"))
                labelInfo.SetActive(false);
        }
    }
}
