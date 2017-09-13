using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class PlayerStateMachine : MonoBehaviour {
    
    public PlayerInput playerInput;
    public PlayerStates startingState = PlayerStates.Idle;
    public NodeNavigation playerNavigation;
    public GraphContainer graphContainer;

    private StateMachine<PlayerStates> fsm;

    private bool initialized;

    void Awake()
    {
        if (!initialized) Init();
    }

    void Init()
    {
        fsm = StateMachine<PlayerStates>.Initialize(this, startingState);
        initialized = true;
    }

    void Idle_Update()
    {
        SelectionArea selectionArea;
        if (playerInput.selected && (selectionArea = GetSelectedArea()) != null)
        {
            playerNavigation.Reset();
            //Node tempNode = graphContainer.Graph.CreateTempNode(playerNavigation.transform.position);
            //playerNavigation.AddDestination(tempNode);
            playerNavigation.AddDestination(selectionArea.destinationNodeContainer.Node);
        }
    }

    public StateMachine<PlayerStates> FSM
    {
        get {
            if (!initialized)
            {
                Init();
            }
            return fsm; }
    }

    private SelectionArea GetSelectedArea()
    {
        RaycastHit[] hits = playerInput.GetCursorOverObjects();
        foreach(RaycastHit hit in hits)
        {
            SelectionArea component = hit.collider.gameObject.GetComponent<SelectionArea>();
            if (component != null)
            {
                return component;
            }
        }
        return null;
    }
}
