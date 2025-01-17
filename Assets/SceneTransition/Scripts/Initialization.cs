using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialization : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(720,1600, false);
    }
    void Start()
    {
#if UNITY_ANDROID
        Application.targetFrameRate = -1;
#endif
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        levelManager.GoToMainMenu();
    }
}
