using UnityEngine;

public class DonutOrder : MonoBehaviour
{
   // private string CustomerWant_Sauce;
    private string PlayerSelectPrepared_Sauce;
    public enum SauceType { Chocolate, Strawberry, Vanilla }
    public SauceType sauce;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Donut Order: "+sauce);
        }
    }



    public SauceType  GetSauceType()
    {
        return sauce;
    }
}
