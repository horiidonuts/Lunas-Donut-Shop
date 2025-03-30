using UnityEngine;
using DG.Tweening;

public class PanelFollowDonut : MonoBehaviour
{
    [SerializeField] private Transform target3DObject; // Assign the 3D object to follow
    [SerializeField] private RectTransform uiPanel; // Assign the UI panel
    [SerializeField] private Camera mainCamera; // Assign your camera (or use Camera.main)
    [SerializeField] private Ease easing;
    [SerializeField] private float followDuration;
        
    private void Update()
    {
        if (!target3DObject || !uiPanel || !mainCamera)
            return;

        // Convert world position to screen position
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target3DObject.position);

        // Check if the object is in front of the camera
        if (screenPos.z > 0)
        {
            uiPanel.transform.DOMove(screenPos, followDuration).SetEase(easing);
        }
    }
}