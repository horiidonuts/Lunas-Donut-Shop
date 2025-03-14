using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MoveSettingItems : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Ease easeIn;
    [SerializeField] private Ease easeOut;
    [SerializeField] private float moveAmount;
    [SerializeField] private float moveDuration;
    
    void Start()
    {
        // Basta ayarlar bolmesindeki her itemi gizler 
        panel.transform.localPosition =
            new Vector3(panel.transform.localPosition.x - moveAmount, panel.transform.localPosition.y,
                panel.transform.localPosition.z);
    }

    public void MoveItemsRight()
    {
        panel.transform.DOLocalMove(new Vector3(panel.transform.localPosition.x + moveAmount,
                    panel.transform.localPosition.y,
                    panel.transform.localPosition.z),
                moveDuration)
            .SetEase(easeOut); // Ayarlar bolmesindeki itemleri saga kaydir (paneli ac)
    }

    public void MoveItemsLeft()
    {
        panel.transform.DOLocalMove(new Vector3(panel.transform.localPosition.x - moveAmount,
                    panel.transform.localPosition.y,
                    panel.transform.localPosition.z),
                moveDuration)
            .SetEase(easeIn);
    }
}