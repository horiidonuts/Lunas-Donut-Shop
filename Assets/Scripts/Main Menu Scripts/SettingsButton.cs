using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;

public class SettingsButton : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button[] buttons; 
    [SerializeField] private Button backButton;
    [Header("Menu Button Values")]
    [SerializeField] private float moveX;
    [SerializeField] private Ease easingIn;
    [SerializeField] private Ease easingOut;
    [SerializeField] private float moveDelay;
    [SerializeField] private bool moveBack = false;
    [SerializeField] private float moveDuration;
    [Header("Back Button Values")]
    [SerializeField] private float backButtonMoveX;
    
    void Start()
    {
        backButton.transform.localPosition = new Vector3(backButton.transform.localPosition.x - backButtonMoveX,
            backButton.transform.localPosition.y,
            backButton.transform.localPosition.z);
    }

    void Update()
    {
        
    }

    public void MoveMenuButtons()
    {
        StartCoroutine(MoveButtons(moveBack));
    }

    private IEnumerator MoveButtons(bool goingToClose)
    {
        if (!goingToClose)
        {
            backButton.transform.DOLocalMove(new Vector3(
                    backButton.transform.localPosition.x + backButtonMoveX,
                    backButton.transform.localPosition.y,
                    backButton.transform.localPosition.z),
                moveDuration).SetEase(easingOut);
            
            foreach (Button button in buttons)
            {
                button.transform.DOMove(
                    new Vector3(button.transform.position.x - moveX,
                        button.transform.position.y,
                        button.transform.position.z),
                    moveDuration).SetEase(easingIn);
                yield return new WaitForSeconds(moveDelay);
                
            }
            
            Debug.Log("Settings Open");
            //yield break;
        }

        else
        {
            backButton.transform.DOLocalMove(new Vector3(
                    backButton.transform.localPosition.x - backButtonMoveX,
                    backButton.transform.localPosition.y,
                    backButton.transform.localPosition.z),
                moveDuration).SetEase(easingIn);
            foreach (Button button in buttons)
            {
                button.transform.DOMove(
                    new Vector3(button.transform.position.x + moveX, button.transform.position.y,
                        backButton.transform.localPosition.z),
                    moveDuration).SetEase(easingIn);
                yield return new WaitForSeconds(moveDelay);
            
            }
            Debug.Log("Settings Close");
        }
        
        
    }
}
