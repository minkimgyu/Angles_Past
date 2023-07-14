public class StateFollowEnemyAttack : IState<BaseFollowEnemy, BaseFollowEnemy.State>
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
       
    }

    public void OperateExit(BaseFollowEnemy enemy)
    {

    }

    public void OperateUpdate(BaseFollowEnemy enemy)
    {
        if (enemy.FollowComponent.IsDistanceLower(enemy.LoadPlayer.transform.position, enemy.Data.SkillMinDistance))
        {
            // 스킬 공격 함수 추가
        }
    }
}