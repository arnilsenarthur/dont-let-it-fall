using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutpostCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] [Range(0,2)] private float _moveSpeed = 1;
    [SerializeField] [Range(0,2)] private float _rotationSpeed = 1;
    [SerializeField] [Range(0,2)] private float _zoomSpeed = 1;
    [SerializeField] [Range(1,20)] private float _maxDistance = 10;
    [SerializeField] [Range(10,200)] private float _zoomMultiplier = 115;

    private Camera _camera;
    private Vector3 _lookAt;
    
    private void Start()
    {
        TryGetComponent(out _camera);
        _lookAt = _target.position;
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(_target.position.x, transform.position.y, transform.position.z);
        float distanceX = Distance1D(targetPosition.x, transform.position.x);
        float distanceZ = Distance1D(_target.position.z, transform.position.z);
        
        if (distanceX > _maxDistance)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
        }
        
        _lookAt = Vector3.Lerp(_lookAt, _target.position, _rotationSpeed * Time.deltaTime);
        transform.LookAt(_lookAt);

        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _zoomMultiplier / distanceZ, _zoomSpeed * Time.deltaTime);
    }

    private float Distance1D(float a, float b)
    {
        return Mathf.Abs(a - b);
    }
}
