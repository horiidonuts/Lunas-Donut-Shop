using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Vector3 rawInput;

    void Update()
    {
        
    }

    void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector3>();
        Debug.Log(rawInput);
    }
}
