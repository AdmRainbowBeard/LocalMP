using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private WaypointPath waypointPath;

    [SerializeField]
    private float speed;

    private int targetWaypointIndex;

    private Transform previousWaypoint;
    private Transform targetWaypoint;

    private float timeToWaypoint;
    private float elapsedTime;

    [SerializeField]
    private Transform playerParent;

    private void Start()
    {
        TargetNextWaypoint();
    }

    private void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;

        float elaspedPercentage = elapsedTime / timeToWaypoint;
        elaspedPercentage = Mathf.SmoothStep(0, 1, elaspedPercentage);
        transform.position = Vector3.Lerp(previousWaypoint.position, targetWaypoint.position, elaspedPercentage);
        transform.rotation = Quaternion.Lerp(previousWaypoint.rotation, targetWaypoint.rotation, elaspedPercentage);

        if (elaspedPercentage >= 1)
        {
            TargetNextWaypoint();
        }
    }

    private void TargetNextWaypoint()
    {
        previousWaypoint = waypointPath.GetWaypoint(targetWaypointIndex);
        targetWaypointIndex = waypointPath.GetNextWaypointIndex(targetWaypointIndex);
        targetWaypoint = waypointPath.GetWaypoint(targetWaypointIndex);

        elapsedTime = 0;

        float distToWaypoint = Vector3.Distance(previousWaypoint.position, targetWaypoint.position);

        timeToWaypoint = distToWaypoint / speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water") return;

        Debug.Log("dxcvxcbxcvbcxvb ");

        playerParent = other.transform.parent;
        Debug.Log("Parent is: " + playerParent);

        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(playerParent);
    }
}
