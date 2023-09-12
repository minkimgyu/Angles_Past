public class StateFollowEnemyDie : BaseState<BaseFollowEnemy.State>
{
    BaseFollowEnemy loadFollowEnemy;

    public StateFollowEnemyDie(BaseFollowEnemy followEnemy)
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
    }
}