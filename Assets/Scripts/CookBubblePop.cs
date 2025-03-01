using DG.Tweening;
using UnityEngine;

public class CookBubblePop : MonoBehaviour
{
    RectTransform _rect;
    [SerializeField] private float _zoomDuration;
    private bool _isPanelOpen = false;
    [SerializeField] private GameObject kuiObject;
    private KitchenUiAnimHandle _kuiAnimHandle;
    

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _rect.transform.DOScale(new Vector3(0,0,0), 0f);
        OpenBubble();
        _kuiAnimHandle = kuiObject.GetComponent<KitchenUiAnimHandle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPanelOpen && _kuiAnimHandle.GetPhase() == 1)
        {
            OpenBubble();
        }

        if (_isPanelOpen && _kuiAnimHandle.GetPhase() != 1)
        {
            CloseBubble();
        }
    }

    private void OpenBubble()
    {
        _rect.transform.DOScaleX(0.35f, _zoomDuration).SetEase(Ease.OutBack);
        _rect.transform.DOScaleY(0.35f, _zoomDuration).SetEase(Ease.OutBack);
        _rect.transform.DOMove(new Vector3(1350, 830, 0), _zoomDuration).SetEase(Ease.OutBack);
        _isPanelOpen = true;
    }
    

    public void CloseBubble()
    {
        _rect.transform.DOScale(new Vector3(0,0,0), _zoomDuration).SetEase(Ease.OutExpo);
        _rect.transform.DOMove(new Vector3(0, 830, 0), _zoomDuration).SetEase(Ease.OutExpo);
        _isPanelOpen = false;
    }
}
