using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(LineRenderer))]
public class WayPointCreator : MonoBehaviour
{

    private LineRenderer lineRenderer;
    private WaypointNode[] createdWaypoints;

    [SerializeField]
    private GameObject _WayPointPrefab;

    public GameObject WayPointPrefab { get => _WayPointPrefab; private set => _WayPointPrefab = value; }

    void Awake()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        createdWaypoints = new WaypointNode[lineRenderer.positionCount];

        CreatePoints();
        AssingWaypointsDestinations();
        WayPointCalculator.UpdateWayPointArray();
        lineRenderer.enabled = false;
    }

    public void CreatePoints()
    {
        
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
           var Dotposition = lineRenderer.GetPosition(i);
            Dotposition.z = 0;

           var waypoint = Instantiate(WayPointPrefab, Dotposition, Quaternion.identity, this.gameObject.transform);
            createdWaypoints[i] = waypoint.gameObject.GetComponent<WaypointNode>();
        }
    }

    public void AssingWaypointsDestinations()
    {
        for (int i = 0; i < createdWaypoints.Length - 1; i++)
        {
            createdWaypoints[i].nextWaypointNode = createdWaypoints[i + 1];
        }
        createdWaypoints[createdWaypoints.Length - 1].nextWaypointNode = createdWaypoints[0];
    }


}
