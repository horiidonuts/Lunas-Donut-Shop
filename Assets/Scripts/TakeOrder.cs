using System;
using UnityEngine;

public class TakeOrder : MonoBehaviour
{
    public static TakeOrder Instance;
    [SerializeField] bool canTakeOrder = false;
    [SerializeField] public bool playerHasOrder = false;
    public string CustomerWant_Sauce; //bunu return edecek bir metod yazılabilir.
    

    public GameObject customer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        CustomerWant_Sauce = null;
    }


    void Update()
    {
        if (canTakeOrder && Input.GetKeyDown(KeyCode.E) && !playerHasOrder)
        {
            playerHasOrder = true;
            DisableSphere(); 

            var donutOrder=customer.GetComponent<DonutOrder>();
            if (donutOrder != null)
            {
                DonutOrder.SauceType DonutSauce = donutOrder.sauce;
                Debug.Log("Donut Order: " + DonutSauce);
                CustomerWant_Sauce = DonutSauce.ToString();
            }
      }
 }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "customer")
        {
            customer = other.gameObject; 
            canTakeOrder = true;
            Transform serviceTransform=other.transform.Find("zone");
            Debug.Log("customera teslim edildi");
            if (serviceTransform != null)
                {
                    OrderPrepare.Instance.SetDonutParent(serviceTransform);
                    Debug.Log("ebeveyn");
                }
                else
                {
                    Debug.Log("transform null");
                }
                
            //otherın altındaki servicezone emptyobjectin transformunu al
        }
        
                // child içinde service tagli objenin transformuna orderprepare içindeki
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

    public void ResetOrderState()
    {
        playerHasOrder = false;
    }


    public bool HasPlayerOrder()
    {
        return playerHasOrder;
    }


   



}
