 using UnityEngine;

public class SelectableOutline : MonoBehaviour
{
    public static int outlinedLayer;

    private void Awake()
    {
        // Cache the outlined layer once (to avoid string lookup every time)
        outlinedLayer = LayerMask.NameToLayer("Outlined");

        gameObject.layer = outlinedLayer;
    }

    
}
