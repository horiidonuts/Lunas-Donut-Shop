using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class CookBubblePop : MonoBehaviour
{
    RectTransform _rect;
    [SerializeField] private float zoomDuration;
    private bool _isPanelOpen = false;
    [SerializeField] private GameObject kuiObject;
    private KitchenUiAnimHandle _kuiAnimHandle;
    private Vector3 _firstScale; // Baloncugun ilk boyutunu tutan vektor
    private Vector3 _firstPosition; // Baloncugun ilk konumunu tutan vektor


    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _rect.transform.DOScale(new Vector3(0, 0, 0),
            0f); // Basta balonun boyutunu 0 yaptik bu sekilde isliyor animasyon.
        _kuiAnimHandle = GameObject.Find("AnimHandler").GetComponent<KitchenUiAnimHandle>();
    }

    void Start()
    {
        _firstScale = _rect.transform.localScale;
        OpenBubble();
    }

    void Update()
    {
        if (!_isPanelOpen && _kuiAnimHandle.GetPhase() == 1 &&
            !MoveDonutToBoard.Instance.IsOnBoard()) // Panel kapali ve faz 1 ise baloncugu ac
        {
            OpenBubble();
        }

        if (_isPanelOpen && _kuiAnimHandle.GetPhase() != 1 /*&& MoveDonutToBoard.Instance.IsOnBoard()*/
           ) // Panel acik ve faz 1 degil ise baloncugu kapat
        {
            CloseBubble();
        }
    }

    private void OpenBubble() // Baloncugu acan method
    {
        _rect.transform.DOScale(_firstScale, zoomDuration).SetEase(Ease.OutBack);
        _isPanelOpen = true;
    }

    public void CloseBubble() // Baloncugu kapatan method
    {
        _rect.transform.DOScale(new Vector3(0, 0, 0), zoomDuration).SetEase(Ease.OutExpo);
        _isPanelOpen = false;
    }
}