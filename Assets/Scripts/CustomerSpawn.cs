using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerSpawn : MonoBehaviour
{

    public static CustomerSpawn Instance; //singleton
    public GameObject[] customerPrefabs; //customer prefabs
    public Transform spawnPoint; //spawn point
    public int poolSize = 1; //pool size
     public List<GameObject> customerPool; //customer pool

        private int currentCustomerIndex = 0;  // Müşteri havuzundan hangi müşteriyi alacağımızı belirleyen index
        //private float spawnInterval = 2.0f; // Müşteri oluşturma aralığı
       // private float nextSpawnTime = 0f; // Bir sonraki müşteri oluşturma zamanı
         private bool isSpawning = false; // Müşteri oluşturuluyor mu kontrolü

     void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    
        customerPool = new List<GameObject>(); // Müşteri havuzunu oluştur

        // Initialize the pool
        for (int i = 0; i < poolSize; i++)
        {
            foreach (var prefab in customerPrefabs)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                customerPool.Add(obj);
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


     /*private IEnumerator SpawnCustomerRoutine()
    {
        isSpawning = true;
        while (true)
        {
            SpawnCustomer();
            yield return new WaitForSeconds(spawnInterval);
        }
    }*/


   public void SpawnCustomer()
    {
        // Havuzdaki tüm objelerin aktif olup olmadığını kontrol et
        bool allActive = true; // Tüm objeler aktif mi kontrolü
        foreach (GameObject customer in customerPool) // Havuzdaki tüm objeleri kontrol et
        { // Eğer bir obje aktif değilse tüm objeler aktif değil demektir
            if (!customer.activeInHierarchy) // Eğer bir obje aktif değilse
            { 
                allActive = false; // Tüm objeler aktif değil
                break; // Döngüyü sonlandır
            }
        }

        // Eğer tüm objeler aktif değilse yeni bir müşteri oluştur
        if (!allActive) // Eğer tüm objeler aktif değilse
        {
            GameObject customer = customerPool[currentCustomerIndex]; // Müşteri havuzundan bir müşteri al
            customer.transform.position = spawnPoint.position; // Müşteriyi spawn pointe yerleştir
            customer.SetActive(true); // Müşteriyi aktif hale getir

            currentCustomerIndex = (currentCustomerIndex + 1) % customerPool.Count; // Bir sonraki müşteri indexini belirle
        }
    }

}
