using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the event bus. It will be the hub of activity for a event type.
public static class EventBus<T> where T : IEventWrapper
{
    //Store all of the bindings relating to the type of an event and put all those in a hash set.
    static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();

    //Register and deregister a binding from the bus.
    public static void Register(BusEventBinding<T> binding) => bindings.Add(binding);
    public static void Deregister(BusEventBinding<T> binding) => bindings.Remove(binding);

    //need a public method that will accept an event and invoke both the Actions on the binding
    public static void Raise(T @event)
    {
        foreach (var binding in bindings)
        {
            //no '?' needed because null checks not needed for delegates
            binding.RaiseEvent(@event);
        }
    }
}
