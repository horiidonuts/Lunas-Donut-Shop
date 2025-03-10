using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveAmountX;
    [SerializeField] private float moveDuration;
    [SerializeField] private float _zoomTime;
    
    [FormerlySerializedAs("_mainCamera")] [SerializeField] Camera mainCamera;
    private KitchenUiAnimHandle _kuiHandle;
    private int _currentPhase;
    private float _currentFov;
    private bool _zoomedIn = false;
    
    void Start()
    {
        mainCamera = Camera.main;
        _kuiHandle = GetComponent<KitchenUiAnimHandle>();
        _currentFov = mainCamera.fieldOfView;
    }
    
    // void Update()
    // {
        
    // }

    public void MoveCameraToRight()
    {
        Vector3 targetPos = new Vector3(mainCamera.transform.position.x + moveAmountX, mainCamera.transform.position.y, mainCamera.transform.position.z);
        mainCamera.transform.DOMove(targetPos, moveDuration,false).SetEase(Ease.OutQuart).OnComplete(
         ()=>{
            targetPos=new Vector3(mainCamera.transform.position.x+moveAmountX,
            mainCamera.transform.position.y,
            mainCamera.transform.position.z);
            }
         );
        _kuiHandle.IncreasePhase();
        _currentPhase = _kuiHandle.GetPhase();
    }

    public void MoveCameraToLeft()
    {
        Vector3 targetPos = new Vector3(mainCamera.transform.position.x - moveAmountX, mainCamera.transform.position.y, mainCamera.transform.position.z);
        mainCamera.transform.DOMove(targetPos, moveDuration,false).SetEase(Ease.OutQuart).OnComplete(
            ()=>{
                targetPos=new Vector3(mainCamera.transform.position.x-moveAmountX,
                mainCamera.transform.position.y,
                mainCamera.transform.position.z);
            }
        ); 
        _kuiHandle.DecreasePhase();
        _currentPhase = _kuiHandle.GetPhase();
    }

    public Vector3 GetCurrentPos()
    {
        return mainCamera.transform.position;
    }

    public void ZoomIn()
    {
        DOTween.To(() => mainCamera.fieldOfView, x => mainCamera.fieldOfView = x, 30, _zoomTime).SetEase(Ease.OutQuart);
        _zoomedIn = true;
    }

    public void ZoomOut()
    {
        DOTween.To(() => mainCamera.fieldOfView, x => mainCamera.fieldOfView = x, 50, _zoomTime).SetEase(Ease.OutQuart);
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
