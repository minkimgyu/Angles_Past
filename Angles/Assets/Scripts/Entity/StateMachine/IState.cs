//상태들의 최상위 인터페이스.
public interface IState<T, W>
{
    void OperateEnter(T value);
    void OperateUpdate(T value);
    void OperateExit(T value);

    /// <summary>
    /// state 변환 시 메시지를 보낼 경우 가장 먼저 실행되는 메소드, 여기서 변수를 초기화해준다.
    /// </summary>

    void OnAwakeMessage(T value, Telegram<W> telegram);

    /// <summary>
    /// 현재 스테이지가 작동 중인 경우, 메시지를 보내면 실행되는 메소드
    /// </summary>

    void OnProcessingMessage(T value, Telegram<W> telegram);

    void CheckSwitchStates(T value);
}