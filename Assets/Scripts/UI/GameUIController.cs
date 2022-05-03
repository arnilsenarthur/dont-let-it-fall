using UnityEngine;
using DontLetItFall.Entity.Player;

namespace DontLetItFall.UI
{
    public class GameUIController : MonoBehaviour
    {
        public GameObject grabObjectPopup;
        private GameObject _grabObject;

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