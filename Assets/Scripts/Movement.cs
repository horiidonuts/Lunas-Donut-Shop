using System;
using System.Drawing;
using System.Transactions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Animator animator;
    [SerializeField] float moveSpeed = 1f;

    int wLeftHash;
    int wRightHash;
    int wUpHash;
    int wDownHash;

    PlayerInput playerInput;
    InputAction moveAction;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        wUpHash = Animator.StringToHash("wUp");
        wDownHash = Animator.StringToHash("wDown");
        wLeftHash = Animator.StringToHash("wLeft");
        wRightHash = Animator.StringToHash("wRight");    
    }

    // void Update()
    // {
    //     HandleMove();
    // }

    void FixedUpdate()
    {
        HandleMove();
    }

    void HandleMove()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(-direction.x + -direction.y, 0, -direction.y + direction.x) * moveSpeed * Time.deltaTime;
        
        //  full up
        if (new Vector3(-direction.x + -direction.y, 0, -direction.y + direction.x) == new Vector3(-1, 0, -1))
        {
            animator.SetBool(wUpHash, true);
            animator.SetBool(wDownHash,false);
            animator.SetBool(wLeftHash,false);
            animator.SetBool(wRightHash,false);
            
        }

        //  full left
        else if (new Vector3(-direction.x + -direction.y, 0, -direction.y + direction.x) == new Vector3(1, 0, -1))
        {
            animator.SetBool(wLeftHash, true);
            animator.SetBool(wUpHash, false);
            animator.SetBool(wDownHash,false);
            animator.SetBool(wRightHash,false);
        }

        //  upleft
        else if (direction.x < 0 && direction.y > 0)
        {
            animator.SetBool(wUpHash, false);
            animator.SetBool(wDownHash,false);
            animator.SetBool(wLeftHash,true);
            animator.SetBool(wRightHash,false);
        }

        // full right
        else if (new Vector3(-direction.x + -direction.y, 0, -direction.y + direction.x) == new Vector3(-1, 0, 1))
        {
            animator.SetBool(wRightHash, true);
            animator.SetBool(wUpHash, false);
            animator.SetBool(wDownHash,false);
            animator.SetBool(wLeftHash,false);
        }

        //  full down
        else if (new Vector3(-direction.x + -direction.y, 0, -direction.y + direction.x) == new Vector3(1, 0, 1))
        {
            animator.SetBool(wDownHash, true);
            animator.SetBool(wUpHash, false);
            animator.SetBool(wRightHash, false);
            animator.SetBool(wLeftHash,false);
        }

        //  upright
        else if (direction.x > 0 && direction.y > 0)
        {
            animator.SetBool(wUpHash, false);
            animator.SetBool(wDownHash,false);
            animator.SetBool(wLeftHash,false);
            animator.SetBool(wRightHash,true);
        }

        // downright
        else if (direction.x > 0 && direction.y < 0)
        {
            animator.SetBool(wDownHash, true);
            animator.SetBool(wUpHash, false);
            animator.SetBool(wRightHash, false);
            animator.SetBool(wLeftHash,false);
        }

        //  downleft
        else if (direction.x < 0 && direction.y < 0)
        {
            animator.SetBool(wDownHash, true);
            animator.SetBool(wUpHash, false);
            animator.SetBool(wLeftHash, false);
            animator.SetBool(wRightHash,false);
            
        }

        //  standing still
        else 
        {
            animator.SetBool(wUpHash, false);
            animator.SetBool(wDownHash,false);
            animator.SetBool(wLeftHash,false);
            animator.SetBool(wRightHash,false);
        }


    }

    void MoveAnim()
    {
        bool wUp = animator.GetBool(wUpHash);
        bool wDown = animator.GetBool(wDownHash);
        bool wLeft = animator.GetBool(wLeftHash);
        bool wRight = animator.GetBool(wRightHash);
    }
}
