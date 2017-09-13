using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class PlayerStateMachine : MonoBehaviour {
    
    public PlayerInput playerInput;
    public PlayerStates startingState = PlayerStates.Idle;
    public NodeNavigation playerNavigation;

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

    void Idle_Enter()
    {
        
    }

    void Idle_Update()
    {
        if (playerInput.selected)
        {
            RaycastHit[] hit = playerInput.GetCursorOverObjects();
            if (hit.Length > 0)
            {
                playerNavigation.AddDestination(hit[0].collider.gameObject.GetComponent<Room>().destinationNodeComponent);
            }
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
}
