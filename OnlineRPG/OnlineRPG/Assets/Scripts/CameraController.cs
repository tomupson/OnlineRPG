using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    Transform target;

    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    private float currentZoom = 10f;

    public float pitch = 2f;

    public float yawSpeed = 100f;
    private float currentYaw = 0f;

    void Start()
    {
        target = GetComponentInParent<Player>().transform;
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        }

        currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
    }

    void LateUpdate()
    {
        transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pitch);

        transform.RotateAround(target.position, Vector3.up, currentYaw);
    }
}