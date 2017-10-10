using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphContainer : MonoBehaviour {

    private Graph graph;
    private Dictionary<ShoppingSectionType, ShoppingSection> shoppingSections;

    void Awake ()
    {
        Node[] nodes = GetNodes();
        Edge[] edges = GetEdges();
        this.graph = new Graph(edges, nodes);
        foreach (Node node in nodes)
        {
            node.PathFinder = new PathFinder(graph, node.Id);
            node.PathFinder.Initialize();
        }
        ShoppingSection[] shoppingSectionArray = GetComponentsInChildren<ShoppingSection>();
        shoppingSections = new Dictionary<ShoppingSectionType, ShoppingSection>();
        foreach (ShoppingSection shoppingSection in shoppingSectionArray)
        {
            shoppingSections.Add(shoppingSection.shoppingSectionType, shoppingSection);
        }
    }
    
    public ShoppingSection GetShoppingSectionByType(ShoppingSectionType shoppingSectionType)
    {
        return shoppingSections.ContainsKey(shoppingSectionType) ? shoppingSections[shoppingSectionType] : null;
    }

    private NodeContainer[] GetNodeComponents()
    {
        NodeContainer[] components = GetComponentsInChildren<NodeContainer>();
        foreach (NodeContainer component in components)
        {
            component.Initialize();
        }
        return components;
    }

    private EdgeContainer[] GetEdgeComponents()
    {
        EdgeContainer[] components = GetComponentsInChildren<EdgeContainer>();
        foreach (EdgeContainer component in components)
        {
            component.Initialize();
        }
        return components;
    }

    private Edge[] GetEdges()
    {
        return GetEdges(GetEdgeComponents());
    }

    private Edge[] GetEdges(EdgeContainer[] components)
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

    private Node[] GetNodes(NodeContainer[] components)
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
        return graph.Nodes[source].PathFinder.GetPath(destination);
    }

    public NodeContainer GetNodeContainerForNode(Node node)
    {
        foreach (NodeContainer nodeContainer in GetNodeComponents())
        {
            if (nodeContainer.Node.Id == node.Id)
            {
                return nodeContainer;
            }
        }
        return null;
    }

    public NodeContainer GetPaymentNode()
    {
        return GameObject.FindGameObjectWithTag("Payment").GetComponent<NodeContainer>();
    }

    public NodeContainer GetExitNode()
    {
        return GameObject.FindGameObjectWithTag("Exit").GetComponent<NodeContainer>();
    }

    public Graph Graph
    {
        get
        {
            return graph;
        }
    }
}
