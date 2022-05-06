using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontLetItFall
{
    [CreateAssetMenu(fileName = "LoadingText", menuName = "DLIF/LoadingTexts")]
    public class LoadingTexts : ScriptableObject
    {
        public List<string> LoadingTitle;
        public List<string> LoadingSubtitle;
    }
}
