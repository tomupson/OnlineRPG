using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Interactable : MonoBehaviour
{
    [Header("Base Class Variables")]
    public Transform interactionTransform;
    public float radius = 3f;

    [Space]

    public Texture2D defaultCursor;
    public Texture2D hoverCursor;

    [Space]

    [Header("Skills")]
    public string skillName;
    public float xpGain;

    [Space]

    [Header("Items")]
    public Inventory inventory;
    public string itemToAddSlug;

    [Space]

    [Tooltip("The name of the interactable.")] public string interactableName;

    bool isFocused;
    bool hasArrived = false;
    [HideInInspector] public Transform player;
    private Action callbackMethod;

    void Update()
    {
        if (isFocused && !hasArrived)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= radius)
            {
                Interact();
                hasArrived = true;
            }
        }
    }

    public virtual void Interact()
    {
        if (callbackMethod != null)
        {
            callbackMethod.Invoke();
        }
    }

    public virtual void OpenContextMenu()
    {
        InteractionManager.instance.Show();
    }

    public void OnFocus(Transform player)
    {
        isFocused = true;
        this.player = player;
    }

    public void OnDefocus()
    {
        isFocused = false;
        this.player = null;
    }

    public void MoveToInteractable(Action callbackMethod)
    {
        this.callbackMethod = callbackMethod;
        hasArrived = false;
        if (player != null)
        {
            player.GetComponent<Player>().FollowTarget(this);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
        {
            interactionTransform = this.transform;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
}