using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeContainer : MonoBehaviour
{
    public NodeContainer nodeA;
    public NodeContainer nodeB;
    private Edge edge;
    private bool initialized = false;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (initialized)
        {
            return;
        }
        this.edge = new Edge(nodeA.Node, nodeB.Node);
        initialized = true;
    }

    void OnDrawGizmos()
    {
        if (nodeA != null && nodeB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(nodeA.transform.position, nodeB.transform.position);
        }
    }

    public Edge Edge
    {
        get
        {
            return edge;
        }
    }
}
