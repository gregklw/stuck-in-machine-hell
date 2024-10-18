using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialization : MonoBehaviour
{
    void Start()
    {
#if UNITY_ANDROID
        Application.targetFrameRate = -1;
#endif
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        levelManager.GoToMainMenu();
    }
}
