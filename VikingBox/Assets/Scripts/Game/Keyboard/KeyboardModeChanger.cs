//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class KeyboardModeChanger : MonoBehaviour
    {
        Collider m_Collider;

        void Awake()
        {
            m_Collider = GetComponent<Collider>();
            m_Collider.enabled = false;
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe(GameEvents.Puzzle.ThirdStarted, OnPuzzleStarted);
            EventManager.Instance.Subscribe(GameEvents.Keyboard.CorrectAnswer, OnCorrectAnswer);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.ThirdStarted, OnPuzzleStarted);
            EventManager.Instance.Unsubscribe(GameEvents.Keyboard.CorrectAnswer, OnCorrectAnswer);
        }

        void OnMouseUpAsButton()
        {
            EventManager.Instance.Notify(GameEvents.Audio.Play, "SFX_KeyboardTab");

            EventManager.Instance.Notify(GameEvents.Keyboard.ChangeMode);
        }

        void SetCollider(bool isEnabled)
        {
            m_Collider.enabled = isEnabled;
        }

        void OnPuzzleStarted()
        {
            SetCollider(true);
        }

        void OnCorrectAnswer()
        {
            SetCollider(false);
        }
    }
}