using System.Collections;
using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    public Transform target; //hedef chair pozisyonu
    public float speed; //customer hareket hızı
    private bool hasReachedTarget = false; //hedefe ulaşıldı mı kontrolü
    public GameObject orderSphere;
    public bool hasOrdered = false; //sipariş verildi mi kontrolü
    public GameObject Pay_Zone;
    public GameObject Exit_Zone;
    private Transform chair; // Müşterinin oturduğu sandalye

    void Start()
    {
        speed = 1f; // Hızı belirle
        Pay_Zone = GameObject.Find("Pay_Zone");
        Exit_Zone = GameObject.Find("Exit_Zone");
    }

    void Update()
    {
        moveCustomer(); // Müşteriyi hareket ettirme metodunu çağır
    }

    private void CheckCustomerTransform()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            DeskStateControl.Chairs.Remove(chair.gameObject); // Müşterinin oturduğu sandalyeyi listeden çıkar
            hasReachedTarget = true; // Hedefe ulaşıldığını işaretle
            SetActive_orderSphere(); // OrderIndicator scriptindeki SetActive_orderSphere metodunu çağır
                        target = null;
            CustomerSpawn.Instance.SpawnCustomer(); // Yeni bir müşteri oluştur
        }
    }

    public void moveCustomer()
    {
        if (target != null && !hasReachedTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            CheckCustomerTransform();
        }
    }

    public void SetRandomTarget()
    {
        if (DeskStateControl.Chairs != null && DeskStateControl.Chairs.Count > 0) // Eğer chairs listesi boş değilse ve chairs listesinde eleman varsa
        {
            int randomIndex = Random.Range(0, DeskStateControl.Chairs.Count); // Rastgele bir index belirle
            chair = DeskStateControl.Chairs[randomIndex].gameObject.transform; // Hedefi rastgele seçilen indexin pozisyonu olarak belirle
            target = chair;
            hasReachedTarget = false; // Hedefe ulaşılmadığını işaretle
        }
        else
        {
            Debug.LogWarning("No available chairs to set as target.");
        }
    }

    public void SetActive_orderSphere()
    {
        orderSphere.SetActive(true); // OrderSphere objesini aktif hale getir
    }

    public void SetDeactive_orderSphere()
    {
        orderSphere.SetActive(false); // OrderSphere objesini pasif hale getir
        Debug.Log("Sipariş Alındı!!!");
    }

    public void moveTo_PayZone()
    {
        transform.position = Vector3.MoveTowards(transform.position, Pay_Zone.transform.position, 2f * Time.deltaTime);
    }

    public IEnumerator WaitAndMoveToPayZone()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Kasaya gidecek" + transform.position);

        while (Vector3.Distance(transform.position, Pay_Zone.transform.position) > 0.01f)
        {
            moveTo_PayZone();
            yield return null; // Bir sonraki frame'i bekle
        }

        Debug.Log("Kasaya ulaştı" + transform.position);
        StartCoroutine(WaitAndMoveToExit());
    }






    public void moveTo_Exit()
    {
        transform.position = Vector3.MoveTowards(transform.position, Exit_Zone.transform.position, 2f * Time.deltaTime);
    }


    public void Destroy_childDonut()
    {
       Transform service = transform.Find("Service_Zone");
      if (service != null)
       { 
        foreach (Transform child in service)
        {
            if (child.tag == "donut")
            {
                Destroy(child.gameObject);
            }
            else
            {
                Debug.Log("Donut yok");
            }
        }
       }
    
    }


    

    public IEnumerator WaitAndMoveToExit()
    {
        yield return new WaitForSeconds(3f);
        while (Vector3.Distance(transform.position, Exit_Zone.transform.position) > 0.01f)
        {
            moveTo_Exit();
            yield return null; // Bir sonraki frame'i bekle
        }

        //donutu yok ettiğin kodu buraya yaz
        Destroy_childDonut();
       // DeskStateControl.Chairs.Add(chair.gameObject); // Müşteri sandalyeden kalktığında sandalyeyi tekrar listeye ekle
        //CustomerSpawn.Instance.ReturnCustomerToPool(gameObject);
    }
}