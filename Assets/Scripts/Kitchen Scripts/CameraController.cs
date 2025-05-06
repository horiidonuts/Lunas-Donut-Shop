using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveAmountX;
    [SerializeField] private float moveDuration;
    [SerializeField] private float zoomTime;

    [SerializeField] Camera mainCamera;

    [SerializeField] private KitchenUiAnimHandle kuiHandle;
    private int _currentPhase;
    private float _currentFov;
    private bool _zoomedIn;

    void Start()
    {
        TransitionOut.Instance.TranslateOut();
        //mainCamera = Camera.main;
        kuiHandle = kuiHandle.GetComponent<KitchenUiAnimHandle>();
        _currentFov = mainCamera.fieldOfView;
    }

    public void MoveCameraToRight()
    {
        Vector3 targetPos = new Vector3(mainCamera.transform.position.x + moveAmountX, mainCamera.transform.position.y,
            mainCamera.transform.position.z);
        mainCamera.transform.DOMove(targetPos, moveDuration, false).SetEase(Ease.OutQuart).OnComplete(
            () =>
            {
                targetPos = new Vector3(mainCamera.transform.position.x + moveAmountX,
                    mainCamera.transform.position.y,
                    mainCamera.transform.position.z);
            }
        );
        
        if (kuiHandle == null)
        {
            Debug.LogError("KitchenUiAnimHandle is not assigned!");
            return;
        }
        
        kuiHandle.IncreasePhase();
        _currentPhase = kuiHandle.GetPhase();
        Debug.Log("Phase increased. Current phase: " + _currentPhase);
    }

    public void MoveCameraToLeft()
    {
        Vector3 targetPos = new Vector3(mainCamera.transform.position.x - moveAmountX, mainCamera.transform.position.y,
            mainCamera.transform.position.z);
        mainCamera.transform.DOMove(targetPos, moveDuration, false).SetEase(Ease.OutQuart).OnComplete(
            () =>
            {
                targetPos = new Vector3(mainCamera.transform.position.x - moveAmountX,
                    mainCamera.transform.position.y,
                    mainCamera.transform.position.z);
            }
        );
        kuiHandle.DecreasePhase();
        _currentPhase = kuiHandle.GetPhase();
    }

    public Vector3 GetCurrentPos()
    {
        return mainCamera.transform.position;
    }

    public void ZoomIn()
    {
        DOTween.To(() => mainCamera.fieldOfView, x => mainCamera.fieldOfView = x, 30, zoomTime).SetEase(Ease.OutQuart);
        _zoomedIn = true;
    }

    public void ZoomOut()
    {
        DOTween.To(() => mainCamera.fieldOfView, x => mainCamera.fieldOfView = x, 50, zoomTime).SetEase(Ease.OutQuart);
        _zoomedIn = false;
    }

    public float GetMoveDuration()
    {
        return moveDuration;
    }

    public bool GetZoomStatus()
    {
        return _zoomedIn;
    }
}