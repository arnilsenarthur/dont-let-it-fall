using System;
using System.Collections;
using System.Collections.Generic;
using DontLetItFall;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public HUDScript HUD;
    private void OnTriggerEnter(Collider col)
    {
        if(!col.CompareTag("Player"))
            Destroy(col);
        else
            HUD.DeathScreen();
    }

    private void OnCollisionEnter(Collision col)
    {
        if(!col.gameObject.CompareTag("Player"))
            Destroy(col.gameObject);
        else
            HUD.DeathScreen();
    }
}
