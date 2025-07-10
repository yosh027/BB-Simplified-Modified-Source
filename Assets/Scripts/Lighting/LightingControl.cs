using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class LightingControl : MonoBehaviour
{
    #region MonoBehaviourLifecycle
    private void Start()
    {
        ogaddlights = lights;
        ogaddrenders = renders;
        ReCalc();
    }

    private void Update()
    {
        RemoveNullEntries();

        int wallMask = LayerMask.GetMask("Wall");

        foreach (Renderer render in renders)
        {
            if (render != null)
            {
                Color color = ambient;

                foreach (Source light in lights)
                {
                    if (light != null && light.active)
                    {
                        bool isBlocked = false;

                        if (!autoAddEnvironment)
                        {
                            Vector3 direction = render.transform.position - light.transform.position;
                            float distance = direction.magnitude;

                            Ray ray = new Ray(light.transform.position, direction.normalized);
                            isBlocked = Physics.Raycast(ray, out _, distance, wallMask);
                        }

                        if (!isBlocked)
                        {
                            Vector3 direction = render.transform.position - light.transform.position;
                            float distance = direction.magnitude;

                            float dist = distance / light.range;
                            dist = Mathf.Clamp01(1 - dist);
                            float addr = light.color.r * dist * light.intensity;
                            float addg = light.color.g * dist * light.intensity;
                            float addb = light.color.b * dist * light.intensity;

                            if (light.reverseLighting)
                            {
                                addr *= -1;
                                addg *= -1;
                                addb *= -1;
                            }

                            color.r = Mathf.Clamp(color.r + addr, 0, 1);
                            color.g = Mathf.Clamp(color.g + addg, 0, 1);
                            color.b = Mathf.Clamp(color.b + addb, 0, 1);
                        }
                    }
                }

                Color finalColor = new Color(color.r, color.g, color.b, 1.0f);

                if (render is SpriteRenderer spriteRenderer)
                {
                    spriteRenderer.color = finalColor;
                }
                else
                {
                    if (render.material.HasProperty("_Color0"))
                    {
                        render.material.SetColor("_Color0", color);
                    }
                    if (render.material.HasProperty("_Color1"))
                    {
                        render.material.SetColor("_Color1", color);
                    }
                    if (render.material.HasProperty("_Color"))
                    {
                        Color baseColor = render.material.GetColor("_Color");
                        render.material.SetColor("_Color", new Color(color.r, color.g, color.b, baseColor.a));
                    }
                }
            }
        }
    }
    #endregion

    #region LightingAndRendererSetup
    public void ReCalc()
    {
        if (autoAddEnvironment)
        {
            renders.Clear();
            lights.Clear();

            foreach (Renderer render in ogaddrenders)
            {
                renders.Add(render);
            }
            foreach (Source light in ogaddlights)
            {
                lights.Add(light);
            }
            foreach (MeshRenderer render in environment.gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                if (!render.gameObject.GetComponent<TextMeshPro>())
                {
                    renders.Add(render);
                }
            }
            foreach (SpriteRenderer render in environment.gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                renders.Add(render);
            }
            foreach (Source light in environment.GetComponentsInChildren<Source>())
            {
                lights.Add(light);
            }
        }
        else
        {
            foreach (GameObject group in renderGroups)
            {
                foreach (Renderer render in group.GetComponentsInChildren<Renderer>())
                {
                    if (!renders.Contains(render))
                    {
                        renders.Add(render);
                    }
                }
            }
        }

        foreach (MeshRenderer render in AdditionalMeshes)
        {
            renders.Add(render);
        }
        foreach (SpriteRenderer render in AdditionalSprites)
        {
            renders.Add(render);
        }
        foreach (Source light in AdditionalLights)
        {
            lights.Add(light);
        }
    }
    #endregion

    #region Utility
    private void RemoveNullEntries() => renders.RemoveAll(item => item == null);
    #endregion

    #region SerializedFields
    [Header("Renderers")]
    [SerializeField] private List<GameObject> renderGroups;
    public List<Renderer> renders;
    [SerializeField] private List<Source> lights;

    [Header("Serialized References")]
    [SerializeField] private bool autoAddEnvironment = true;
    [SerializeField] private Transform environment;
    [SerializeField] private Color ambient;

    [Header("Miscellaneous")]
    [SerializeField] private List<SpriteRenderer> AdditionalSprites;
    [SerializeField] private List<MeshRenderer> AdditionalMeshes;
    [SerializeField] private List<Source> AdditionalLights;
    #endregion

    #region PrivateState
    List<Source> ogaddlights;
    List<Renderer> ogaddrenders;
    #endregion
}