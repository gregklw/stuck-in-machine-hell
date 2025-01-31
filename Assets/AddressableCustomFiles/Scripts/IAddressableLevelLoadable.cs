using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAddressableLevelLoadable
{
    IEnumerator Setup();
    IEnumerator Activate();
}
