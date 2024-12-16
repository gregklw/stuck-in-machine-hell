using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private LevelSceneGroup _startingLevelSceneGroup;
    private Button _button;
    private LevelManager _manager;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _manager = FindObjectOfType<LevelManager>();
        _button.onClick.AddListener(
            () =>
            {
                _manager.StartGame(_startingLevelSceneGroup);
                _button.enabled = false;
            }
        );
    }

}
