//���µ��� �ֻ��� �������̽�.
public interface IState<T, W>
{
    void OperateEnter(T value);
    void OperateUpdate(T value);
    void OperateExit(T value);

    /// <summary>
    /// state ��ȯ �� �޽����� ���� ��� ���� ���� ����Ǵ� �޼ҵ�, ���⼭ ������ �ʱ�ȭ���ش�.
    /// </summary>

    void OnAwakeMessage(T value, Telegram<W> telegram);

    /// <summary>
    /// ���� ���������� �۵� ���� ���, �޽����� ������ ����Ǵ� �޼ҵ�
    /// </summary>

    void OnProcessingMessage(T value, Telegram<W> telegram);

    void CheckSwitchStates(T value);
}