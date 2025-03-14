using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private Button _button;
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Translate);
    }

    private void Translate()
    {
        StartCoroutine(TranslateIn());
    }

    private IEnumerator TranslateIn()
    {
        float duration = TransitionEffect.Instance.GetDuration();
        float waitTime = TransitionEffect.Instance.GetWaitTime();
        TransitionEffect.Instance.TransitionIn();
        yield return new WaitForSeconds(duration + waitTime);
        SceneManager.Instance.LoadScene(1);
    }
}
