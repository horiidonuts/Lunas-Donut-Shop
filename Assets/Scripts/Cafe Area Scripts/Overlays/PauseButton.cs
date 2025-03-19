using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    private Button _button;
    
    void Start()
    {
        TransitionOut.Instance.ResetTransition();
        TransitionOut.Instance.TranslateOut();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(TranslateBack);
    }

    private void TranslateBack()
    {
        StartCoroutine(ChangeScene(0));
    }

    private IEnumerator ChangeScene(int index)
    {
        float duration = TransitionEffect.Instance.GetDuration();
        float waitTime = TransitionEffect.Instance.GetWaitTime();
        TransitionEffect.Instance.TransitionIn();
        yield return new WaitForSeconds(duration + waitTime);
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }
}