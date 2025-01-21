using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private LevelSceneGroup _startingLevelSceneGroup;
    private Button _button;
    private MainMenuManager _manager;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _manager = FindObjectOfType<MainMenuManager>();
        _button.onClick.AddListener(
            () =>
            {
                _manager.StartGame(_startingLevelSceneGroup);
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
        AsyncOperation unloadOp = Resources.UnloadUnusedAssets();

        while (!unloadOp.isDone)
        {
            Debug.Log((Mathf.Round(unloadOp.progress * 100)) / 100.0);
            yield return null;
        }
    }

}
