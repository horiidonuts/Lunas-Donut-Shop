using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerSpawn : MonoBehaviour
{
    public static CustomerSpawn Instance;
    public GameObject[] customerPrefabs;
    public Transform spawnPoint;
    public int poolSize=1;
    public List<GameObject> customerPool;
    private int currentCustomerIndex=0;
    private bool isSpawning=false;

    void Awake()
    {
        if(Instance==null)
        {
            Instance=this;
        }
        else
        {
            Destroy(gameObject);
        }

        customerPool=new List<GameObject>();

                for (int i = 0; i < poolSize; i++)
        {
            foreach (var prefab in customerPrefabs)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                customerPool.Add(obj); // Müşteriyi havuza ekle
            }
        }
    }


    void Update()
    {
         if (Input.GetKeyDown(KeyCode.X) && !isSpawning)
        {
            SpawnCustomer();
        }
    }



    public void SpawnCustomer()
    {
        bool allActive=true;
        foreach (GameObject customer in customerPool)
        {
            if(!customer.activeInHierarchy)
            {
                allActive=false;
                break;
            }

        }

        if(!allActive)
        {
            GameObject customer=customerPool[currentCustomerIndex];
            customer.transform.position=spawnPoint.position;
            customer.SetActive(true);
            currentCustomerIndex=(currentCustomerIndex+1)%customerPool.Count;
            
        }


         


    }









}
