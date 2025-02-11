using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableSpawner : MonoBehaviour, IAddressableSceneStartLoadable, IAddressableUnloadable
{
    [SerializeField] private AssetReferenceGameObject _objectToSpawn;
    private GameObject _loadedPrefab;
    public AssetReferenceGameObject ObjectToSpawn
    {
        get => _objectToSpawn;
        set => _objectToSpawn = value;
    }

#if UNITY_EDITOR
    private GameObject _preview;

    public void SpawnPreview()
    {
        StartCoroutine(SpawnPreviewCoroutine());
    }

    public void DestroyPreview()
    {
        if (_preview != null) DestroyImmediate(_preview);
    }

    private IEnumerator SpawnPreviewCoroutine()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(_objectToSpawn);
        yield return handle;
        if (_preview == null)
        {
            _preview = Instantiate(handle.Result, transform.position, Quaternion.identity);
            _preview.hideFlags = HideFlags.DontSave;
        }
    }
#endif

    private AsyncOperationHandle<GameObject> _loadedHandle;

    private void OnDisable()
    {
        Unload();
    }

    public IEnumerator LoadAddressables()
    {
        _loadedHandle = Addressables.LoadAssetAsync<GameObject>(_objectToSpawn);
        LoadingBar.Instance.RegisterOperation(_loadedHandle);
        yield return _loadedHandle;
        _loadedPrefab = _loadedHandle.Result;
        Init();
    }

    public void Init()
    {
        //Debug.Log(_loadedPrefab + " Initialized" + " " + gameObject.name);
        GameObject instance = Instantiate(_loadedPrefab, transform);
        instance.transform.localPosition = Vector3.zero;
    }

    public void Unload()
    {
        Addressables.Release(_loadedHandle);
    }
}
