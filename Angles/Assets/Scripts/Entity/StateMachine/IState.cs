//상태들의 최상위 인터페이스.
public interface IState<T>
{
    public abstract void OperateEnter();
    public abstract void OperateUpdate();
    public abstract void OperateExit();

    /// <summary>
    /// state 변환 시 메시지를 보낼 경우 가장 먼저 실행되는 메소드, 여기서 변수를 초기화해준다.
    /// </summary>

    public abstract void OnAwakeMessage(Telegram<T> telegram);

    /// <summary>
    /// 현재 스테이지가 작동 중인 경우, 메시지를 보내면 실행되는 메소드
    /// </summary>

    public abstract void OnProcessingMessage(Telegram<T> telegram);

    public abstract void CheckSwitchStates();

    public abstract void OnSetToGlobalState();
}