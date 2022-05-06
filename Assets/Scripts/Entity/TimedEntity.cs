using UnityEngine;
using DontLetItFall.UI;
using DontLetItFall.Variables;

namespace DontLetItFall.Entity
{
    public class TimedEntity : MonoBehaviour
    {
        public ObjectLifeBar lifeBar;

        public Value<float> maxTime = new Value<float>(15);
        public Value<float> currentTime = new Value<float>(0);

        public string triggerToHolder = "";
        public bool isInTrigger = false;

        private void OnEnable()
        {
            currentTime.value = 0;
            lifeBar = GameUIController.Instance.ShowLifeBar(this.gameObject, Vector3.up);
        }

        private void Update()
        {
            if (!isInTrigger)
                currentTime.value += Time.deltaTime;

            float mx = maxTime.value;
            lifeBar.value = (mx - currentTime.value) / mx;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == triggerToHolder)
            {
                isInTrigger = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == triggerToHolder)
            {
                isInTrigger = false;
            }
        }
    }
}