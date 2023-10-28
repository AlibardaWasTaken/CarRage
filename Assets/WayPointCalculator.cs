using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WayPointCalculator : MonoBehaviour
{

    private static WaypointNode[] allWayPoints;

    public static WaypointNode FindClosestWayPoint(Transform transformPos)
    {
        return allWayPoints
            .OrderBy(x => Vector3.Distance(transformPos.position, x.transform.position))
            .FirstOrDefault();
    }

    public static void UpdateWayPointArray()
    {
        allWayPoints = FindObjectsOfType<WaypointNode>();
    }
}
