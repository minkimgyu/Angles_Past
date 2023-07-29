using UnityEngine;

public class StateFollowEnemyStop : IState<BaseFollowEnemy.State>
{
    BaseFollowEnemy loadFollowEnemy;

    public StateFollowEnemyStop(BaseFollowEnemy followEnemy) 
    { 
        loadFollowEnemy = followEnemy; 
    }

    public void CheckSwitchStates()
    {
        throw new System.NotImplementedException();
    }

    public void OnAwakeMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    public void OnProcessingMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    public void OnSetToGlobalState()
    {
    }

    public void OperateEnter()
    {
        loadFollowEnemy.MoveComponent.Stop();
    }

    public void OperateExit()
    {
       
    }

    public void OperateUpdate()
    {
        if (loadFollowEnemy.LoadPlayer == null) return;

        if (!loadFollowEnemy.FollowComponent.IsDistanceLower(loadFollowEnemy.LoadPlayer.transform.position, loadFollowEnemy.Data.StopMinDistance))
        {
            loadFollowEnemy.SetState(BaseFollowEnemy.State.Follow);
        }
    }
}

public class StateTriangleEnemyStop : IState<BaseFollowEnemy.State>
{
    YellowTriangleEnemy loadFollowEnemy;

    public StateTriangleEnemyStop(YellowTriangleEnemy followEnemy)
    {
        loadFollowEnemy = followEnemy;
    }

    public void CheckSwitchStates()
    {
        throw new System.NotImplementedException();
    }

    public void OnAwakeMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    public void OnProcessingMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
        throw new System.NotImplementedException();
    }

    public void OnSetToGlobalState()
    {
    }

    public void OperateEnter()
    {
        loadFollowEnemy.MoveComponent.Stop();
    }

    public void OperateExit()
    {

    }

    public void OperateUpdate()
    {
        Vector2 dir = loadFollowEnemy.FollowComponent.ReturnDirVec(loadFollowEnemy.LoadPlayer.transform.position);
        loadFollowEnemy.MoveComponent.RotationPlayer(dir.normalized, true);

        if (!loadFollowEnemy.FollowComponent.IsDistanceLower(loadFollowEnemy.LoadPlayer.transform.position, loadFollowEnemy.Data.StopMinDistance))
        {
            loadFollowEnemy.SetState(BaseFollowEnemy.State.Follow);
        }
    }
}