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

    void GoToGetDamageState(float damage, Vector2 dir, float thrust) // ���⼭ �������� follow, stop���� �־��ֱ� �� �� ���� ��쿡�� ������ ������Ʈ�� �Ѿ�� �˹��
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