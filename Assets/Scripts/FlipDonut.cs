using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlipDonut : MonoBehaviour
{
    Camera _mainCamera;
    private PlayerInput _playerInput;
    private InputAction _clickAction;
    Animator _animator;
    
    

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _playerInput.actions["Click"].performed += OnClick;
        _clickAction = _playerInput.actions["Click"];
        _mainCamera = Camera.main;
    }

    void Start()
    {
        
    }
    
    
    void Update()
    {
        
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

    public void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Donut"))
        {
            Debug.Log("Object hit: " + hit.transform.name);
            StartCoroutine(FlipTrigger());
        }
    }
    
    private IEnumerator FlipTrigger()
    {
        _animator.SetTrigger("Flip");
        yield return new WaitForSeconds(1f);
        _animator.ResetTrigger("Flip");
    }
}
