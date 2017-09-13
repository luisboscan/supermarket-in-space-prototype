using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeNavigation : MonoBehaviour {

    public float speed = 5f;
    public GraphComponent graphComponent;
    public List<DestinationNodeComponent> destinations;
    public NodeComponent startingNode;

    private Queue<Node> queue;
    private Node lastNode;
    private Node nextNode;

    void Start ()
    {
        if (startingNode == null)
        {
            lastNode = GetClosestNode(graphComponent.Graph.Nodes);
        } else
        {
            lastNode = startingNode.Node;
        }
        transform.position = lastNode.Position;
        queue = new Queue<Node>();
    }
	
	void Update ()
    {
        while (nextNode == null && destinations.Count > 0)
        {
            ChangeDestination();
        }
        if (nextNode == null)
        {
            return;
        }
        MoveToNextPosition();
    }

    private void MoveToNextPosition()
    {
        Vector3 nextPosition = GetNextPosition();
        if (nextPosition == nextNode.Position)
        {
            lastNode = nextNode;
            nextNode = GetNextNode();
        }
        transform.position = nextPosition;
    }

    private void ChangeDestination()
    {
        List<Node> path = graphComponent.GetPath(lastNode.Id, GetNextDestination().id);
        if (path != null)
        {
            foreach (Node node in path)
            {
                queue.Enqueue(node);
            }
            nextNode = GetNextNode();
        }
    }

    private Vector3 GetNextPosition()
    {
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, nextNode.Position, speed * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, nextPosition);
        float maxDistance = Vector3.Distance(transform.position, nextNode.Position);
        if (distance == 0 || distance > maxDistance)
        {
            nextPosition = nextNode.Position;
        }
        return nextPosition;
    }

    private Node GetClosestNode(Dictionary<int, Node> nodes)
    {
        float minDistance = float.MaxValue;
        Node closestNode = null;
        foreach (Node node in nodes.Values)
        {
            float distance = Vector3.Distance(transform.position, node.Position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestNode = node;
            }
        }
        return closestNode;
    }

    private Node GetNextNode()
    {
        if (queue.Count > 0)
        {
            return queue.Dequeue();
        }
        return null;
    }

    private NodeComponent GetNextDestination()
    {
        if (destinations.Count > 0)
        {
            NodeComponent nextDestination = destinations[0];
            destinations.RemoveAt(0);
            return nextDestination;
        }
        return null;
    }

    public void AddDestination(DestinationNodeComponent node)
    {
        destinations.Add(node);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
    }
}