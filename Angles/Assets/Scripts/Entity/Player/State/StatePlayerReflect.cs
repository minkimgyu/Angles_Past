using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerReflect : IState<Player, Player.State>
{
    Vector2 savedReflectVec;

    public void CheckSwitchStates(Player value)
    {
        throw new System.NotImplementedException();
    }

    public void OnAwakeMessage(Player value, Telegram<Player.State> telegram)
    {
        if (telegram.SenderStateName == Player.State.Attack)
            savedReflectVec = telegram.Message.dir;
    }

    public void OnProcessingMessage(Player value, Telegram<Player.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    public void OperateEnter(Player player)
    {
        player.DashComponent.QuickEndTask();

        Message<Player.State> message = new Message<Player.State>();
        message.dir = savedReflectVec;
        Telegram<Player.State> telegram = new Telegram<Player.State>(Player.State.Reflect, Player.State.Attack, message);
        player.RevertToPreviousState(telegram);
    }

    public void OperateExit(Player player)
    {

    }

    public void OperateUpdate(Player player)
    {
        
    }
}
