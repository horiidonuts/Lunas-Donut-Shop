using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class WallFade : MonoBehaviour
{
    [SerializeField] float fadeDuration = 1f;
    private Material material;
    private Color originalColor;

    [SerializeField] float minFade;
    [SerializeField] float fadeSpeed = 0.2f;
    [SerializeField] public bool faded = false;
    [SerializeField] GameObject[] walls;
    Dictionary<GameObject, Material> materials = new Dictionary<GameObject, Material>();
    void Start()
    {
        foreach (GameObject wall in walls)
        {
            if (wall.TryGetComponent<Renderer>(out Renderer renderer))
            {
                materials[wall] = renderer.material;
            }
            
        }

        if (materials.Count == 0)
        {
            originalColor=materials[walls[0]].color;
        }
        
         fadeSpeed = 1f / fadeDuration;
    }
    
    void Update()
    {

        foreach (var entry in materials)
        {
            Material material = entry.Value;
            
            if (faded && material.color.a > minFade)
            {
                Color newColor = material.color;
                newColor.a -= fadeSpeed * Time.deltaTime;
                material.color = newColor;
            }
            
            else if (!faded && material.color.a < 1)
            {
                Color newColor = material.color;
                newColor.a += fadeSpeed * Time.deltaTime;
                material.color = newColor;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (!faded)
        {
            faded = true;
            Debug.Log("Walls Fading");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (faded)
        {
            faded = false;
            Debug.Log("Walls Unfading");
        }
        
    }
}