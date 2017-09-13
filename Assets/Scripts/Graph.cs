﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph {

    private Edge[] edges;
    private Dictionary<int, Node> nodes;

    public Graph(Edge[] edges, Node[] nodes)
    {
        this.edges = edges;
        this.nodes = new Dictionary<int, Node>();
        foreach (Node node in nodes)
        {
            this.nodes[node.Id] = node;
        }
    }

    public Edge[] Edges
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