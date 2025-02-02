using System;
using Unity.VisualScripting;
using UnityEngine;

public class PanelAnimHandler : MonoBehaviour
{

    public static PanelAnimHandler Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] GameObject player; //oyuncu nesnesini tutar
    [SerializeField] private bool canProcessOrder; //sipariş alınıp alınamayacağını kontrol eder
    [SerializeField] private bool _playerHasOrder; //oyuncunun sipariş alıp almadığını kontrol eder
    [SerializeField] Animator anim; //panel animasyonunu kontrol eder

        public string PlayerSelectPrepared_Sauce;



//seçimleri buradan alacağız




    void Start()
    {
       // anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (canProcessOrder && Input.GetKeyDown(KeyCode.E) && _playerHasOrder) //oyuncu
        {
            OpenPanel();
            
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player") //etiketi oyuncu olan nesne mutfak tezgahına yaklaştığında
        {
           // player = other.gameObject; //oyuncu nesnesini player değişkenine ata
            canProcessOrder = true; //sipariş alındıya hazırlanabilir (işlenebilir) durumunu true yap (karakter mutfak teezgahının içine girince)
            GetPlayerOrderState(); 

        }
    }


    void OnTriggerExit(Collider other)
    {
        canProcessOrder = false;
    }


     public void PlayerSelectSauce_Chocolate()
    {
        PlayerSelectPrepared_Sauce = "Chocolate";
        Debug.Log("PlayerSelectPrepared_Sauce: "+PlayerSelectPrepared_Sauce);
    }


     public void PlayerSelectSauce_Strawberry()
    {
        PlayerSelectPrepared_Sauce = "Strawberry";
        
    }


    public void PlayerSelectSauce_Vanilla()
    {
        PlayerSelectPrepared_Sauce = "Vanilla";
    }







    
    void OpenPanel()
    {
        anim.SetBool("panel_open", true);
    }

    public void ClosePanel()
    {
        anim.SetBool("panel_open", false);
        player.GetComponent<TakeOrder>().playerHasOrder = false;

        OrderCheck.Instance.CheckOrder();
        

        //donut instantiate olacak TakeOrder scripti içinde
    }

    private void GetPlayerOrderState()
    {
      _playerHasOrder = TakeOrder.Instance.HasPlayerOrder(); //daha okunaklı olması için takeorderdan playerHasOrderı return ettim
    }

    



}
