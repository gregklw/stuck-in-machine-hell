using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//concrete definition of event binding class
//explicit interface implimentation
public class BusEventBinding<T> : EventBinding<T> where T : IEventWrapper
{
    public BusEventBinding(Action<T> onEvent) : base(onEvent)
    {
    }

    public BusEventBinding(Action onEventNoArgs) : base(onEventNoArgs)
    {
    }
}
