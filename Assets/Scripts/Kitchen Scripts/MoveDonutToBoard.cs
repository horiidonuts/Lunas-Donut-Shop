using System;
using UnityEngine;
using DG.Tweening;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.InputSystem;

public class MoveDonutToBoard : MonoBehaviour
{
    [SerializeField] private Vector3 endPos;
    [SerializeField] private float jumpPower;
    [SerializeField] private int jumpCount;
    [SerializeField] private float duration;
    [SerializeField] private bool snap;
    [SerializeField] private Ease easing;
    
    private CookBubblePop _cookBubblePop;
    private GameObject _donut;
    public static MoveDonutToBoard Instance;
    private bool _isOnBoard;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }
    
    // Add function to make bubble notch always follow the donut.

    private void Start()
    {
        _cookBubblePop = GameObject.Find("Panel").GetComponent<CookBubblePop>();
        _donut = GameObject.FindWithTag("Donut");
    }
    
    public void MoveToBoard()
    {
        _donut.transform.DOJump(endPos, jumpPower, jumpCount, duration, snap).SetEase(easing);
        _donut.transform.DORotate(new Vector3(0f, 0f, -180f), 
            duration, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuint);
        _cookBubblePop.CloseBubble();
        _isOnBoard = true;
    }

    public bool IsOnBoard()
    {
        return _isOnBoard;
    }

    public void SetIsOnBoard(bool isOnBoard)
    {
        _isOnBoard = isOnBoard;
    }
}
