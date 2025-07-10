﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MaterialKit
{
	
#if UNITY_EDITOR
	[RequireComponent(typeof(Canvas)), ExecuteInEditMode, AddComponentMenu("Layout/DP Canvas Scaler")]
#endif

    public class DpCanvasScaler : UIBehaviour
    {
        #region Constructors
        protected DpCanvasScaler() { }
        #endregion

        #region Properties
        public float referencePixelsPerUnit
        {
            get => m_ReferencePixelsPerUnit;
            set => m_ReferencePixelsPerUnit = value;
        }

        public float fallbackScreenDPI
        {
            get => m_FallbackScreenDPI;
            set => m_FallbackScreenDPI = value;
        }

        public float defaultSpriteDPI
        {
            get => m_DefaultSpriteDPI;
            set => m_DefaultSpriteDPI = value;
        }

        public float dynamicPixelsPerUnit
        {
            get => m_DynamicPixelsPerUnit;
            set => m_DynamicPixelsPerUnit = value;
        }
        #endregion

        #region UnityCallbacks
        protected override void OnEnable()
        {
            base.OnEnable();
            m_Canvas = GetComponent<Canvas>();
            Handle();
        }

        protected override void OnDisable()
        {
            SetScaleFactor(1f);
            SetReferencePixelsPerUnit(100f);
            base.OnDisable();
        }

        protected virtual void Update() => Handle();
        #endregion

        #region Handling
        protected virtual void Handle()
        {
            if (m_Canvas == null || !m_Canvas.isRootCanvas) return;

            if (m_Canvas.renderMode == RenderMode.WorldSpace)
            {
                HandleWorldCanvas();
            }
            else
            {
                HandleConstantPhysicalSize();
            }
        }

        protected virtual void HandleWorldCanvas()
        {
            SetScaleFactor(m_DynamicPixelsPerUnit);
            SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit);
        }

        protected virtual void HandleConstantPhysicalSize()
        {
            float dpi = Screen.dpi;
            float effectiveDPI = (dpi != 0f) ? dpi : m_FallbackScreenDPI;
            float baseDPI = 160f;
            SetScaleFactor(effectiveDPI / baseDPI);
            SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit * baseDPI / m_DefaultSpriteDPI);
        }
        #endregion

        #region Helpers
        protected void SetScaleFactor(float scaleFactor)
        {
            if (scaleFactor == m_PrevScaleFactor) return;
            m_Canvas.scaleFactor = scaleFactor;
            m_PrevScaleFactor = scaleFactor;
        }

        protected void SetReferencePixelsPerUnit(float referencePixelsPerUnit)
        {
            if (referencePixelsPerUnit == m_PrevReferencePixelsPerUnit) return;
            m_Canvas.referencePixelsPerUnit = referencePixelsPerUnit;
            m_PrevReferencePixelsPerUnit = referencePixelsPerUnit;
        }
        #endregion

        #region SerializedFields
        [Tooltip("If a sprite has this 'Pixels Per Unit' setting, then one pixel in the sprite will cover one unit in the UI.")]
        [SerializeField] protected float m_ReferencePixelsPerUnit = 100f;

        [Tooltip("The DPI to assume if the screen DPI is not known.")]
        [SerializeField] protected float m_FallbackScreenDPI = 96f;

        [Tooltip("The pixels per inch to use for sprites that have a 'Pixels Per Unit' setting that matches the 'Reference Pixels Per Unit' setting.")]
        [SerializeField] protected float m_DefaultSpriteDPI = 96f;

        [Tooltip("The amount of pixels per unit to use for dynamically created bitmaps in the UI, such as Text.")]
        [SerializeField] protected float m_DynamicPixelsPerUnit = 1f;
        #endregion

        #region PrivateFields
        private Canvas m_Canvas;
        [NonSerialized] private float m_PrevScaleFactor = 1f;
        [NonSerialized] private float m_PrevReferencePixelsPerUnit = 100f;
        #endregion
    }
}