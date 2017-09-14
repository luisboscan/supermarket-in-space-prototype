using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class NodeContainer : MonoBehaviour
{
    public static int lastId;
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
        this.node = new Node(++lastId, transform.position);
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