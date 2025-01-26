using System.Collections.Generic;
using UnityEngine;

public class DeskStateControl : MonoBehaviour
{
    public static List<GameObject> Chairs; // Sandalyelerin listesi


    void Start()
    {
        
        Chairs = new List<GameObject>(); // Sandalyelerin listesini oluştur
        GameObject[] chair = GameObject.FindGameObjectsWithTag("chair"); // Tag'i "chair" olan tüm objeleri bul
        Chairs.AddRange(chair); // Chairs listesine bulunan objeleri ekle (addrange metodu ile listeye bütün chair tagli olanları foreach döngüsü kullanmadan ekledik)
                Debug.Log("Chairs: " + Chairs.Count); // Debug.Log ile konsola sandalye sayısını yazdır

    }
    void Update()
    {
    }

    public static void RemoveAtList() // Chairs listesinden eleman çıkarma metodu 
    {
        if (Chairs.Count > 0) // Eğer Chairs listesinde eleman varsa
       {
            Chairs.RemoveAt(Chairs.Count - 1); // Chairs listesinden son elemanı çıkar
            Debug.Log("remove çalıştı"+Chairs.Count); // Debug.Log ile konsola Chairs listesindeki eleman sayısını yazdır (boş sandalye sayısı)
            
        }
    }
}
