public interface ISubject<T, W>
{
    void AddObserver(IObserver<T, W> observer);

    void RemoveObserver(IObserver<T, W> observer);

    void NotifyObservers(T state, W data); // �ڽ� ��ü���� ���� �ۼ�
}
