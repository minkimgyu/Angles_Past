using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerReflect : IState<Player.State>
{
    Vector2 savedReflectVec;
    Player loadPlayer;

    public StatePlayerReflect(Player player)
    {
        loadPlayer = player;
    }

    public void CheckSwitchStates()
    {
        throw new System.NotImplementedException();
    }

    public void OnAwakeMessage(Telegram<Player.State> telegram)
    {
        if (telegram.SenderStateName == Player.State.Attack)
            savedReflectVec = telegram.Message.dir;
    }

    public void OnProcessingMessage(Telegram<Player.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    public void OnSetToGlobalState()
    {
    }

    public void OperateEnter()
    {
        loadPlayer.DashComponent.QuickEndTask();

        Message<Player.State> message = new Message<Player.State>();
        message.dir = savedReflectVec;
        Telegram<Player.State> telegram = new Telegram<Player.State>(Player.State.Reflect, Player.State.Attack, message);

        SoundManager.Instance.PlaySFX(loadPlayer.transform.position, "Reflect", 0.3f);

        loadPlayer.RevertToPreviousState(telegram);
    }

    public void OperateExit()
    {

    }

    public void OperateUpdate()
    {
        
    }
}
