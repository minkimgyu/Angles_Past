using UnityEngine;

abstract public class StateFollowEnemyAttack : IState<BaseFollowEnemy.State>
{
    private BaseFollowEnemy loadFollowEnemy;
    bool isInAttackRange;

    protected StateFollowEnemyAttack(BaseFollowEnemy followEnemy)
    {
        loadFollowEnemy = followEnemy;
    }

    public void CheckSwitchStates()
    {
    }

    public void OnAwakeMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
    }

    public void OnProcessingMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
    }

    public void OnSetToGlobalState()
    {
        loadFollowEnemy.WhenEnable += ResetIsInAttackRange;
        loadFollowEnemy.UnderAttackAction += GoToGetDamageState;
    }

    public void OperateEnter()
    {
    }

    public void OperateExit()
    {
    }

    void GoToGetDamageState(float damage, Vector2 dir, float thrust)
    {
        Message<BaseFollowEnemy.State> message = new Message<BaseFollowEnemy.State>();
        message.dir = dir;
        message.damage = damage;
        message.thrust = thrust;

        Telegram<BaseFollowEnemy.State> telegram = new Telegram<BaseFollowEnemy.State>(BaseFollowEnemy.State.Damaged, message);
        loadFollowEnemy.SetState(BaseFollowEnemy.State.Damaged, telegram);
    }

    void ResetIsInAttackRange() // ó���� �ٰŸ����� ���Ÿ����� äũ�ؼ� OperateUpdate�� ����
    {
        if (loadFollowEnemy.LoadPlayer == null) return;

        if (loadFollowEnemy.FollowComponent.IsDistanceLower(loadFollowEnemy.LoadPlayer.transform.position, loadFollowEnemy.Data.SkillUseDistance))
        {
            isInAttackRange = false;
        }
        else 
        {
            isInAttackRange = true;
        }
    }

    public virtual void OperateUpdate()
    {
        if (loadFollowEnemy.CurrentStateName == BaseFollowEnemy.State.Damaged) return;

        if(loadFollowEnemy.LoadPlayer == null) return;

        if (loadFollowEnemy.FollowComponent.IsDistanceLower(loadFollowEnemy.LoadPlayer.transform.position, loadFollowEnemy.Data.SkillUseDistance) && isInAttackRange == false)
        {
            ExecuteInRangeMethod();
            isInAttackRange = true;
        }
        else if (!loadFollowEnemy.FollowComponent.IsDistanceLower(loadFollowEnemy.LoadPlayer.transform.position, loadFollowEnemy.Data.SkillUseDistance + loadFollowEnemy.Data.SkillUseOffsetDistance) 
            && isInAttackRange == true)
        {
            ExecuteInOutsideMethod();
            isInAttackRange = false;
        }
    }

    public abstract void ExecuteInRangeMethod();


    public abstract void ExecuteInOutsideMethod();
}