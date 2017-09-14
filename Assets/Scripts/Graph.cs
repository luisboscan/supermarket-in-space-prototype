using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph {

    private List<Edge> edges;
    private Dictionary<int, Node> nodes;

    public Graph(Edge[] edges, Node[] nodes)
    {
        this.edges = new List<Edge>();
        foreach (Edge edge in edges)
        {
            this.edges.Add(edge);
        }
        this.nodes = new Dictionary<int, Node>();
        foreach (Node node in nodes)
        {
            this.nodes[node.Id] = node;
        }
    }

    public Node CreateNode(Vector3 position)
    {
        Node node = new Node(++NodeContainer.lastId, position);
        node.PathFinder = new PathFinder(this, node.Id);
        nodes[node.Id] = node;
        return node;
    }

    public Edge CreateEdge(Node nodeA, Node nodeB)
    {
        Edge edge = new Edge(nodeA, nodeB);
        edges.Add(edge);
        return edge;
    }

    public void RemoveEdgesAroundNode(Node node)
    {
        for(int i=edges.Count-1; i>=0; i--)
        {
            if (edges[i].GetOtherEnd(node) != null)
            {
                edges.RemoveAt(i);
            }
        }
    }

    public List<Edge> Edges
    {
        get
        {
            return edges;
        }
    }

    public Dictionary<int, Node> Nodes
    {
        get
        {
            return nodes;
        }
    }
}