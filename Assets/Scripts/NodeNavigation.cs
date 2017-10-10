using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeNavigation : MonoBehaviour {

    public const string DestinationReachedNotification = "Navigation.DestinationReachedNotification";
    public const string DestinationChangedNotification = "Navigation.DestinationChangedNotification";

    public float speed = 5f;
    public List<Node> destinations;
    public NodeContainer startingNode;
    
    private GraphContainer graphComponent;
    private Queue<Node> queue;
    private Node lastNode;
    private Node currentNode;
    private Node nextNode;
    private Node tempNode;

    void Start ()
    {
        graphComponent = GameObject.FindGameObjectWithTag("Graph").GetComponent<GraphContainer>();
        destinations = new List<Node>();
        if (startingNode == null)
        {
            lastNode = GetClosestNode(graphComponent.Graph.Nodes);
        } else
        {
            lastNode = startingNode.Node;
        }
        currentNode = lastNode;
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
            currentNode = lastNode;
            nextNode = GetNextNode();
            if (nextNode == null)
            {
                gameObject.PostNotification(DestinationReachedNotification, currentNode);
            }
        }
        transform.position = nextPosition;
    }

    private void ChangeDestination()
    {
        List<Node> path = graphComponent.GetPath(currentNode.Id, GetNextDestination().Id);
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

    private Node GetNextDestination()
    {
        if (destinations.Count > 0)
        {
            Node nextDestination = destinations[0];
            destinations.RemoveAt(0);
            return nextDestination;
        }
        return null;
    }

    public void AddDestination(Node node)
    {
        destinations.Add(node);
    }

    public void SetDestination(Node node)
    {
        if (nextNode == null)
        {
            AddDestination(node);
        } else
        {
            if (tempNode == null)
            {
                tempNode = graphComponent.Graph.CreateNode(transform.position);
            }
            else
            {
                tempNode.Position = transform.position;
            }
            graphComponent.Graph.RemoveEdgesAroundNode(tempNode);
            graphComponent.Graph.CreateEdge(tempNode, lastNode);
            graphComponent.Graph.CreateEdge(tempNode, nextNode);
            tempNode.PathFinder.Initialize();
            Reset();
            currentNode = tempNode;
            AddDestination(node);
        }
        gameObject.PostNotification(DestinationChangedNotification, node);
    }

    public Node LastNode
    {
        set
        {
            lastNode = value;
        }
        get
        {
            return lastNode;
        }
    }

    public Node NextNode
    {
        get
        {
            return nextNode;
        }
    }

    public Node CurrentNode
    {
        get
        {
            return currentNode;
        }
    }

    public void Reset()
    {
        nextNode = null;
        destinations.Clear();
        queue.Clear();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
    }
}