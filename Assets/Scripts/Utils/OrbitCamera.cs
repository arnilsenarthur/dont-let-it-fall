using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public GameObject target;

    [Header("SETTINGS")]
    public Vector3 offset = Vector3.down;

    public float minDistance = 3f;
    public float maxDistance = 10f;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;
    public float scrollSpeed = 5f;

    public float yMinLimit = -20;
    public float yMaxLimit = 80;

    private Vector3 _TargetPosition => target.transform.position + offset;

    [Header("STATE")]
    public float distance = 5f;
    public float x = 0.0f;
    public float y = 0.0f;
    private float prevDistance;

    void Start()
    {
        var angles = transform.eulerAngles;
        //x = angles.y;
        //y = angles.x;
    }

    private void OnEnable()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        //Cursor.lockState = CursorLockMode.None;
    }

    void LateUpdate()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        }
        */

        distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        if (target)
        {
            var pos = Input.mousePosition;
            var dpiScale = 1f;
            if (Screen.dpi < 1) dpiScale = 1;
            if (Screen.dpi < 200) dpiScale = 1;
            else dpiScale = Screen.dpi / 200f;

            //if (pos.x < 380 * dpiScale && Screen.height - pos.y < 250 * dpiScale) return;

            // comment out these two lines if you don't want to hide mouse curser or you have a UI button 
            //Cursor.visible = false;

            if (Input.GetMouseButton(1))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            }

            y = ClampAngle(y, yMinLimit, yMaxLimit);
            var rotation = Quaternion.Euler(y, x, 0);
            var position = rotation * new Vector3(0.0f, 0.0f, -distance) + _TargetPosition;
            transform.rotation = rotation;
            transform.position = position;

        }

        if (Mathf.Abs(prevDistance - distance) > 0.001f)
        {
            prevDistance = distance;
            var rot = Quaternion.Euler(y, x, 0);
            var po = rot * new Vector3(0.0f, 0.0f, -distance) + _TargetPosition;
            transform.rotation = rot;
            transform.position = po;
        }
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
