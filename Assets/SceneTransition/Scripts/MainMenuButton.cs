using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    //[SerializeField] private LevelSceneGroup _startingLevelSceneGroup;
    private Button _button;
    private LevelManager _manager;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _manager = FindObjectOfType<LevelManager>();
        _button.onClick.AddListener(
            () =>
            {
                _manager.StartGame();
                _button.enabled = false;
            }
        );
    }

    public void UnloadUnusedAssets()
    {
        StartCoroutine(UnloadUnusedAssetsCoroutine());
    }

    private IEnumerator UnloadUnusedAssetsCoroutine()
    {
        AsyncOperation unloadOperation = Resources.UnloadUnusedAssets();
        while (!unloadOperation.isDone)
        {
            Debug.Log($"Progress: {(Mathf.Round(unloadOperation.progress * 100)) / 100.0}");
            yield return null;
        }
    }
}
