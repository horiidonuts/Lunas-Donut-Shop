using System;
using System.Collections;
using Mono.Cecil.Cil;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class KitchenUiAnimHandle : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    private float _waitDuration;
    [SerializeField] private int index; 
    [SerializeField] private int currentPhase;
    
    private CameraController _camController;
    private CookBubblePop _cookBubblePop;
    private static readonly int Clicked = Animator.StringToHash("Clicked");
    
    private void Start() 
    {
        currentPhase = 1;
        _camController = GetComponent<CameraController>();
        _waitDuration = _camController.GetMoveDuration();
        _cookBubblePop = GetComponent<CookBubblePop>();
    }
    
    public void OnButtonClick(Button button)
    {
        Animator animator = button.GetComponent<Animator>();
        if (animator != null)
        {
            _waitDuration = _camController.GetMoveDuration();
            //Debug.Log("Button hit: " + button);
            animator.SetTrigger(Clicked);
            DisableEnableButtons();
            
        }
    }

    private void Update() 
    {
        if (_camController.GetCurrentPos() == new Vector3(-0.8f, 1.95f, 1.27f))
        {
            DisableSingularButton(1);
        }

        if (currentPhase == 2 && !_camController.GetZoomStatus())
        {
            _camController.ZoomIn();
        }

        if (currentPhase != 2 && _camController.GetZoomStatus())
        {
            _camController.ZoomOut();
        }
    }

    private void DisableSingularButton(int index)
    {
        buttons[index].interactable = false;
    }

    public void DisableEnableButtons()
    {
        foreach (var button in buttons)
        {
            StartCoroutine(DEbuttons(button));
        }
    }

    IEnumerator DEbuttons(Button button)
    {
        button.interactable = false;
        yield return new WaitForSeconds(_waitDuration);
        button.interactable = true;
    }

    public void IncreasePhase()
    {
        currentPhase++;
    }

    public void DecreasePhase()
    {
        currentPhase--;
    }

    public int GetPhase()
    {
        return currentPhase;
    }
}
