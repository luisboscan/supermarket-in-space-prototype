using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphComponent : MonoBehaviour {

    private Graph graph;
    private Dictionary<int, PathFinder> nodePathFinders;

    void Awake ()
    {
        Node[] nodes = GetNodes();
        Edge[] edges = GetEdges();
        this.graph = new Graph(edges, nodes);
        nodePathFinders = new Dictionary<int, PathFinder>();
        foreach (Node node in nodes)
        {
            nodePathFinders[node.Id] = new PathFinder(graph, node.Id);
        }
    }

    private NodeComponent[] GetNodeComponents()
    {
        NodeComponent[] components = GetComponentsInChildren<NodeComponent>();
        foreach (NodeComponent component in components)
        {
            component.Initialize();
        }
        return components;
    }

    private EdgeComponent[] GetEdgeComponents()
    {
        EdgeComponent[] components = GetComponentsInChildren<EdgeComponent>();
        foreach (EdgeComponent component in components)
        {
            component.Initialize();
        }
        return components;
    }

    private Edge[] GetEdges()
    {
        return GetEdges(GetEdgeComponents());
    }

    private Edge[] GetEdges(EdgeComponent[] components)
    {
        Edge[] array = new Edge[components.Length];
        for (int i = 0; i < components.Length; i++)
        {
            array[i] = components[i].Edge;
        }
        return array;
    }

    private Node[] GetNodes()
    {
        return GetNodes(GetNodeComponents());
    }

    private Node[] GetNodes(NodeComponent[] components)
    {
        Node[] array = new Node[components.Length];
        for (int i=0; i<components.Length; i++)
        {
            array[i] = components[i].Node;
        }
        return array;
    }

    public List<Node> GetPath(int source, int destination)
    {
        return nodePathFinders[source].GetPath(destination);
    }

    public Graph Graph
    {
        get
        {
            return graph;
        }
    }
}
