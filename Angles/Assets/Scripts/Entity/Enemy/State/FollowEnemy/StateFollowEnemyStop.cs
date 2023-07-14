using UnityEngine;

public class StateFollowEnemyStop : IState<BaseFollowEnemy, BaseFollowEnemy.State>
{
    public void CheckSwitchStates(BaseFollowEnemy value)
    {
        throw new System.NotImplementedException();
    }

    public void OnAwakeMessage(BaseFollowEnemy value, Telegram<BaseFollowEnemy.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    public void OnProcessingMessage(BaseFollowEnemy value, Telegram<BaseFollowEnemy.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    public void OperateEnter(BaseFollowEnemy enemy)
    {
        Debug.Log("Stop");
        enemy.MoveComponent.Stop();
    }

    public void OperateExit(BaseFollowEnemy enemy)
    {
       
    }

    public void OperateUpdate(BaseFollowEnemy enemy)
    {
        if (!enemy.FollowComponent.IsDistanceLower(enemy.LoadPlayer.transform.position, enemy.Data.StopMinDistance))
        {
            enemy.SetState(BaseFollowEnemy.State.Follow);
        }
    }
}