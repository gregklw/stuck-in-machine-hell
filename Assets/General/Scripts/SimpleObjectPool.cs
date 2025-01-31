using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleObjectPool<T> : MonoBehaviour
{
    private Stack<T> _objectPool = new Stack<T>();

    protected abstract void CreateObjectIfEmpty();

    protected T GetObjectFromPool()
    {
        if (_objectPool.Count == 0) CreateObjectIfEmpty();
        return _objectPool.Pop();
    }

    protected void AddObjectToPool(T objectToAdd)
    { 
        _objectPool.Push(objectToAdd);
    }
}
