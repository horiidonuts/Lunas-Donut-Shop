using System.Collections.Generic;
using UnityEngine;

public class DeskStateControl : MonoBehaviour
{
        public static List<GameObject> Chairs; // Sandalyelerin listesi

    void Start()
    {
        Chairs = new List<GameObject>(); // Sandalyelerin listesini oluştur
        GameObject[] chairs = GameObject.FindGameObjectsWithTag("chair"); // Tag'i "chair" olan tüm objeleri bul
        Chairs.AddRange(chairs); // Sandalyelerin listesine bulunan objeleri ekle
        Debug.Log("Chairs count: " + Chairs.Count); // Sandalyelerin listesindeki eleman sayısını konsola yazdır
    }

    public static void RemoveAtList()
    {
        if (Chairs.Count > 0) // Eğer sandalyelerin listesi boş değilse
        {
            Chairs.RemoveAt(Chairs.Count - 1); // Sandalyelerin listesinden son elemanı çıkar
            Debug.Log("remove çalıştı"+ Chairs.Count); // Konsola "remove çalıştı" yazdır ve sandalyelerin listesindeki eleman sayısını yazdır
        }
    }



}
