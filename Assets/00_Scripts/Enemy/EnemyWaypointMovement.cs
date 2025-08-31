using UnityEngine;

public class EnemyWaypointMovement : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    public float moveSpeed = 3f;
    private int currentWaypointIndex = 0;

    void OnEnable()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            GameObject waypointsParent = GameObject.FindGameObjectWithTag("WayPoints");
            if (waypointsParent != null)
            {
                int childCount = waypointsParent.transform.childCount;
                waypoints = new Transform[childCount];
                for (int i = 0; i < childCount; i++)
                {
                    waypoints[i] = waypointsParent.transform.GetChild(i);
                }
            }
            else
            {
                Debug.LogError("WayPoints 태그가 붙은 오브젝트를 찾을 수 없습니다.");
            }
        }
        currentWaypointIndex = 0;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        if (distance < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
                currentWaypointIndex = 0;
        }
    }
}
