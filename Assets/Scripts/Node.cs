using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    private int id;
    private Vector3 position;
    private PathFinder pathFinder;

    public Node(int id, Vector3 position) {
        this.id = id;
        this.position = position;
    }

    public int Id
    {
        get
        {
            return id;
        }
    }

    public Vector3 Position
    {
        get
        {
            return position;
        }
    }

    public PathFinder PathFinder
    {
        set
        {
            this.pathFinder = value;
        }
        get
        {
            return pathFinder;
        }
    }
}
