using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class NPC : MonoBehaviour
{
    #region Unity Lifecycle
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        OnStart();
    }

    protected virtual void Update() => OnUpdate();

    protected virtual void FixedUpdate()
    {
        if (canTargetPlayer)
        {
            CheckForPlayer();
        }

        if (!isInteracting)
        {
            HandleMovement();
        }

        OnFixedUpdate();
    }
    #endregion

    #region AI Behavior
    protected virtual void CheckForPlayer()
    {
        Vector3 direction = player.position - transform.position;

        if (transform.position.RaycastFromPosition(direction, out RaycastHit hit))
        {
            isInteracting = hit.transform.CompareTag("Player");

            if (isInteracting && canTargetPlayer)
            {
                TargetPlayer();
            }
        }
    }

    protected virtual void HandleMovement()
    {
        if (!agent.IsReadyToMove() || coolDown.CountdownWithDeltaTime() != 0) return;

        if (!canTargetPlayer || !isInteracting)
        {
            Wander();
        }
    }

    protected virtual void Wander(string locationType = "default")
    {
        wanderer?.SetNewTargetForAgent(agent, locationType);
        ResetCooldown();
    }

    protected virtual void TargetPlayer()
    {
        agent.SetDestination(player.position);
        ResetCooldown();
    }
    #endregion

    #region Hooks
    public virtual void OnStart() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
    #endregion

    #region Utility
    protected void ResetCooldown() => coolDown = 1;
    #endregion

    #region Editor Gizmos
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showPath && agent != null)
        {
            NavMeshPath path = agent.path;
            if (path != null)
            {
                Handles.color = pathColor;
                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    Handles.DrawAAPolyLine(pathWidth, path.corners[i], path.corners[i + 1]);
                }
            }
        }
    }
#endif
    #endregion

    #region Serialized Fields
    [Header("NPC Functions")]
    [SerializeField] protected Transform player;
    [SerializeField] protected AILocationSelectorScript wanderer;
    public bool isInteracting, canTargetPlayer;

    [Header("Gizmo Settings")]
    [SerializeField] private bool showPath = true;
    [SerializeField] private Color pathColor = Color.red;
    [SerializeField] private float pathWidth = 15f;
    #endregion

    #region Internal State
    protected float coolDown;
    protected NavMeshAgent agent;
    #endregion
}