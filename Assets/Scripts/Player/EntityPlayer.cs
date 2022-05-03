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

        #region Events
        public UnityEvent<PlayerInteraction> OnCanInteractWithObject;
        public UnityEvent<PlayerInteraction> OnStopCanInteractWithObject;
        #endregion
    }
}