using UnityEngine;

public class StateFollowEnemyFollow : IState<BaseFollowEnemy.State>
{
    BaseFollowEnemy loadFollowEnemy;

    public StateFollowEnemyFollow(BaseFollowEnemy followEnemy)
    {
        loadFollowEnemy = followEnemy;
    }

    public void OnAwakeMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    public void OnProcessingMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    public void OperateEnter()
    {

    }

    public void OperateExit()
    {
       
    }

    public void OperateUpdate()
    {
        CheckSwitchStates();
    }

    public void CheckSwitchStates()
    {
        if (loadFollowEnemy.LoadPlayer == null) return;

        if (!loadFollowEnemy.FollowComponent.IsDistanceLower(loadFollowEnemy.LoadPlayer.transform.position, loadFollowEnemy.Data.FollowMinDistance))
        {
            Vector2 dir = loadFollowEnemy.FollowComponent.ReturnDirVec(loadFollowEnemy.LoadPlayer.transform.position);
            loadFollowEnemy.MoveComponent.Move(dir, loadFollowEnemy.Data.Speed.IntervalValue);
        }
        else
        {
            loadFollowEnemy.SetState(BaseFollowEnemy.State.Stop);
        }
    }

    public void OnSetToGlobalState()
    {
    }
}

public class StateTriangleEnemyFollow : IState<BaseFollowEnemy.State>
{
    YellowTriangleEnemy loadFollowEnemy;

    public StateTriangleEnemyFollow(YellowTriangleEnemy followEnemy)
    {
        loadFollowEnemy = followEnemy;
    }

    public void OnAwakeMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
    }

    public void OnProcessingMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
    }

    public void OperateEnter()
    {

    }

    public void OperateExit()
    {

    }

    public void OperateUpdate()
    {
        CheckSwitchStates();
    }

    public void CheckSwitchStates()
    {
        if (!loadFollowEnemy.FollowComponent.IsDistanceLower(loadFollowEnemy.LoadPlayer.transform.position, loadFollowEnemy.Data.FollowMinDistance))
        {
            Vector2 dir = loadFollowEnemy.FollowComponent.ReturnDirVec(loadFollowEnemy.LoadPlayer.transform.position);
            loadFollowEnemy.MoveComponent.Move(dir, loadFollowEnemy.Data.Speed.IntervalValue);
            loadFollowEnemy.MoveComponent.RotationPlayer(dir, true);
        }
        else
        {
            loadFollowEnemy.SetState(BaseFollowEnemy.State.Stop);
        }
    }

    public void OnSetToGlobalState()
    {
    }
}