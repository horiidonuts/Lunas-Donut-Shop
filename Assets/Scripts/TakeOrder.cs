using System;
using UnityEngine;

public class TakeOrder : MonoBehaviour
{
    public static TakeOrder Instance;
    [SerializeField] bool canTakeOrder = false;
    [SerializeField] public bool playerHasOrder = false;
    public string CustomerWant_Sauce; //bunu return edecek bir metod yazılabilir.
    
    public bool DonutOrderProcess = false; //donut garsonun elindeyken true olacak 
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
            if (DonutOrderProcess)
            {
                Transform serviceTransform=customer.transform.Find("Service_Zone");
                Debug.Log("customera teslim edildi");
                OrderPrepare.Instance.SetDonutParent(serviceTransform);
                //customerın içindeki customermovementın içinde sayacı başlat ve kasaya gitsin
                var customerMovement = customer.GetComponent<CustomerMovement>();
                //customerMovement.DestroyDonut();
                StartCoroutine(customerMovement.WaitAndMoveToPayZone()); 
                DonutOrderProcess = false;
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

    public void SetDonutOrderProcess( )
    {
        DonutOrderProcess = true;
    }


   



}
