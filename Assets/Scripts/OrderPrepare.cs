using UnityEngine;
using DG.Tweening;

public class OrderPrepare : MonoBehaviour
{

    public static OrderPrepare Instance;
    public Transform Player_ServiceTransform;

    public GameObject Donut_Prefab;
    public GameObject Donut;
    Vector3 Donut_Position;

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        Donut_Position= new Vector3(this.gameObject.transform.position.x, 
        this.gameObject.transform.position.y+0.15f, this.gameObject.transform.position.z);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DonutOrder_prepare();
        }
    }

//0.02+y olsun
    public void DonutOrder_prepare( )
    {
         Donut=Instantiate(Donut_Prefab,Donut_Position,Quaternion.identity);
        Debug.Log("Donut Order Prepared"); 
       Donut.transform.DOJump(Player_ServiceTransform.position, 0.2f, 1, 1f).OnComplete(()=>{
        Donut.transform.SetParent(Player_ServiceTransform); // Set the parent to Player_ServiceTransform
    
        }); 
    }


    public void SetDonutParent(Transform parentTransform) 
    {
        if (Donut != null)
    {        
        Donut.transform.SetParent(parentTransform); // Keep world position
        Donut.transform.localPosition = Vector3.zero;
    }
   }
    
}
