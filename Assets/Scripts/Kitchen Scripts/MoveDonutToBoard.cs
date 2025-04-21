using System;
using UnityEngine;
using DG.Tweening;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class MoveDonutToBoard : MonoBehaviour
{
    [SerializeField] private Vector3 endPos;
    [SerializeField] private float jumpPower;
    [SerializeField] private int jumpCount;
    [SerializeField] private float duration;
    [SerializeField] private bool snap;
    [SerializeField] private Ease moveEasing;
    [SerializeField] private Ease rotateEasing;
    
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

    private void Start()
    {
        _cookBubblePop = GameObject.Find("Panel").GetComponent<CookBubblePop>();
        _donut = GameObject.FindWithTag("Donut");
    }
    
    public void MoveToBoard() // Donutu tahtaya götür
    {
        if (_isOnBoard) return; // Eğer donut tahtada ise çık
        CookDonut.Instance.StopCooking();
        _donut.transform.DOJump(endPos, jumpPower,
            jumpCount, duration, snap).SetEase(moveEasing); // Donutu zıplama hareketi ile hareket ettir
        _donut.transform.DORotate(new Vector3(0f, 0f, -180f), // Hareket ettirirken aynı zamanda da döndür
            duration, RotateMode.LocalAxisAdd).SetEase(rotateEasing);
        _cookBubblePop.CloseBubble(); // CookBubble'ı kapat
        _isOnBoard = true; // Artık donut tahtanın üstünde
    }

    public bool IsOnBoard()
    {
        return _isOnBoard; // Tahtanın üstünde mi getter'ı 
    }

    public void SetIsOnBoard(bool isOnBoard)
    {
        _isOnBoard = isOnBoard; // Dışarıdan ayarlamak için _isOnBoard setter'ı
    }
}
