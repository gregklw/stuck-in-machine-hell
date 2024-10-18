using System;

public abstract class EventBinding<T> : IEventBinding<T> where T : IEventWrapper
{
    //empty delegates to avoid null checks
    private Action<T> _onEvent = _ => { };
    private Action _onEventNoArgs = () => { };
    public Action<T> OnEvent
    {
        get => _onEvent;
        set => _onEvent = value;
    }
    public Action OnEventNoArgs
    {
        get => _onEventNoArgs;
        set => _onEventNoArgs = value;
    }

    //constructors
    public EventBinding() { }
    public EventBinding(Action<T> onEvent) => this._onEvent = onEvent;
    public EventBinding(Action onEventNoArgs) => this._onEventNoArgs = onEventNoArgs;

    //add remove no arg actions
    public void Add(Action onEventNoArgs) => OnEventNoArgs += onEventNoArgs;
    public void Remove(Action onEventNoArgs) => OnEventNoArgs -= onEventNoArgs;

    //add remove actions
    public void Add(Action<T> onEvent) => OnEvent += onEvent;
    public void Remove(Action<T> onEvent) => OnEvent -= onEvent;

    public void RaiseEvent(T @event)
    {
        OnEvent.Invoke(@event);
        OnEventNoArgs.Invoke();
    }
}
