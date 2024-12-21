using System;
using System.Transactions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Vector3 rawInput;

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 delta = rawInput * moveSpeed * Time.deltaTime;
        transform.position += delta;
    }

    void OnMove(InputValue value)
    {
        Vector3 input = value.Get<Vector3>();

        float x_input = input.x;
        float z_input = input.z;

        if (input.z > Mathf.Epsilon || input.z < -Mathf.Epsilon)
        {
            rawInput = new Vector3(z_input * -0.5f, input.y, z_input * -0.5f);
        }
        else if(input.x > Mathf.Epsilon || input.x < -Mathf.Epsilon)
        {
            rawInput = new Vector3(x_input * -0.5f, input.y, x_input * 0.5f);
        }

        else 
        {
            rawInput = new Vector3(0, 0, 0);
        }

        

        Debug.Log(rawInput);
    }

}
