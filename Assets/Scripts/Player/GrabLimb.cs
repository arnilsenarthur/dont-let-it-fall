using UnityEngine;

namespace DontLetItFall.Entity.Player
{
    public class GrabLimb : MonoBehaviour
    {
        #region Public Fields
        [Header("SETTINGS")]
        public Rigidbody hand;
        public EntityPlayer player;
        #endregion


        /*
        #region Private Fields
        private GameObject _currentObject;
        private bool _isGrabbing = false;
        #endregion

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_currentObject != null)
                {
                    FixedJoint joint = _currentObject.AddComponent<FixedJoint>();
                    joint.connectedBody = hand;
                    joint.breakForce = 500;
                    _isGrabbing = true;
     
                    PlayerInteraction interaction = new PlayerInteraction();
                    interaction.targetObject = _currentObject;
                    interaction.type = 0;
                    player.OnStopCanInteractWithObject.Invoke(interaction);
                }
            }
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (_currentObject != null)
                {
                    FixedJoint joint = _currentObject.GetComponent<FixedJoint>();
                    Destroy(joint);
                }

                _isGrabbing = false;
                _currentObject = null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != null && other.tag == "Grabbable")
            {
                _currentObject = other.gameObject;

                PlayerInteraction interaction = new PlayerInteraction();
                interaction.targetObject = _currentObject;
                interaction.type = PlayerInteractionType.Grab;
                player.OnCanInteractWithObject.Invoke(interaction);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_isGrabbing)
                return;

            if (_currentObject != null && other.gameObject == _currentObject)
            {
                _currentObject = null;

                PlayerInteraction interaction = new PlayerInteraction();
                interaction.targetObject = _currentObject;
                interaction.type = PlayerInteractionType.Grab;
                player.OnStopCanInteractWithObject.Invoke(interaction);
            }
        }
        */

        public GameObject currentObject;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != null && other.tag == "Grabbable" && player.grabbedObject == null)
            {
                currentObject = other.gameObject;

                PlayerInteraction interaction = new PlayerInteraction();
                interaction.targetObject = currentObject;
                interaction.type = PlayerInteractionType.Grab;
                player.OnCanInteractWithObject.Invoke(interaction);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(player.grabbedObject != null)
                return;

            if (currentObject != null && other.gameObject == currentObject)
            {
                currentObject = null;

                PlayerInteraction interaction = new PlayerInteraction();
                interaction.targetObject = currentObject;
                interaction.type = PlayerInteractionType.Grab;
                player.OnStopCanInteractWithObject.Invoke(interaction);
            }
        }
    }
}