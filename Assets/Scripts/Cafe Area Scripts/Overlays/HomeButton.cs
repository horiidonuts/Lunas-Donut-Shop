using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HomeButton : MonoBehaviour
{
    [SerializeField] private Button _homeButton;
    void Start()
    {
        _homeButton = GetComponent<Button>();
        _homeButton.onClick.AddListener(OnHomeButtonClick);
    }

    private void OnHomeButtonClick()
    {
        StartCoroutine(ChangeScene(0)); // Ana menüye dön
    }

    private IEnumerator ChangeScene(int index)
    {
        // Ana menüye geçiş yap
        float duration = TransitionEffect.Instance.GetDuration();
        float waitTime = TransitionEffect.Instance.GetWaitTime();
        TransitionEffect.Instance.TransitionIn(0);
        yield return new WaitForSeconds(duration + waitTime);
    }
}
