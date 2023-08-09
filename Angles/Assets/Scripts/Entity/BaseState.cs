using UnityEngine;

//상태들의 최상위 인터페이스.
abstract public class BaseState<T>
{
    public abstract void OperateEnter();
    public abstract void OperateUpdate();
    public abstract void OperateExit();

    /// <summary>
    /// state 변환 시 메시지를 보낼 경우 가장 먼저 실행되는 메소드, 여기서 변수를 초기화해준다.
    /// </summary>

    public abstract void OnMessage(Telegram<T> telegram); // 이름 바꾸기

    public virtual void CheckSwitchStates() { } // 여기서 State가 변경되는지 확인

    //////////////////////////////////////////////////////////////////////////////////////// 추가 이벤트

    // 필요한 곳에서만 더 구현하게끔 제작
    public virtual void ReceiveTriggerEnter(Collider2D collider) { }
    public virtual void ReceiveCollisionEnter(Collision2D collision) { }
    public virtual void ReceiveCollisionExit(Collision2D collision) { }

    public virtual void ReceiveOnEnable() { }
    public virtual void ReceiveOnDisable() { }

    public virtual void ReceiveUnderAttack(float damage, Vector2 dir, float thrust) { }
}