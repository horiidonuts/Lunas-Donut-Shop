using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class FlipDonut : MonoBehaviour
{
    private Camera _mainCamera;
    private PlayerInput _playerInput;
    private InputAction _clickAction;
    //private Animator _animator;
    private bool _flipped;
    private static readonly int Flip = Animator.StringToHash("Flip");

    private GameObject _donut;
    private Vector3 _firstPosition;
    private Vector3 _lastPosition;
    
    private Quaternion _firstRotation;
    [SerializeField] private Vector3 lastRotation;

    [SerializeField] private Ease moveEasing;
    [SerializeField] private Ease rotateEasing;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float rotateDuration;
    
    [SerializeField] private float jumpPower;
    
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        //_animator = GetComponent<Animator>();
        _clickAction = _playerInput.actions["Click"];
        _mainCamera = Camera.main;
    }
    
    private void Start()
    {
        _flipped = false;
        _donut = GameObject.Find("lunadonut2");
        _firstPosition = _donut.transform.position;
        _lastPosition = _firstPosition;
        _lastPosition.y = _firstPosition.y + jumpHeight;
        _firstRotation = _donut.transform.rotation;
        lastRotation.z = _firstRotation.z - 180;
        Debug.Log("First Pos: " + _firstPosition + " Last Pos: " + _lastPosition);

    }

    private void OnEnable()
    {
        _clickAction.performed += OnClick;
    }

    private void OnDisable()
    {
        _clickAction.performed -= OnClick;
    }

    private void OnDestroy()
    {
        _playerInput.actions["Click"].performed -= OnClick;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition); // Ekrandan dunyaya raycast

        if (Physics.Raycast(ray, out hit) // Eger raycast bit objeye carparsa,  
            && hit.transform.CompareTag("Donut") // bu donutsa ve
            && !IsPointerOverUIObject()) // uzerinde bir ui objesi yoksa
        {
            if (!_flipped) // Donut daha once cevirilmemisse
            {
                Debug.Log("Object hit: " + hit.transform.name); // Raycastin carptigi objeyi yazdir
                FlipTrigger();
                CookDonut.Instance.ChangeSides();
                _flipped = true;
            }
            else
            {
                MoveDonutToBoard.Instance.MoveToBoard();
                MoveDonutToBoard.Instance.SetIsOnBoard(true);
            }
        }
    }

    private void FlipTrigger()
    {
        _donut.transform.DOJump(_firstPosition, jumpPower, 1, jumpDuration).SetEase(moveEasing);
        
        _donut.transform.DORotate(lastRotation, rotateDuration, RotateMode.LocalAxisAdd).SetEase(rotateEasing);
    }

    private bool IsPointerOverUIObject()
    {
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}