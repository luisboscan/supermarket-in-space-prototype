using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class NodeContainer : MonoBehaviour
{
    private static int lastId;
    public int id;
    private Node node;
    private bool initialized = false;

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (initialized)
        {
            return;
        }
        this.id = ++lastId;
        this.node = new Node(id, transform.position);
        initialized = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }

    public Node Node
    {
        get
        {
            return node;
        }
    }
}