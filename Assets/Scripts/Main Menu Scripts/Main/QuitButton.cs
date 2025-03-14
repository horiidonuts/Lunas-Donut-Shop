using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class QuitButton : MonoBehaviour
{
    private Button _button;
    private GameObject _transitionEffect;
    //private TransitionEffect _transition;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        StartCoroutine(QuitProcess());
    }

    private IEnumerator QuitProcess()
    {
        Debug.LogWarning("Quitting Game");
        TransitionEffect.Instance.TransitionIn();
        yield return new WaitForSeconds(TransitionEffect.Instance.GetDuration());
        Application.Quit();
    }
}
