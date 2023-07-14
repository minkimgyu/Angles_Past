public interface ISubject<T, W>
{
    void AddObserver(IObserver<T, W> observer);

    void RemoveObserver(IObserver<T, W> observer);

    void NotifyObservers(T state, W data); // 자식 객체에서 본문 작성
}
