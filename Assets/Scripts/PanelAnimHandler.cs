using System;
using Unity.VisualScripting;
using UnityEngine;

public class PanelAnimHandler : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] private bool canProcessOrder;
    [SerializeField] private bool _playerHasOrder;
    [SerializeField] Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (canProcessOrder && Input.GetKeyDown(KeyCode.E) && _playerHasOrder)
        {
            OpenPanel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            canProcessOrder = true;
            GetPlayerOrderState();
        }
    }

    void OnTriggerExit(Collider other)
    {
        canProcessOrder = false;
    }

    void OpenPanel()
    {
        anim.SetBool("panel_open", true);
    }

    public void ClosePanel()
    {
        anim.SetBool("panel_open", false);
    }

    private void GetPlayerOrderState()
    {
        var playerOrderState = player.GetComponentInParent<TakeOrder>();
        _playerHasOrder = playerOrderState.playerHasOrder;
    }
}
