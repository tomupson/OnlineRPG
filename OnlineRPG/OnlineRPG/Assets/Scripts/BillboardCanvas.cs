using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

	void Update()
	{
        transform.LookAt(transform.position + targetCamera.transform.rotation * Vector3.forward, targetCamera.transform.rotation * Vector3.up);
    }
}