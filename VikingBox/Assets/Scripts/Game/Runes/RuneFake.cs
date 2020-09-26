//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using KishiTech.Core.Events;
using UnityEngine;

namespace VikingBox
{
    public class RuneFake : MonoBehaviour
    {
        MeshRenderer m_MeshRenderer;

        void Awake()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
            m_MeshRenderer.enabled = false;
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe(GameEvents.Runes.CorrectAnswer, OnCorrectAnswer);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(GameEvents.Runes.CorrectAnswer, OnCorrectAnswer);
        }

        void OnCorrectAnswer()
        {
            m_MeshRenderer.enabled = true;
        }
    }
}