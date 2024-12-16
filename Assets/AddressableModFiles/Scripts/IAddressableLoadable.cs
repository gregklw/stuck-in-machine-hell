using System.Collections;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

public interface IAddressableLoadable
{
    IEnumerator LoadAddressables(List<AsyncOperationHandle> handles);
    void Init();
}
