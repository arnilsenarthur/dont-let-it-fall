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

        public GameObject expiredPlaceholder;

        private void OnEnable()
        {
            currentTime.value = 0;
        }

        private void Update()
        {
            if (!isInTrigger)
            {
                currentTime.value += Time.deltaTime;

                if(currentTime.value > maxTime.value)
                {
                    if(expiredPlaceholder == null)
                        return;

                    GameObject spawned = Instantiate(expiredPlaceholder, this.transform.position, this.transform.rotation);
                    Destroy(this.gameObject);
                }
            }

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