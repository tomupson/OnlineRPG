using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    //public Interactable focus;
    public LayerMask movementMask;

    [SerializeField] private Camera playerCam;
    [SerializeField] private GameObject mouseInteractInfoPanel;

    Player player;
    Vector3 pointToMove;
    Interactable focus;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            InteractionManager.instance.Hide();

            Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                RemoveFocus();
                player.MoveToPoint(hit.point); // Move the player to the point they clicked.
            }
        }

        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            GetInteraction();
        }
    }

    void GetInteraction()
    {
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            GameObject objHit = hit.collider.gameObject;
            Interactable interactableObjHit = objHit.transform.root.GetComponentInChildren<Interactable>(); // InChildren also seearches the parent.

            if (interactableObjHit != null)
            {
                interactableObjHit.OpenContextMenu();
                SetFocus(interactableObjHit);
                pointToMove = interactableObjHit.transform.position;
            }
            else
            {
                RemoveFocus();
                Debug.Log(string.Format("No interactable component found on \"{0}\"", objHit.name));
                pointToMove = hit.point;
                InteractionManager.instance.SetOptions(null);
                InteractionManager.instance.Show();
            }
        }
    }

    public void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
                focus.OnDefocus();
            focus = newFocus;
        }

        newFocus.OnFocus(player);
    }

    void RemoveFocus()
    {
        if (focus != null)
            focus.OnDefocus();
        focus = null;
        player.StopTargetFollow();
    }

    public void GoToPointMarked()
    {
        player.MoveToPoint(pointToMove);
    }
}