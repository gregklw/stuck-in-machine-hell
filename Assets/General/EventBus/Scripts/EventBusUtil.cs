using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBusUtil
{
    public static IReadOnlyList<Type> EventTypes { get; set; }
    public static IReadOnlyList<Type> EventBusTypes { get; set; }

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    //public static void Initialize()
    //{ 
    //    EventTypes = Pre
    //}
}
