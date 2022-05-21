using System;
using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Entity.Player;
using UnityEngine;

namespace DontLetItFall
{
    public class FurnaceScript : MonoBehaviour
    {
        [SerializeField] private HUDScript _hudScript;
        [SerializeField] [Range(0,2)] private float massConvertValue = .2f;
        [SerializeField] [Range(0, 1)] private float noFuelBurn = .5f;
        
        [SerializeField] private EntityPlayer _player;

        private void Awake()
        {
            _player ??= FindObjectOfType<EntityPlayer>();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (!col.CompareTag("Grabbable")) return;

            Rigidbody colRb = col.GetComponent<Rigidbody>();
            float fuel = colRb.mass * massConvertValue;
            
            if (col.name.Contains("CoalBox"))
                _hudScript.AddFuel(fuel);
            else
                _hudScript.AddFuel(fuel*noFuelBurn);

            _player.ReleaseObject();
            
            Destroy(col.gameObject);
        }
    }
}
