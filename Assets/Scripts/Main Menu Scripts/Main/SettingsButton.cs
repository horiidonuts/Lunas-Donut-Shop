using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine.Serialization;

public class SettingsButton : MonoBehaviour
{
    [Header("Buttons")] [SerializeField] private Button[] buttons;
    [SerializeField] private Button backButton;

    [Header("Menu Button Values")] [SerializeField]
    private float moveX;

    [SerializeField] private Ease easingIn;
    [SerializeField] private Ease easingOut;
    [SerializeField] private float moveDelay;
    [SerializeField] private bool moveBack = false;
    [SerializeField] private float moveDuration;

    [Header("Back Button Values")] [SerializeField]
    private float backButtonMoveX;

    [Header("AnimHandler GameObject")] [SerializeField]
    private GameObject animHandler;

    private MoveSettingItems _settingItems;

    [Header("Settings Menu")] 
    [SerializeField] private GameObject settingsMenu;

    void Start()
    {
        _settingItems = animHandler.GetComponent<MoveSettingItems>();
    }

    public void MoveMenuButtons()
    {
        StartCoroutine(MoveButtons(moveBack));
    }

    private IEnumerator MoveButtons(bool goingToClose)
    {
        if (!goingToClose) // Ayarlar bolmesi acildi
        {
            settingsMenu.SetActive(true); // Hiyerarside setactive'i aktif yap
            
            foreach (Button button in buttons) // Tum butonlari sola kaydir 
            {
                button.transform.DOMove(
                    new Vector3(button.transform.position.x - moveX,
                        button.transform.position.y,
                        button.transform.position.z),
                    moveDuration).SetEase(easingIn);
                yield return new WaitForSeconds(moveDelay);
            }

            _settingItems.MoveItemsRight();
            Debug.Log("Settings Open");
        }

        else // Ayarlar bolmesi kapandi
        {
            _settingItems.MoveItemsLeft();

            foreach (Button button in buttons) // Tum butonlari sirayla eski yerine dondur
            {
                button.transform.DOMove(
                    new Vector3(button.transform.position.x + moveX, button.transform.position.y,
                        backButton.transform.localPosition.z),
                    moveDuration).SetEase(easingOut);
                yield return new WaitForSeconds(moveDelay);
            }

            settingsMenu.SetActive(false); // Setactive deaktive et
            Debug.Log("Settings Close");
        }
    }
}