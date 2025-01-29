using System;
using Unity.VisualScripting;
using UnityEngine;

public class TakeOrder : MonoBehaviour
{
    [SerializeField] bool canTakeOrder = false;
    [SerializeField] public bool playerHasOrder = false;

    private GameObject customer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canTakeOrder && Input.GetKeyDown(KeyCode.E) && !playerHasOrder)
        {
            playerHasOrder = true;
            DisableSphere();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "customer")
        {
            customer = other.gameObject;
            canTakeOrder = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "customer")
        {
            canTakeOrder = false;
        }
    }

    void DisableSphere()
    {
        var customerMovement = customer.GetComponent<CustomerMovement>();
        customerMovement.SetDeactive_orderSphere();
    }
}
