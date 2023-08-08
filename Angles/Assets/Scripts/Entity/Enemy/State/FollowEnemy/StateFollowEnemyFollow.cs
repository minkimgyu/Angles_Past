using UnityEngine;

public class StateFollowEnemyFollow : BaseState<BaseFollowEnemy.State>
{
    BaseFollowEnemy loadFollowEnemy;

    public StateFollowEnemyFollow(BaseFollowEnemy followEnemy)
    {
        loadFollowEnemy = followEnemy;
    }

    public override void OnMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
    }

    public override void OperateEnter()
    {
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
        if (loadFollowEnemy.LoadPlayer == null) return;

        if (!loadFollowEnemy.FollowComponent.IsDistanceLower(loadFollowEnemy.LoadPlayer.transform.position, loadFollowEnemy.FollowEnemyData.FollowMinDistance))
        {
            Vector2 dir = loadFollowEnemy.FollowComponent.ReturnDirVec(loadFollowEnemy.LoadPlayer.transform.position);
            loadFollowEnemy.MoveComponent.Move(dir, loadFollowEnemy.HealthData.Speed.IntervalValue);
        }
        else
        {
            loadFollowEnemy.SetState(BaseFollowEnemy.State.Stop);
        }
    }
}

public class StateTriangleEnemyFollow : BaseState<BaseFollowEnemy.State>
{
    YellowTriangleEnemy loadFollowEnemy;

    public StateTriangleEnemyFollow(YellowTriangleEnemy followEnemy)
    {
        loadFollowEnemy = followEnemy;
    }

    public override void OnMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
    }

    public override void OperateEnter()
    {
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
        if (!loadFollowEnemy.FollowComponent.IsDistanceLower(loadFollowEnemy.LoadPlayer.transform.position, loadFollowEnemy.FollowEnemyData.FollowMinDistance))
        {
            Vector2 dir = loadFollowEnemy.FollowComponent.ReturnDirVec(loadFollowEnemy.LoadPlayer.transform.position);
            loadFollowEnemy.MoveComponent.Move(dir, loadFollowEnemy.HealthData.Speed.IntervalValue);
            loadFollowEnemy.MoveComponent.RotationPlayer(dir, true);
        }
        else
        {
            loadFollowEnemy.SetState(BaseFollowEnemy.State.Stop);
        }
    }
}