//���µ��� �ֻ��� �������̽�.
public interface IState<T>
{
    public abstract void OperateEnter();
    public abstract void OperateUpdate();
    public abstract void OperateExit();

    /// <summary>
    /// state ��ȯ �� �޽����� ���� ��� ���� ���� ����Ǵ� �޼ҵ�, ���⼭ ������ �ʱ�ȭ���ش�.
    /// </summary>

    public abstract void OnAwakeMessage(Telegram<T> telegram);

    /// <summary>
    /// ���� ���������� �۵� ���� ���, �޽����� ������ ����Ǵ� �޼ҵ�
    /// </summary>

    public abstract void OnProcessingMessage(Telegram<T> telegram);

    public abstract void CheckSwitchStates();

    public abstract void OnSetToGlobalState();
}