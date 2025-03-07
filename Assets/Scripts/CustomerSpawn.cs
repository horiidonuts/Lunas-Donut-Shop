using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class CustomerSpawn : MonoBehaviour
{
    public static CustomerSpawn Instance;
    public GameObject[] customerPrefabs;
    public Transform spawnPoint;
    public int poolSize = 1;
    public List<GameObject> customerPool;
    private bool isSpawning = false; // Müşteri spawn etme kontrolü

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

        customerPool = new List<GameObject>();

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
        bool allActive = true; // Tüm müşteriler aktif mi kontrolü
        foreach (GameObject customer in customerPool) // Havuzdaki tüm müşterileri kontrol et
        {
            if (!customer.activeInHierarchy) // Eğer müşteri aktif değilse
            {
                allActive = false; // Tüm müşteriler aktif değil
                break; // Döngüyü sonlandır
            }
        }

        if (!allActive) // Eğer tüm müşteriler aktif değilse
        {
            GameObject customer = null;
            for (int i = 0; i < customerPool.Count; i++)
            {
                if (!customerPool[i].activeInHierarchy)
                {
                    customer = customerPool[i];
                    customerPool.RemoveAt(i); // Müşteriyi havuzdan çıkar
                    break;
                }
            }

            if (customer != null)
            {
                customer.transform.position = spawnPoint.position; // Müşteriyi spawn noktasına yerleştir
                customer.GetComponent<CustomerMovement>().SetRandomTarget(); // Müşteriye rastgele bir hedef belirle
                customer.SetActive(true); // Müşteriyi aktif et
            }
        }
    }

    public void ReturnCustomerToPool(GameObject customer)
    {
        customer.SetActive(false); // Müşteriyi pasif hale getir
        customerPool.Add(customer); // Müşteriyi havuza geri ekle
    }
}