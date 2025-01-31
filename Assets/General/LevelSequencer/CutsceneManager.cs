using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _cutsceneText;
    private Button _cutsceneButton;
    private TMP_Text _cutsceneButtonText;
    private Image _cutscenePanelObj;

    private void Awake()
    {
        _cutscenePanelObj = GetComponent<Image>();
        _cutsceneButton = GetComponentInChildren<Button>(true);
        _cutsceneButtonText = _cutsceneButton.GetComponentInChildren<TMP_Text>(true);
    }

    public void ToggleButtonVisibility(bool isActive)
    {
        if (isActive == false) _cutsceneButtonText.text = "";
        _cutsceneButton.gameObject.SetActive(isActive);
    }

    public void TogglePanelVisibility(bool isActive)
    {
        if (isActive == false) _cutsceneText.text = "";
        _cutscenePanelObj.gameObject.SetActive(isActive);
    }

    public void AddButtonAction(UnityEngine.Events.UnityAction action)
    {
        _cutsceneButton.onClick.AddListener(action);
    }

    public void ClearButtonActions()
    {
        Debug.Log("Actions for button cleared!");
        _cutsceneButton.onClick.RemoveAllListeners();
    }

    public void ChangeHeaderText(string text)
    {
        _cutsceneText.text = text;
    }

    public void ChangeButtonText(string text)
    {
        _cutsceneButtonText.text = text;
    }

    //public IEnumerator StartIntroCoroutine()
    //{
    //    TogglePanelVisibility(true);
    //    WaitForSeconds delay = new WaitForSeconds(1);
    //    int numberOfSeconds = 5;

    //    while (numberOfSeconds > 0)
    //    {
    //        _cutsceneText.text = $"Wave starting in {numberOfSeconds}";
    //        numberOfSeconds--;
    //        yield return delay;
    //    }
    //    TogglePanelVisibility(false);
    //}

    public IEnumerator StartOutroCoroutine()
    {
        TogglePanelVisibility(true);
        _cutsceneText.text = $"Level finished!";
        WaitForSeconds delay = new WaitForSeconds(2);
        yield return delay;
        _cutsceneButtonText.text = "Proceed to next level";
        ToggleButtonVisibility(true);
    }
}
