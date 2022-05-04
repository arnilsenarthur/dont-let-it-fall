using UnityEngine;
using UnityEngine.Events;
using DontLetItFall.Data;

namespace DontLetItFall.Entity.Player
{
    public enum PlayerInteractionType
    {
        Grab
    }

    public struct PlayerInteraction
    {
        public PlayerInteractionType type;
        public GameObject targetObject;
    }

    public class EntityPlayer : EntityBase
    {
        public PlayerStats stats;

        public GameObject grabbedObject = null;

        #region Events
        public UnityEvent<PlayerInteraction> OnCanInteractWithObject;
        public UnityEvent<PlayerInteraction> OnStopCanInteractWithObject;
        #endregion

        public GrabLimb[] grabLimbs;

        public void GrabObject()
        {
            foreach (GrabLimb limb in grabLimbs)
            {
                if (limb.currentObject != null)
                {
                    grabbedObject = limb.currentObject;

                    if (grabbedObject.GetComponent<FixedJoint>() == null)
                    {
                        FixedJoint joint = grabbedObject.AddComponent<FixedJoint>();
                        joint.connectedBody = limb.hand;
                        joint.breakForce = 500;
                    }

                    PlayerInteraction interaction = new PlayerInteraction();
                    interaction.targetObject = grabbedObject;
                    interaction.type = PlayerInteractionType.Grab;

                    OnStopCanInteractWithObject.Invoke(interaction);
                }
            }
        }

        public void ReleaseObject()
        {
            if (grabbedObject != null)
            {
                FixedJoint joint = grabbedObject.GetComponent<FixedJoint>();
                Destroy(joint);
                grabbedObject = null;
            }
        }
    }
}