using UnityEngine;

public class MouseAppearingScript : MonoBehaviour
{
    private void Update()
    {
        MouseCursor.SetActive(false);
        if (Sych.ScreenCenterRaycast(out RaycastHit hit))
        {
            Transform hitTransform = hit.transform;
            float maxDistance = 0f;

            if (hitTransform.CompareTag("Door") | hitTransform.CompareTag("TapePlayer"))
            {
                maxDistance = 15;
            }
            else if (hitTransform.CompareTag("Item") | hitTransform.CompareTag("Notebook") | hitTransform.CompareTag("Phone"))
            {
                maxDistance = 10;
            }

            MouseCursor.SetActive(maxDistance > 0 && hitTransform.IsWithinDistanceFrom(playerTransform, maxDistance));
        }
    }

    [Header("References")]
    [SerializeField] private GameObject MouseCursor;
    [SerializeField] private Transform playerTransform;

}