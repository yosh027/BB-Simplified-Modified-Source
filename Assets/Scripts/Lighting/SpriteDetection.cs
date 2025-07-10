using UnityEngine;

public class SpriteDetection : MonoBehaviour
{
    #region SetupAndInitialization
    private void Start() => InitializeComponents();

    private void InitializeComponents()
    {
        lightingControl = FindObjectOfType<LightingControl>();
        detectionRoot = GetComponent<Transform>();
    }
    #endregion

    #region SpriteTrackingAndRegistration
    private void Update()
    {
        if (lightingControl == null || detectionRoot == null) return;
        
        SpriteRenderer[] detectedSprites = detectionRoot.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sprite in detectedSprites)
        {
            if (!lightingControl.renders.Contains(sprite))
            {
                lightingControl.renders.Add(sprite);
            }
        }
    }
    #endregion

    #region References
    private Transform detectionRoot;
    private LightingControl lightingControl;
    #endregion
}