using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    public Transform target; //hedef chair pozisyonu
    public float speed; //customer hareket hızı
    private bool hasReachedTarget = false; //hedefe ulaşıldı mı kontrolü
    public GameObject orderSphere;



    void Start()
    {
        speed = 1f; // Hızı belirle
      SetRandomTarget(); // Rastgele bir hedef belirleme metodunu çağır

    }

    void Update()
    {
       moveCustomer(); // Müşteriyi hareket ettirme metodunu çağır
    }

    private void CheckCustomerTransform()
    {

        // Pozisyonları belirli bir tolerans değeri ile karşılaştır
        if (Vector3.Distance(transform.position, target.position) < 0.01f && DeskStateControl.Chairs.Count > 0)
        {

            DeskStateControl.RemoveAtList();
            hasReachedTarget = true; // Hedefe ulaşıldığını işaretle
            SetActive_orderSphere(); // OrderIndicator scriptindeki SetActive_orderSphere metodunu çağır
            //burada Spawncustomer metodunu çağırarak yeni bir customer oluşturabilirsiniz
            CustomerSpawn.Instance.SpawnCustomer(); // Yeni bir müşteri oluştur

            target=null;
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
        if (DeskStateControl.Chairs != null) // Eğer chairs listesi boş değilse ve chairs listesinde eleman varsa
        {
            int randomIndex = DeskStateControl.Chairs.Count-1; // Rastgele bir index belirle (listedeki son elemanın indexi)
            target = DeskStateControl.Chairs[randomIndex].gameObject.transform; // Hedefi rastgele seçilen indexin pozisyonu olarak belirle
        }
    }


    public void SetActive_orderSphere()
    { 
        orderSphere.SetActive(true); // OrderSphere objesini aktif hale getir 
        //eray-animasyonla yukarı aşağı hareket ettirebilirsin küreyi.
    }

    public void SetDeactive_orderSphere()
    {
        orderSphere.SetActive(false); // OrderSphere objesini pasif hale getir
        Debug.Log("Sipariş Alındı!!!");
    }


}