using UnityEngine;

public class StateFollowEnemyFollow : IState<BaseFollowEnemy, BaseFollowEnemy.State>
{
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

    }

    public void OperateExit(BaseFollowEnemy enemy)
    {
       
    }

    public void OperateUpdate(BaseFollowEnemy enemy)
    {
        CheckSwitchStates(enemy);
    }

    public void CheckSwitchStates(BaseFollowEnemy enemy)
    {
        if (!enemy.FollowComponent.IsDistanceLower(enemy.LoadPlayer.transform.position, enemy.Data.FollowMinDistance))
        {
            Vector2 dir = enemy.FollowComponent.ReturnDirVec(enemy.LoadPlayer.transform.position);
            enemy.MoveComponent.Move(dir, enemy.Data.Speed);
        }
        else
        {
            enemy.SetState(BaseFollowEnemy.State.Stop);
        }
    }
}