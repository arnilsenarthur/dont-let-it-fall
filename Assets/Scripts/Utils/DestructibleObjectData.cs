using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestructibleObject", menuName = "DLIF/Destructible/DestructibleObject", order = 0)]
public class DestructibleObjectData : ScriptableObject
{
    [Header("Settings")]
    public float maxLife;
    [Range(0,150)]
    public float shieldDamage;
    [Range(0,1)]
    public float impulseDamage;

    [Space]
    [Header("Materials")]
    [SerializeField]
    public List<Material> objectLifeLevels;
}
