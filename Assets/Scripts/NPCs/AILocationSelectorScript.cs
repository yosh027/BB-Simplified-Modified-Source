using UnityEngine;
using UnityEngine.AI;

public class AILocationSelectorScript : MonoBehaviour
{
    #region Setting Locations
    public Vector3 SetNewTargetForAgent(NavMeshAgent agent, string locationType = "default")
    {
        ambience.PlayAudio();

        Transform[] targetLocations = locationType switch
        {
            "hall"    => hallLocation,
            "present" => presentLocation,
            _         => newLocation
        };

        int id = Random.Range(0, targetLocations.Length);
        Vector3 targetPosition = targetLocations[id].position;

        if (agent != null)
        {
            agent.SetDestination(targetPosition);
        }

        return targetPosition;
    }
    #endregion

    #region Serialized Fields
    [Header("Location Arrays")]
    [SerializeField] private Transform[] newLocation;
    [SerializeField] private Transform[] hallLocation;
    [SerializeField] private Transform[] presentLocation;

    [Header("References")]
    [SerializeField] private AmbienceScript ambience;
    #endregion
}