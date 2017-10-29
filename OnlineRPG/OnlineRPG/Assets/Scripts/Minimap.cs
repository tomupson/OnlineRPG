using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Camera minimapCamera;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private bool rotateMap;
    [SerializeField] private RectTransform arrow;

    void LateUpdate()
    {
        Vector3 newPos = player.position;
        newPos.y = minimapCamera.transform.position.y;
        minimapCamera.transform.position = newPos;
        if (rotateMap)
        {
            minimapCamera.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, -playerCamera.transform.eulerAngles.y));
            arrow.rotation = Quaternion.identity;
        } else
        {
            arrow.rotation = Quaternion.Euler(new Vector3(0, 0, -player.eulerAngles.y));
        }
    }
}