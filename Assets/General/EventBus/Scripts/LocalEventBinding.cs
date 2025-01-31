using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalEventBinding<T> : EventBinding<T> where T : IEventWrapper
{
    public LocalEventBinding() : base()
    {
    }
    public LocalEventBinding(Action<T> onEvent) : base(onEvent)
    {
    }

    public LocalEventBinding(Action onEventNoArgs) : base(onEventNoArgs)
    {
    }
}
