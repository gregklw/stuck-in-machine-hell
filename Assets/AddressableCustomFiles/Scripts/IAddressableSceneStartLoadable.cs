using System.Collections;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

public interface IAddressableSceneStartLoadable
{
    IEnumerator LoadAddressables();
    void Init();
}
