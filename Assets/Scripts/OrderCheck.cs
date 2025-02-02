using UnityEngine;

public class OrderCheck : MonoBehaviour
{

    public static OrderCheck Instance;
    //takeorder scripti içinde e tıklayınca temas edilen customerin donutorder scriptine eriş



//Kontrol burada yapacağız
    private void Awake()
    {
        Instance = this;
    }



    void Start()
    {
    }

    void Update()
    {
       
    }


    public void CheckOrder()
    {

         Debug.Log("PlayerSelectPrepared_Sauce: "+PanelAnimHandler.Instance.PlayerSelectPrepared_Sauce);
         Debug.Log("CustomerWant_Sauce"+TakeOrder.Instance.CustomerWant_Sauce);

         if (PanelAnimHandler.Instance.PlayerSelectPrepared_Sauce == TakeOrder.Instance.CustomerWant_Sauce)
        {
            Debug.Log("Sipariş doğru");
        }
        else
        {
            Debug.Log("Sipariş yanlış");
        }


    }

   





}
