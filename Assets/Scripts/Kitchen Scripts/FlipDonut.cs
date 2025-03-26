using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FlipDonut : MonoBehaviour
{
    private Camera _mainCamera;
    private PlayerInput _playerInput;
    private InputAction _clickAction;
    private Animator _animator;
    private bool _flipped;
    private static readonly int Flip = Animator.StringToHash("Flip");

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _clickAction = _playerInput.actions["Click"];
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _flipped = false;
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
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)
            && hit.transform.CompareTag("Donut")
            && !IsPointerOverUIObject())
        {
            if (!_flipped)
            {
                Debug.Log("Object hit: " + hit.transform.name);
                StartCoroutine(FlipTrigger());
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

    private IEnumerator FlipTrigger()
    {
        _animator.SetTrigger(Flip);
        yield return new WaitForSeconds(1f);
        _animator.ResetTrigger(Flip);
        yield return new WaitForSeconds(1f);
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