using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private Graph graph;
    private Dictionary<int, Node> settledNodes;
    private Dictionary<int, Node> unSettledNodes;
    private Dictionary<int, Node> predecessors;
    private Dictionary<int, float> distance;

    public PathFinder(Graph graph, int source)
    {
        this.graph = graph;
        Initialize(graph.Nodes[source]);
    }

    private void Initialize(Node source)
    {
        settledNodes = new Dictionary<int, Node>();
        unSettledNodes = new Dictionary<int, Node>();
        distance = new Dictionary<int, float>();
        predecessors = new Dictionary<int, Node>();
        distance[source.Id] = 0;
        unSettledNodes[source.Id] = source;
        while (unSettledNodes.Count > 0)
        {
            Node node = getMinimum(unSettledNodes);
            settledNodes[node.Id] = node;
            unSettledNodes.Remove(node.Id);
            FindMinimalDistances(node);
        }
    }

    private void FindMinimalDistances(Node node)
    {
        List<Node> adjacentNodes = getNeighbors(node);
        foreach (Node target in adjacentNodes)
        {
            if (GetShortestDistance(target) > GetShortestDistance(node)
                    + GetDistance(node, target))
            {
                distance[target.Id] = GetShortestDistance(node)
                        + GetDistance(node, target);
                predecessors[target.Id] = node;
                unSettledNodes[target.Id] = target;
            }
        }

    }

    private float GetDistance(Node node, Node target)
    {
        return Vector3.Distance(node.Position, target.Position);
    }

    private List<Node> getNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        foreach (Edge edge in graph.Edges)
        {
            Node otherEnd = edge.GetOtherEnd(node);
            if (otherEnd != null)
            {
                neighbors.Add(otherEnd);
            }
        }
        return neighbors;
    }

    private Node getMinimum(Dictionary<int, Node> nodes)
    {
        Node minimum = null;
        foreach (Node node in nodes.Values)
        {
            if (minimum == null)
            {
                minimum = node;
            }
            else
            {
                if (GetShortestDistance(node) < GetShortestDistance(minimum))
                {
                    minimum = node;
                }
            }
        }
        return minimum;
    }

    private bool isSettled(Node node)
    {
        return settledNodes.ContainsKey(node.Id);
    }

    private float GetShortestDistance(Node destination)
    {
        if (distance.ContainsKey(destination.Id))
        {
            return distance[destination.Id];
        }
        return float.MaxValue;
    }

    /*
     * This method returns the path from the source to the selected target and
     * NULL if no path exists
     */
    public List<Node> GetPath(int target)
    {
        List<Node> path = new List<Node>();
        Node step = graph.Nodes[target];
        // check if a path exists
        if (!predecessors.ContainsKey(step.Id))
        {
            return null;
        }
        path.Add(step);
        while (predecessors.ContainsKey(step.Id))
        {
            step = predecessors[step.Id];
            path.Add(step);
        }
        // Put it into the correct order
        path.Reverse();
        return path;
    }
}