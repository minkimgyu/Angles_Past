public interface IObserver<T, W>
{
    public void OnNotify(T state, W data); // �ϳ��� state, �ڴ� data Ŭ����
}
