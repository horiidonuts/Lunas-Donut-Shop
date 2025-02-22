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
        mainCamera.transform.DOMove(targetPos, moveDuration,false).OnComplete(
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
        mainCamera.transform.DOMove(targetPos, moveDuration,false).OnComplete(
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

    public void DecreaseFOV()
    {
        DOTween.To(() => _currentFov, x => _currentFov = x, 30, _zoomTime);
    }
}
