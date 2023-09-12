using UnityEngine;

public class StateFollowEnemyStop : BaseState<BaseFollowEnemy.State>
{
    BaseFollowEnemy loadFollowEnemy;

    public StateFollowEnemyStop(BaseFollowEnemy followEnemy) 
    { 
        loadFollowEnemy = followEnemy; 
    }

    public override void OnMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
    }

    public override void OperateEnter()
    {
        loadFollowEnemy.MoveComponent.Stop();
    }

    public override void OperateExit()
    {
    }

    public override void CheckSwitchStates()  // 여기서 State가 변경되는지 확인
    {
        if (loadFollowEnemy.LoadPlayer == null) return;

        float stopDistance = loadFollowEnemy.FollowDistance.IntervalValue + loadFollowEnemy.FollowOffsetDistance.IntervalValue;

        if (!loadFollowEnemy.FollowComponent.IsDistanceLower(loadFollowEnemy.LoadPlayer.transform.position, stopDistance))
        {
            loadFollowEnemy.SetState(BaseFollowEnemy.State.Follow);
        }
    }

    public override void OperateUpdate()
    {
    }
}

public class StateTriangleEnemyStop : BaseState<BaseFollowEnemy.State>
{
    YellowTriangleEnemy loadFollowEnemy;

    public StateTriangleEnemyStop(YellowTriangleEnemy followEnemy)
    {
        loadFollowEnemy = followEnemy;
    }

    public override void CheckSwitchStates()
    {
        float stopDistance = loadFollowEnemy.FollowDistance.IntervalValue + loadFollowEnemy.FollowOffsetDistance.IntervalValue;

        if (!loadFollowEnemy.FollowComponent.IsDistanceLower(loadFollowEnemy.LoadPlayer.transform.position, stopDistance))
        {
            loadFollowEnemy.SetState(BaseFollowEnemy.State.Follow);
        }
    }

    public override void OnMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
    }

    public override void OperateEnter()
    {
        loadFollowEnemy.MoveComponent.Stop();
    }

    public override void OperateExit()
    {
    }

    public override void OperateUpdate()
    {
        Vector2 dir = loadFollowEnemy.FollowComponent.ReturnDirVec(loadFollowEnemy.LoadPlayer.transform.position);
        loadFollowEnemy.MoveComponent.RotationPlayer(dir.normalized, true);
    }
}