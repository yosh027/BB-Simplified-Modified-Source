﻿using UnityEngine;

public class BsodaSparyScript : MonoBehaviour
{
    #region Initialization
    private void Start()
    {
        if (shouldRotate)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = Mathf.Round(Random.Range(0f, 359f));
            transform.eulerAngles = eulerAngles;
        }

        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }
    #endregion

    #region Per-Frame Logic
    private void Update()
    {
        rb.velocity = transform.forward * speed;

        lifeSpan -= Time.deltaTime;
        if (lifeSpan < 0f)
        {
            Destroy(gameObject, 0f);
        }
    }
    #endregion

    #region Serialized Configuration
    [Header("Movement Settings")]
    [SerializeField] private float speed;

    [Header("Lifespan Settings")]
    [SerializeField] private float lifeSpan;

    [Header("Rotation Settings")]
    [SerializeField] private bool shouldRotate;
    #endregion

    #region Internal References
    private Rigidbody rb;
    #endregion
}