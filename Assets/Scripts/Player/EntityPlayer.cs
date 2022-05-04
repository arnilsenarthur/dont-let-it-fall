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
        public Rigidbody bodyHips;
        public GameObject grabbedObject = null;

        #region Events
        public UnityEvent<PlayerInteraction> OnCanInteractWithObject;
        public UnityEvent<PlayerInteraction> OnStopCanInteractWithObject;
        #endregion

        #region Public Fields
        public bool Assembled {
            get { return _assembled; }
            set { SetAssembled(value); }
        }
        
        public float wakeUpSpeed = 1f;
        public GrabLimb[] grabLimbs;
        public ConfigurableJoint[] joints;
        public float assembledPositionSpring = 60f;
        public float disassembledPositionSpring = 5f;
        #endregion

        #region Private Fields
        private bool _assembled = true;
        #endregion

        public void GrabObject()
        {
            foreach (GrabLimb limb in grabLimbs)
            {
                if (limb.currentObject != null)
                {
                    if (grabbedObject != null && grabbedObject != limb.currentObject)
                        return;

                    grabbedObject = limb.currentObject;

                    ConfigurableJoint joint = grabbedObject.AddComponent<ConfigurableJoint>();
                    joint.anchor = new Vector3(0, 0, 0);
                    joint.xMotion = ConfigurableJointMotion.Locked;
                    joint.yMotion = ConfigurableJointMotion.Locked;
                    joint.zMotion = ConfigurableJointMotion.Locked;
                    joint.angularXMotion = ConfigurableJointMotion.Locked;
                    joint.angularYMotion = ConfigurableJointMotion.Locked;
                    joint.angularZMotion = ConfigurableJointMotion.Locked;
                    joint.targetPosition = new Vector3(0, 0, 0);
                    joint.connectedBody = limb.hand;
                    joint.projectionDistance = 0.01f;

                    joint.breakForce = 500; 
                    joint.linearLimit = new SoftJointLimit(){limit = 0.01f};

                    //make joint closer as possible

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
                foreach (Joint joint in grabbedObject.GetComponents<Joint>())
                {
                    Destroy(joint);
                }

                grabbedObject = null;
            }
        }

        private void Update() 
        {
            if(Input.GetKeyDown(KeyCode.J))
                SetAssembled(!_assembled);
        }
        
        private void FixedUpdate() 
        {
            //Stand Assembled
            if(_assembled)
            {
                bodyHips.transform.localRotation = Quaternion.Lerp(bodyHips.transform.localRotation, Quaternion.identity, Time.deltaTime * wakeUpSpeed);
                bodyHips.transform.localPosition = Vector3.Lerp(bodyHips.transform.localPosition, Vector3.zero, Time.deltaTime * wakeUpSpeed);
            }   
        }

        public void SetAssembled(bool assembled)
        {
            bodyHips.isKinematic = assembled;
            _assembled = assembled;

            foreach (ConfigurableJoint joint in joints)
            {
                JointDrive drive = joint.angularXDrive;
                drive.positionSpring = assembled ? assembledPositionSpring : disassembledPositionSpring;
                joint.angularXDrive = drive;
                
                drive = joint.angularYZDrive;
                drive.positionSpring = assembled ? assembledPositionSpring : disassembledPositionSpring;
                joint.angularYZDrive = drive;
            }
        }
    }
}