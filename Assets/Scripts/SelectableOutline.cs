using UnityEngine;

public class SelectableOutline : MonoBehaviour
{
    public static int outlinedLayer;

    private int originalLayer;

    private void Awake()
    {
        // Cache the original layer
        originalLayer = gameObject.layer;
        // Cache the outlined layer once (to avoid string lookup every time)
        outlinedLayer = LayerMask.NameToLayer("Outlined");
    }

    public void Select()
    {
        gameObject.layer = outlinedLayer;
    }

    public void Deselect()
    {
        gameObject.layer = originalLayer;
    }
}
