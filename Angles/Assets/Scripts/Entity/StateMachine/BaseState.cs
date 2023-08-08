using UnityEngine;

//���µ��� �ֻ��� �������̽�.
abstract public class BaseState<T>
{
    public abstract void OperateEnter();
    public abstract void OperateUpdate();
    public abstract void OperateExit();

    /// <summary>
    /// state ��ȯ �� �޽����� ���� ��� ���� ���� ����Ǵ� �޼ҵ�, ���⼭ ������ �ʱ�ȭ���ش�.
    /// </summary>

    public abstract void OnMessage(Telegram<T> telegram); // �̸� �ٲٱ�

    public virtual void CheckSwitchStates() { } // ���⼭ State�� ����Ǵ��� Ȯ��

    //////////////////////////////////////////////////////////////////////////////////////// �߰� �̺�Ʈ

    // �ʿ��� �������� �� �����ϰԲ� ����
    public virtual void ReceiveTriggerEnter(Collider2D collider) { }
    public virtual void ReceiveCollisionEnter(Collision2D collision) { }
    public virtual void ReceiveCollisionExit(Collision2D collision) { }

    public virtual void ReceiveOnEnable() { }
    public virtual void ReceiveOnDisable() { }

    public virtual void ReceiveUnderAttack(float damage, Vector2 dir, float thrust) { }
}