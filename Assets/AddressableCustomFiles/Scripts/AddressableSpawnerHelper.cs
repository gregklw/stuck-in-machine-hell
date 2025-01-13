#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

[InitializeOnLoad]
public static class AddressableSpawnerHelper
{
    [MenuItem("AddressableSpawnHelper/Create Addressable Spawner")]
    private static void ValidateMenuActionPrefab()
    {
        //var isPrefab = PrefabUtility.GetPrefabParent(Selection.activeGameObject) != null;
        //return isPrefab;
        Transform[] selectedObjects = Selection.transforms;
        GameObject selectedGameObject = Selection.activeGameObject;

        HashSet<GameObject> scannedObjects = new HashSet<GameObject>();

        foreach (Transform target in selectedObjects)
        {
            scannedObjects.Add(target.gameObject);
            Debug.Log(target.name);
            GameObject go = new GameObject();
            go.transform.SetParent(target.parent);
            go.transform.SetAsFirstSibling();
            go.name = $"AddressablesSpawner_{target.name}";
            go.transform.position = target.position;
            AddressableSpawner spawner = go.AddComponent<AddressableSpawner>();
            string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromSource(target.gameObject)));
            AssetReferenceGameObject assetReferenceGameObject = new AssetReferenceGameObject(guid);
            spawner.ObjectToSpawn = assetReferenceGameObject;
        }
    }

    [MenuItem("AddressableSpawnHelper/Spawn Preview for all AddressableSpawners")]
    public static void TriggerPreview()
    {
        AddressableSpawner[] spawners = GameObject.FindObjectsOfType<AddressableSpawner>();
        Array.ForEach(spawners, spawner =>
        {
            //Debug.Log(spawner.gameObject.name);
            spawner.SpawnPreview();
        }
        );
    }
    
    [MenuItem("AddressableSpawnHelper/Destroy Spawner Previews")]
    private static void DestroySpawnPreviews()
    {
        AddressableSpawner[] spawners = GameObject.FindObjectsOfType<AddressableSpawner>();
        Array.ForEach(spawners, spawner =>
        {
            spawner.DestroyPreview();
        }
        );
    }


    // register an event handler when the class is initialized
    static AddressableSpawnerHelper()
    {
        EditorApplication.playModeStateChanged += LogPlayModeState;
    }

    private static void LogPlayModeState(PlayModeStateChange state)
    {
        DestroySpawnPreviews();
    }
}
#endif