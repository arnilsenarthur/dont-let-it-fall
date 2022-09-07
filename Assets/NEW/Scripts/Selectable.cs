using System.Collections.Generic;
using UnityEngine;

namespace New
{
    public class Selectable : MonoBehaviour 
    {
        public virtual bool CanBeSelected() => true;
        public virtual void OnSelect() {}
        public virtual void OnDeselect() {}
        public virtual bool CanBeInMultiSelection(List<Selectable> selected) => false;
        public virtual bool OnClick(Vector3 position, Vector3 normal, GameObject gameObject) => false;

        /*
        public virtual void OnSelect() {}

        public virtual void OnDeselect() {}

        public virtual bool CanBeSelected() => false;

        public virtual bool OnClick(Vector3 position, Vector3 normal, GameObject gameObject) => false;

        public virtual bool CanBeInMultiSelection(List<Selectable> selected) => true;
        */
    }
}