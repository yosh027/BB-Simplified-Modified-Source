using UnityEngine;

public class Source : MonoBehaviour
{
    #region MonoBehaviourLifecycle
    private void Start()
    {
        if (InternSPOOP)
        {
            if (GameControllerScript.Instance.spoopMode)
            {
                color = InternSPOOPcolor;
            }
        }
    }
    #endregion

    #region LightSettings
    [Header("Normal Light Settings")]
    public Color color = Color.white;
    public float range, intensity;
    public bool active, reverseLighting;
    #endregion

    #region SpoopModeSupport
    [Header("Custom Bonuses")]
    public bool InternSPOOP = false;
    public Color InternSPOOPcolor = Color.white;
    #endregion
}