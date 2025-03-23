using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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
        _playerInput.actions["Click"].performed += OnClick;
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
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit)
            && hit.transform.CompareTag("Donut")
            && !IsPointerOverUIObject()&& !_flipped)
        {
            Debug.Log("Object hit: " + hit.transform.name);
            StartCoroutine(FlipTrigger());
            CookDonut.Instance.ChangeSides();
            _flipped = true;
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
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}