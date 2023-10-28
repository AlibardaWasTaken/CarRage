using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DrawPathHandler : MonoBehaviour
{
    public Transform transformRootObject;

    WaypointNode[] waypointNodes;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (transformRootObject == null)
            return;


    }

}
