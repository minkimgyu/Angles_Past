using UnityEngine;

abstract public class StateFollowEnemyAttack : BaseState<BaseFollowEnemy.State>
{
    private BaseFollowEnemy loadFollowEnemy;
    bool isInAttackRange;

    protected StateFollowEnemyAttack(BaseFollowEnemy followEnemy)
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

    public override void ReceiveOnEnable() 
    {
        ResetIsInAttackRange();
    }

    public override void ReceiveUnderAttack(float damage, Vector2 dir, float thrust)
    {
        GoToGetDamageState(damage, dir, thrust);
    }

    void GoToGetDamageState(float damage, Vector2 dir, float thrust) // 여기서 빼버리고 follow, stop에만 넣어주기 이 두 가지 경우에만 데미지 스테이트로 넘어가서 넉백됨
    {
        Message<BaseFollowEnemy.State> message = new Message<BaseFollowEnemy.State>();
        message.dir = dir;
        message.damage = damage;
        message.thrust = thrust;

        Telegram<BaseFollowEnemy.State> telegram = new Telegram<BaseFollowEnemy.State>(BaseFollowEnemy.State.Damaged, message);
        loadFollowEnemy.SetState(BaseFollowEnemy.State.Damaged, telegram);
    }

    void ResetIsInAttackRange() // 처음에 근거리인지 원거리인지 채크해서 OperateUpdate에 적용
    {
        if (loadFollowEnemy.LoadPlayer == null) return;

        if (loadFollowEnemy.FollowComponent.IsDistanceLower(loadFollowEnemy.LoadPlayer.transform.position, loadFollowEnemy.FollowEnemyData.SkillUseDistance))
        {
            isInAttackRange = false;
        }
        else 
        {
            isInAttackRange = true;
        }
    }

    public override void OperateUpdate()
    {
        if (loadFollowEnemy.CurrentStateName == BaseFollowEnemy.State.Damaged) return;

        if(loadFollowEnemy.LoadPlayer == null) return;

        if (loadFollowEnemy.FollowComponent.IsDistanceLower(loadFollowEnemy.LoadPlayer.transform.position, loadFollowEnemy.FollowEnemyData.SkillUseDistance) && isInAttackRange == false)
        {
            ExecuteInRangeMethod();
            isInAttackRange = true;
        }
        else if (!loadFollowEnemy.FollowComponent.IsDistanceLower(loadFollowEnemy.LoadPlayer.transform.position, loadFollowEnemy.FollowEnemyData.SkillUseDistance + loadFollowEnemy.FollowEnemyData.SkillUseOffsetDistance) 
            && isInAttackRange == true)
        {
            ExecuteInOutsideMethod();
            isInAttackRange = false;
        }
    }

    public abstract void ExecuteInRangeMethod();


    public abstract void ExecuteInOutsideMethod();
}