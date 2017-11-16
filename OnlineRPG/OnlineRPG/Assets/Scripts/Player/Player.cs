using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour
{
    [HideInInspector] public CharacterStats stats;

    [SerializeField] private float rotationSpeed = 5f;

    NavMeshAgent agent;
    Transform target;

    PhotonView photonView;
    public static GameObject LocalPlayer { get; set; }

    void Awake()
    {
        photonView = PhotonView.Get(this);
        if (photonView.isMine)
        {
            LocalPlayer = gameObject;
            if (EventHandler.OnPlayerJoinedRoom != null)
            {
                EventHandler.OnPlayerJoinedRoom.Invoke(PhotonNetwork.player);
            }
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<CharacterStats>();
    }

    public void MoveToPoint(Vector3 point)
    {
        InteractionManager.singleton.Hide();
        agent.SetDestination(point);
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            LookAtTarget();
        }
    }

    public void FollowTarget(Interactable newTarget)
    {
        InteractionManager.singleton.Hide();
        agent.stoppingDistance = newTarget.radius * 0.7f;
        agent.updateRotation = false;
        target = newTarget.interactionTransform;
    }

    public void StopTargetFollow()
    {
        agent.stoppingDistance = 0;
        agent.updateRotation = true;
        target = null;
    }

    void LookAtTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Don't rotate on Y.
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}