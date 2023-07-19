public class StateFollowEnemyDie : IState<BaseFollowEnemy.State>
{
    BaseFollowEnemy loadFollowEnemy;

    public StateFollowEnemyDie(BaseFollowEnemy followEnemy)
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

    }

    public void OperateExit()
    {

    }

    public void OperateUpdate()
    {

    }
}