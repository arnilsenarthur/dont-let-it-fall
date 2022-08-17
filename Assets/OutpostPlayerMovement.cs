using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutpostPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 centerOfMass;
    private float MoveX => Input.GetAxis("Horizontal");
    private float MoveY => Input.GetAxis("Vertical");

    private Rigidbody rb => GetComponent<Rigidbody>();

    private void Start()
    {
        rb.centerOfMass = centerOfMass;
    }

    private void Update()
    {
        transform.Translate(new Vector3(MoveX * moveSpeed, 0, MoveY*moveSpeed) * Time.deltaTime, Space.World);
    }
}
