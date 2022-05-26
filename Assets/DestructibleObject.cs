using System;
using System.Collections;
using System.Collections.Generic;
using DontLetItFall.UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class DestructibleObject : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField]
    private ObjectLifeBar lifeBar;

    [Space]
    [Header("Settings")]
    [SerializeField]
    private DestructibleObjectData data;
    
    private float currentLife;
    private float lifeValue => currentLife / data.maxLife;
    
    private MeshRenderer meshRenderer;
    private Collider collider;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        currentLife = data.maxLife;
        
        ChangeMaterial();
    }

    void Update()
    {
        lifeBar.value = lifeValue;

        if(!meshRenderer.enabled && currentLife > 0)
        {
            DetectLife();
        }
    }

    private void ChangeMaterial()
    {

        if (meshRenderer != null)
        {          
            int index = (int) Mathf.Floor(lifeValue * data.objectLifeLevels.Count);
            meshRenderer.material = data.objectLifeLevels[Mathf.Clamp(index, 0, data.objectLifeLevels.Count - 1)];
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        float damage = col.impulse.magnitude;

        if(damage > data.shieldDamage)
        {
            currentLife -= (damage * data.impulseDamage);
            DetectLife();
            ChangeMaterial();
        }
    }

    private void DetectLife()
    {
        if (currentLife <= 0)
        {
            currentLife = 0;
            
            meshRenderer.enabled = false;
            collider.enabled = false;
        }
        else if(!meshRenderer.enabled)
        {
            meshRenderer.enabled = true;
            collider.enabled = true;
        }
    }
}
