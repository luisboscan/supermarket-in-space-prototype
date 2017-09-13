using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge {

    private Node nodeA;
    private Node nodeB;

    public Edge(Node nodeA, Node nodeB)
    {
        this.nodeA = nodeA;
        this.nodeB = nodeB;
    }

    public Node GetOtherEnd(Node start)
    {
        if (nodeA.Id.Equals(start.Id))
        {
            return nodeB;
        }
        if (nodeB.Id.Equals(start.Id))
        {
            return nodeA;
        }
        return null;
    }
}
