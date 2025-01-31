using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//T represents some form of event
public abstract class EventChannelObject<T> : ScriptableObject where T : IEventWrapper
{
    public Action<T> OnEventRaised;

}
