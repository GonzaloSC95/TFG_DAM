using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Dictionary<PlayerStatesEnum, PlayerStates> states = new Dictionary<PlayerStatesEnum, PlayerStates>() { { PlayerStatesEnum.PlayerGrounded, new PlayerGrounded() }, { PlayerStatesEnum.PlayerJumping, new PlayerJumping() } };
    PlayerStates currentState;
    private PlayerStatesEnum currentStateEnum;



    private void Start()
    {
        ChangeState(PlayerStatesEnum.PlayerGrounded);
    }

    void Update()
    {
        currentState?.Tick();
    }

    private void ChangeState(PlayerStatesEnum stateToGo)
    {
        currentState?.OnEnd();
        currentStateEnum=stateToGo;
        currentState = states[stateToGo];
        currentState.OnBegin();
    }


    ///Propertys
    public PlayerStatesEnum CurrentStateEnum { get => currentStateEnum; }
}
