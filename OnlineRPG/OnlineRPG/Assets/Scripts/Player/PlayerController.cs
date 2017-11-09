using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    //public Interactable focus;
    public LayerMask movementMask;

    Camera playerCam;
    [SerializeField] private GameObject mouseInteractInfoPanel;

    Player player;
    Vector3 pointToMove;
    Interactable focus;

    List<Option> defaultInteractOptions;

    void Start()
    {
        InitializeOptions();
        player = GetComponent<Player>();
        playerCam = GetComponentInChildren<Camera>();
    }

    void InitializeOptions()
    {
        defaultInteractOptions = new List<Option>()
        {
            new Option()
            {
                Text = "Walk here",
                OnOptionClick = GoToPointMarked
            }
        };
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            InteractionManager.singleton.Hide();

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
            if (objHit == Player.LocalPlayer) return;

            Interactable interactableObjHit = objHit.transform.root.GetComponentInChildren<Interactable>(); // InChildren also seearches the parent.
            
            if (interactableObjHit != null)
            {
                interactableObjHit.GetComponent<IInteractable>().OpenContextMenu();
                SetFocus(interactableObjHit);
                pointToMove = interactableObjHit.transform.position;
            }
            else
            {
                RemoveFocus();
                Debug.LogWarning(string.Format("No interactable component found on \"{0}\"", objHit.name));
                pointToMove = hit.point;
                if (defaultInteractOptions != null)
                {
                    InteractionManager.singleton.SetOptions(defaultInteractOptions, true);
                    InteractionManager.singleton.Show();
                } else
                {
                    Debug.LogError("Interact Options have not been set!");
                }
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