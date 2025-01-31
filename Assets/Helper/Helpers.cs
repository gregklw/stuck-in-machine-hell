using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    private static readonly Dictionary<float, WaitForSeconds> DictionaryWfs = new();

    ///Returns a WaitForSeconds object for use.
    ///If it doesn't exist then create one.
    ///Dictionary will store one object for each amount of seconds used.

    public static WaitForSeconds GetWaitForSeconds(float seconds)
    {
        //If WaitForSeconds for that specific second value exists, return the object.
        if (DictionaryWfs.TryGetValue(seconds, out var forSeconds)) return forSeconds;

        //Otherwise, create a new WaitForSeconds object and store it in the dictionary along with the seconds value.
        WaitForSeconds waitForSeconds = new WaitForSeconds(seconds);
        DictionaryWfs.Add(seconds, waitForSeconds);
        return waitForSeconds;
    }
}
