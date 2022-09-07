using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace New
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private List<Selectable> _selected = new List<Selectable>();

        public bool listeningForClicks = true;

        public List<GameObject> boxes = new List<GameObject>();

        public GameObject prefabBox;

        public virtual void Update() 
        {
            if(listeningForClicks && Input.GetMouseButtonDown(0))
                OnClick();

            if(listeningForClicks && Input.GetMouseButtonDown(2))
                SpawnBox();

            if(Input.GetKeyDown(KeyCode.K))
            {
                foreach(var box in boxes)
                    Destroy(box);

                boxes.Clear();
            }

            if(Input.GetKeyDown(KeyCode.R))
            {
                Scene scene = SceneManager.GetActiveScene(); 
                SceneManager.LoadScene(scene.name);
            }
        }

        public void SpawnBox()
        {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit, 20f)) 
            {
                if(hit.collider.gameObject.tag == "Ship")
                {
                    GameObject box = GameObject.Instantiate(prefabBox);
                    Vector3 point = hit.point;
                    point.y = 4;
                    box.transform.position = point;
                    boxes.Add(box);
                }
            }
        }

        public void OnClick()
        {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit, 20f)) 
            {
                foreach(Selectable selectable in _selected)
                    if(selectable.OnClick(hit.point, hit.normal, hit.collider.gameObject))
                        return;

                Selectable s = hit.collider.gameObject.GetComponentInParent<Selectable>();
            
                if(s == null)
                    return;

                if(!s.CanBeSelected())
                    return;

                Debug.Log("select!");
                
                Select(s);
            }
        }

        public int IndexOf(Selectable selectable)
        {
            return _selected.IndexOf(selectable);
        }

        public Selectable GetSelectable(int index)
        {
            return _selected[index];
        }

        public void Deselect()
        {
            foreach(Selectable s in _selected)
                s.OnDeselect();

            _selected.Clear();
        }

        public void Deselect(int index)
        {
            Deselect(GetSelectable(index));
        }

        public void Deselect(Selectable selectable)
        {   
            if(!_selected.Contains(selectable))
                return;

            _selected.Remove(selectable);
            selectable.OnDeselect();
        }

        public void Select(Selectable selectable)
        {
            if(_selected.Contains(selectable))
            {
                Deselect(selectable);
                return;
            }
            
            if(!selectable.CanBeInMultiSelection(_selected))
                Deselect();

            _selected.Add(selectable);
            selectable.OnSelect();
        }
    }
}