using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableSpawner : MonoBehaviour, IAddressableSceneStartLoadable, IAddressableUnloadable
{
    [SerializeField] private AssetReferenceGameObject _objectToSpawn;
    private GameObject _loadedPrefab;
    private GameObject _loadedInstance;
    public AssetReferenceGameObject ObjectToSpawn
    {
        get => _objectToSpawn;
        set => _objectToSpawn = value;
    }

    private AsyncOperationHandle<GameObject> _loadedHandle;

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
        _loadedInstance = Instantiate(_loadedPrefab, transform);
        _loadedInstance.transform.localPosition = Vector3.zero;
    }

    public void Unload()
    {
        if (_loadedHandle.IsValid())
        {
            Debug.Log($"Unloading: {_loadedHandle.Result}, {Addressables.ReleaseInstance(_loadedPrefab)}");
        }
        Debug.Log($"Unloading: {_loadedInstance}, {Addressables.ReleaseInstance(_loadedInstance)}");
    }

    public void Destroy()
    {
        Destroy(_loadedInstance);
    }
}
