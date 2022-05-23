using System;
using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Entity.Player;
using UnityEngine;

namespace DontLetItFall
{
    public class FurnaceScript : MonoBehaviour
    {
        [SerializeField] private PlayerManager _pManager;
        [SerializeField] [Range(0,2)] private float massConvertValue = .2f;
        [SerializeField] [Range(0, 1)] private float noFuelBurn = .5f;
        
        [SerializeField] private EntityPlayer _player;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _player ??= FindObjectOfType<EntityPlayer>();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (!col.CompareTag("Grabbable")) return;

            Rigidbody colRb = col.GetComponent<Rigidbody>();
            float fuel = colRb.mass * massConvertValue;
            
            if (col.name.Contains("CoalBox"))
                _pManager.AddFuel(fuel);
            else
                _pManager.AddFuel(fuel*noFuelBurn);

            _player.ReleaseObject();
            _audioSource.Play();
            Destroy(col.gameObject);
        }
    }
}
