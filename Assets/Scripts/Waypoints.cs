using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public Transform[] wayPointList;

    public int currentWayPoint = 0;
    private Transform targetWayPoint;

    public float speed = 100f;

    private void Update()
    {
        if (currentWayPoint < this.wayPointList.Length)
        {
            if (targetWayPoint == null)
            {
                targetWayPoint = wayPointList[currentWayPoint];
            }
            

            walk();
        }
    }

    void walk()
    {
        // Rotate
        transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, speed * Time.deltaTime, 0.0f);

        // Move
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, speed * Time.deltaTime);

        if (transform.position == targetWayPoint.position)
        {
            currentWayPoint++;
            if (currentWayPoint == wayPointList.Length)
            {
                currentWayPoint = 0;
            }

            targetWayPoint = wayPointList[currentWayPoint];
        }
    }
}
