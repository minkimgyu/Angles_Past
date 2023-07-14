public interface IObserver<T, W>
{
    public void OnNotify(T state, W data); // 하나는 state, 뒤는 data 클레스
}
