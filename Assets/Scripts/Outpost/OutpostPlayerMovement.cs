using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutpostPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;
    public Vector3 centerOfMass;
    public ParticleSystem dustParticles;
    
    private float Speed => moveSpeed * Time.deltaTime;
    private float MoveX => Input.GetAxis("Horizontal");
    private float MoveY => Input.GetAxis("Vertical");
    
    private Vector3 MoveXY => new Vector3(MoveX, 0, MoveY);
    private Vector3 Movement => MoveXY * Speed;
    private Quaternion RotateXY => Quaternion.LookRotation(MoveXY);

    private Rigidbody rb => GetComponent<Rigidbody>();

    private void Start()
    {
        rb.centerOfMass = centerOfMass;
    }

    private void FixedUpdate()
    {
        if (MoveXY.magnitude != 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, RotateXY, turnSpeed * Time.deltaTime);
            if(!dustParticles.isPlaying) dustParticles.Play();
        }else if(dustParticles.isPlaying) dustParticles.Stop();

        rb.MovePosition(transform.position + Movement);
    }
}
