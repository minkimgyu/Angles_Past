using System;

public class BasicCallEvent
{
    private Action callEvent;

    public void ExecuteCallEvent()
    {
        callEvent?.Invoke();
    }

    public void AddCallEvent(Action action) => callEvent += action;
    public void RemoveCallEvent(Action action) => callEvent -= action;
}
