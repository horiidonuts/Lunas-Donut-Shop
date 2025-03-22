using System.Collections;
using UnityEngine;
using UnityEngine.AI;
//not: agent.SetDestination kapanmıyor. Muhtemelen checkcustomertransform sonunda kapanması lazım ki kasımınbozukkasasına gidebisin.

public class CustomerMovement : MonoBehaviour
{
    public Transform target; //hedef chair pozisyonu

    public float speed; //customer hareket hızı

    private bool hasReachedTarget = false; //hedefe ulaşıldı mı kontrolü
    public GameObject orderSphere;
    public int randomIndex;

    NavMeshAgent agent;
    //public bool hasCompletedOrder=false;
    // public bool isAtCashRegister=false;

    public bool hasOrdered = false; //sipariş verildi mi kontrolü
    public GameObject Pay_Zone;
    public GameObject Exit_Zone;

    void Start()
    {
        speed = 1f; // Hızı belirle
        SetRandomTarget(); // Rastgele bir hedef belirleme metodunu çağır
        Pay_Zone = GameObject.Find("Pay_Zone");
        Exit_Zone = GameObject.Find("Exit_Zone");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        randomIndex=0;
    }

    void Update()
    {
        moveCustomer(); // Müşteriyi hareket ettirme metodunu çağır
    }

    private void CheckCustomerTransform()
    {
        // Pozisyonları belirli bir tolerans değeri ile karşılaştır
        if (Vector3.Distance(transform.position, target.position) < 0.1f && DeskStateControl.Chairs.Count > 0)
        {
            Debug.Log("Hedefe ulaşıldı");

            DeskStateControl.RemoveAtList(); // Müşterinin oturduğu koltuğu listeden çıkar
            hasReachedTarget = true; // Hedefe ulaşıldığını işaretle
            SetActive_orderSphere(); // OrderIndicator scriptindeki SetActive_orderSphere metodunu çağır
            //burada Spawncustomer metodunu çağırarak yeni bir customer oluşturabilirsiniz
            CustomerSpawn.Instance.SpawnCustomer(); // Yeni bir müşteri oluştur
            randomIndex=randomIndex-1;
            target=null;
        }
    }

    public void moveCustomer()
    {

        if (target != null && !hasReachedTarget) // Eğer hedef belirlenmiş ve hedefe ulaşılmamışsa
        {
            if (Vector3.Distance(transform.position, target.position) >=
            0.1f) // Eğer müşteri ve hedef arasındaki mesafe 0.1f'den büyükse
            {
                agent.isStopped = false;
                agent.SetDestination(target.position); // Müşteriyi hedefe doğru hareket ettir
                //CheckCustomerTransform(); // Müşterinin hedefe ulaşıp ulaşmadığını kontrol et
            }
            else if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                CheckCustomerTransform();
            }
        }
    }

    public void SetRandomTarget()
    {
        if (DeskStateControl.Chairs != null) // Eğer chairs listesi boş değilse ve chairs listesinde eleman varsa
        {
            randomIndex =
                DeskStateControl.Chairs.Count - 1; // Rastgele bir index belirle (listedeki son elemanın indexi)
            target = DeskStateControl.Chairs[randomIndex].gameObject
                .transform; // Hedefi rastgele seçilen indexin pozisyonu olarak belirle
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

    /* public void DestroyDonut()
     {
         Transform donutTransform = transform.Find("Donut(Clone)"); // Müşterinin altında Donut(Clone) objesini ara
         if (donutTransform != null)
         {
             Destroy(donutTransform.gameObject);
         }
         else
         {
             Debug.Log("Donut bulunamadı");
         }
     } */


    public void moveTo_PayZone()
    {
        if (Vector3.Distance(transform.position, Pay_Zone.transform.position) >=
        0.1f) 
        {
            agent.isStopped = false;
            agent.SetDestination(Pay_Zone.transform.position);
        }
        else if (Vector3.Distance(transform.position, Pay_Zone.transform.position) < 0.1f)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            transform.position = Vector3.MoveTowards(transform.position, Pay_Zone.transform.position, 2f * Time.deltaTime);
        }
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
        if (Vector3.Distance(transform.position, Exit_Zone.transform.position) >=
        0.1f) 
        {
            agent.isStopped = false;
            agent.SetDestination(Exit_Zone.transform.position);
        }
        else if (Vector3.Distance(transform.position, Exit_Zone.transform.position) < 0.1f)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            transform.position = Vector3.MoveTowards(transform.position, Exit_Zone.transform.position, 2f * Time.deltaTime);
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
    }
}