//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class KeyboardTab : MonoBehaviour
    {
        Material m_Material;
        Color m_Color;

        void Awake()
        {
            m_Color = new Color(0.125f, 0.0f, 0.0f);

            m_Material = GetComponent<MeshRenderer>().sharedMaterial;
            m_Material.SetColor("_Color", m_Color);// Color.black);
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe<KeyboardMode>(GameEvents.Keyboard.AfterChangeMode, OnChangeMode);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe<KeyboardMode>(GameEvents.Keyboard.AfterChangeMode, OnChangeMode);
        }

        void OnChangeMode(KeyboardMode keyboardMode)
        {
            if (keyboardMode == KeyboardMode.Puzzle)
            {
                m_Material.SetColor("_Color", Color.white);
            }
            else
            {
                m_Material.SetColor("_Color", m_Color);// Color.black);
            }
        }
    }
}