using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//interface representing the relationship between action and event of type T
public interface IEventBinding<T>
{
    public Action<T> OnEvent { get; set; } //with argument
    public Action OnEventNoArgs { get; set; } //no argument
    public void RaiseEvent(T @event);
}


