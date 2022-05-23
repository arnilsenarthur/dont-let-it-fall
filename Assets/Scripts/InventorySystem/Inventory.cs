using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DontLetItFall.Variables;

namespace DontLetItFall.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public List<int> acceptedIds;

        public Value<int> currentItemCount;
    }
}