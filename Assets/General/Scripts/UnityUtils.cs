using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class UnityUtils
{
    /// <summary>
    /// Will instantiate an object disabled preventing it from calling Awake/OnEnable.
    /// </summary>
    public static T InstantiateDisabled<T>(T original, Transform parent = null, bool worldPositionStays = false) where T : Object
    {
        if (!GetActiveState(original))
        {
            return Object.Instantiate(original, parent, worldPositionStays);
        }

        (GameObject coreObject, Transform coreObjectTransform) = CreateDisabledCoreObject(parent);
        T instance = Object.Instantiate(original, coreObjectTransform, worldPositionStays);
        SetActiveState(instance, false);
        SetParent(instance, parent, worldPositionStays);
        Object.Destroy(coreObject);
        return instance;
    }

    /// <summary>
    /// Will instantiate an object disabled preventing it from calling Awake/OnEnable.
    /// </summary>
    public static T InstantiateDisabled<T>(T original, Vector3 position, Quaternion rotation, Transform parent = null) where T : Object
    {
        if (!GetActiveState(original))
        {
            return Object.Instantiate(original, position, rotation, parent);
        }

        (GameObject coreObject, Transform coreObjectTransform) = CreateDisabledCoreObject(parent);
        T instance = Object.Instantiate(original, position, rotation, coreObjectTransform);
        SetActiveState(instance, false);
        SetParent(instance, parent, false);
        Object.Destroy(coreObject);
        return instance;
    }

    private static (GameObject coreObject, Transform coreObjectTransform) CreateDisabledCoreObject(Transform parent = null)
    {
        GameObject coreObject = new GameObject(string.Empty);
        coreObject.SetActive(false);
        Transform coreObjectTransform = coreObject.transform;
        coreObjectTransform.SetParent(parent);

        return (coreObject, coreObjectTransform);
    }

    private static bool GetActiveState<T>(T @object) where T : Object
    {
        switch (@object)
        {
            case GameObject gameObject:
                {
                    return gameObject.activeSelf;
                }
            case Component component:
                {
                    return component.gameObject.activeSelf;
                }
            default:
                {
                    return false;
                }
        }
    }

    private static void SetActiveState<T>(T @object, bool state) where T : Object
    {
        switch (@object)
        {
            case GameObject gameObject:
                {
                    gameObject.SetActive(state);

                    break;
                }
            case Component component:
                {
                    component.gameObject.SetActive(state);

                    break;
                }
        }
    }

    private static void SetParent<T>(T @object, Transform parent, bool worldPositionStays) where T : Object
    {
        switch (@object)
        {
            case GameObject gameObject:
                {
                    gameObject.transform.SetParent(parent, worldPositionStays);

                    break;
                }
            case Component component:
                {
                    component.transform.SetParent(parent, worldPositionStays);

                    break;
                }
        }
    }

    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    //public static T FindObjectOfInterface<T>()
    //{
    //    Component[] components = GameObject.FindObjectsOfType<Component>();

    //    foreach (Component component in components)
    //    {
    //        if (component.GetType() == typeof(T))
    //        {
    //            return component as T;
    //        }
    //    }
    //    return null;
    //}
    public static T FindInterface<T>()
    {
        GameObject[] rootGameObjects;
        int sceneCount = SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; i++)
        {
            rootGameObjects = SceneManager.GetSceneAt(i).GetRootGameObjects();

            foreach (var rootGameObject in rootGameObjects)
            {
                //Debug.Log($"Name of GameObject: {rootGameObject} ");
                T childInterface = rootGameObject.GetComponentInChildren<T>();
                //Debug.Log($"== null: {childInterface == null} | Equals default: {childInterface?.Equals(default)} | Equals default(T): {childInterface?.Equals(default(T))}");
                if (childInterface != null) return childInterface;
            }
        }
        return default;
    }

    public static T FindInterfaceInScene<T>(Scene scene)
    {
        GameObject[] rootGameObjects = scene.GetRootGameObjects();

        foreach (var rootGameObject in rootGameObjects)
        {
            //Debug.Log($"Name of GameObject: {rootGameObject} ");
            T childInterface = rootGameObject.GetComponentInChildren<T>();
            return childInterface;
        }

        return default;
    }

    //public static T[] FindObjectsOfInterface<T>() where T : Object
    //{
    //    Component[] components = GameObject.FindObjectsOfType<Component>();
    //    List<T> interfaceList = new List<T>();

    //    foreach (Component component in components)
    //    {
    //        if (component.GetType() == typeof(T))
    //        {
    //            interfaceList.Add(component as T);
    //        }
    //    }
    //    return interfaceList.ToArray();
    //}

    public static List<T> FindInterfaces<T>()
    {
        List<T> interfaces = new List<T>();

        GameObject[] rootGameObjects;

        int sceneCount = SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; i++)
        {
            rootGameObjects = SceneManager.GetSceneAt(i).GetRootGameObjects();

            foreach (var rootGameObject in rootGameObjects)
            {
                //Debug.Log($"Name of GameObject: {rootGameObject} ");
                T[] childrenInterfaces = rootGameObject.GetComponentsInChildren<T>();
                foreach (var childInterface in childrenInterfaces)
                {
                    interfaces.Add(childInterface);
                }
            }
        }
        return interfaces;
    }

    public static List<T> FindInterfacesInScene<T>(Scene scene)
    {
        List<T> interfaces = new List<T>();
        GameObject[] rootGameObjects = scene.GetRootGameObjects();

        foreach (var rootGameObject in rootGameObjects)
        {
            //Debug.Log($"Name of GameObject: {rootGameObject} ");
            T[] childrenInterfaces = rootGameObject.GetComponentsInChildren<T>();
            foreach (var childInterface in childrenInterfaces)
            {
                interfaces.Add(childInterface);
            }
        }

        return interfaces;
    }
}