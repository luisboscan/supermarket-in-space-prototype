using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingSection : MonoBehaviour {

    public ShoppingSectionType shoppingSectionType;
    private Resource resource;
    private DestinationNodeContainer destinationNodeContainer;

    void Start () {
        resource = GetComponent<Resource>();
        destinationNodeContainer = GetComponent<DestinationNodeContainer>();
    }

    public Node GetNode () {
        return destinationNodeContainer.Node;
    }

    public Resource Resource
    {
        get { return resource; }
    }
}
