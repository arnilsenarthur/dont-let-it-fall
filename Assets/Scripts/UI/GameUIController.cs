using System;
using System.Collections.Generic;
using UnityEngine;
using DontLetItFall.Entity.Player;

namespace DontLetItFall.UI
{
    [System.Serializable]
    public class LifeBarTracker
    {
        public ObjectLifeBar bar;
        public GameObject target;
        public Vector3 offset;
    }

    public class GameUIController : MonoBehaviour
    {
        public static GameUIController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameUIController>();
                }
                return _instance;
            }
        }
        private static GameUIController _instance;

        public GameObject grabObjectPopup;

        [Header("PREFABS")]
        public GameObject objectLifebarPrefab;

        private GameObject _grabObject;

        private void Start()
        {
            _instance = this;
        }

        public void OnCanInteractWithObject(PlayerInteraction interaction)
        {
            switch (interaction.type)
            {
                case PlayerInteractionType.Grab:
                    _grabObject = interaction.targetObject;
                    grabObjectPopup.SetActive(true);
                    break;
            }
        }

        public void OnStopCanInteractWithObject(PlayerInteraction interaction)
        {
            switch (interaction.type)
            {
                case PlayerInteractionType.Grab:
                    _grabObject = null;
                    grabObjectPopup.SetActive(false);
                    break;
            }
        }

        private void Update()
        {
            if (_grabObject != null)
            {
                //Object position to screen position
                Vector3 screenPos = Camera.main.WorldToScreenPoint(_grabObject.transform.position);
                grabObjectPopup.transform.position = screenPos;
            }
            
        }
    }
}