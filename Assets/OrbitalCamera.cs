using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    public Transform mainTarget;
    public Transform extraTarget;
    public Transform currentTarget;

    [Range(.1f,2)]
    public float cameraSensitivity;
    [Range(.1f,10)]
    public float zoomSensitivity;

    [Range(1,50)] 
    public float targetMoveSpeed;

    public Vector2 zoomLimit;

    private bool targetChange;
    
    private Coroutine targetChangeRoutine;
    
    //☺
    private float CameraZoomValue => transform.localPosition.z;
    private float CameraZoomNext => CameraZoomValue - ZoomAxis;
    private Transform ParentTransform => transform.parent;
    private Camera _camera => GetComponent<Camera>();

    private float AxisX => Input.GetAxis("Vertical");
    private float AxisY => Input.GetAxis("Horizontal");
    private float AxisZ => Input.GetAxis("Mouse ScrollWheel");
    private Vector2 CameraAxis => new Vector3(AxisX * cameraSensitivity, -AxisY * cameraSensitivity);
    private float ZoomAxis => (AxisZ * zoomSensitivity);

    private void Update()
    {
        TargetFunction();

        ViewSide();

        OrbitalTransform();
    }

    private void TargetFunction()
    {
        TargetChange();

        SelectTarget();

        ResetTarget();
    }

    private void TargetChange()
    {
        if (targetChange)
        {
            if (Vector3.Distance(ParentTransform.position, currentTarget.position) > 1)
            {
                ParentTransform.position = Vector3.Lerp(ParentTransform.position, currentTarget.position,
                    targetMoveSpeed * Time.deltaTime);
            }
            else
            {
                targetChange = false;
            }
        }
    }

    private void SelectTarget()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //Mouse raycast
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Grabbable"))
                {
                    extraTarget = hit.transform;
                    currentTarget = extraTarget;
                    targetChange = true;
                }
            }
        }
    }

    private void ResetTarget()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentTarget = mainTarget;
            targetChange = true;
        }
    }

    private void ViewSide()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (targetChangeRoutine != null) StopCoroutine(targetChangeRoutine);

            targetChangeRoutine = StartCoroutine(GoToRotation(new Vector3(0, 0, 0)));
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (targetChangeRoutine != null) StopCoroutine(targetChangeRoutine);

            targetChangeRoutine = StartCoroutine(GoToRotation(new Vector3(0, 180, 0)));
        }
    }

    private void OrbitalTransform()
    {
        ParentTransform.eulerAngles += (Vector3) CameraAxis;

        if (AxisZ < 0)
        {
            if (CameraZoomNext > -zoomLimit.y)
                transform.localPosition += new Vector3(0, 0, ZoomAxis);
        }
        else if (AxisZ > 0)
        {
            if (CameraZoomNext < -zoomLimit.x)
                transform.localPosition += new Vector3(0, 0, ZoomAxis);
        }
    }
    
    private IEnumerator GoToRotation(Vector3 rotation)
    {
        while (Vector3.Distance(ParentTransform.eulerAngles, rotation) > 0.1f)
        {
            ParentTransform.eulerAngles = Vector3.Lerp(ParentTransform.eulerAngles, rotation, Time.deltaTime * targetMoveSpeed);
            //Debug.Log("Lerping");
            yield return null;
        }
    }
}
