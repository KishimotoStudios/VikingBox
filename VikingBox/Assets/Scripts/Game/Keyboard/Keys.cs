//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class Keys : MonoBehaviour
    {
        [Header("Key Config")]
        [SerializeField]
        int m_NoteIndex = -1;
        [SerializeField]
        float m_Rotation = 2.0f;
        [SerializeField]
        KeyCode m_RealKeyboard;

        Quaternion m_OriginalRotation;
        bool m_IsMouseDown;
        bool m_DidPlayNote;

        Collider m_Collider;

        void Awake()
        {
            m_Collider = GetComponent<Collider>();
            m_Collider.enabled = false;

            m_OriginalRotation = transform.rotation;

            m_IsMouseDown = false;
            m_DidPlayNote = false;
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe(GameEvents.Puzzle.ThirdStarted, OnPuzzleStarted);
            EventManager.Instance.Subscribe(GameEvents.Keyboard.CorrectAnswer, OnCorrectAnswer);
            EventManager.Instance.Subscribe<int>(GameEvents.Letter.Open, OnOpenLetter);
            EventManager.Instance.Subscribe<int>(GameEvents.Letter.Close, OnCloseLetter);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.ThirdStarted, OnPuzzleStarted);
            EventManager.Instance.Unsubscribe(GameEvents.Keyboard.CorrectAnswer, OnCorrectAnswer);
            EventManager.Instance.Unsubscribe<int>(GameEvents.Letter.Open, OnOpenLetter);
            EventManager.Instance.Unsubscribe<int>(GameEvents.Letter.Close, OnCloseLetter);
        }

        void OnMouseDown()
        {
            PressKey();
        }

        void OnMouseEnter()
        {
            if (m_IsMouseDown)
            {
                PressKey();
            }
        }

        void OnMouseExit()
        {
            if (m_IsMouseDown)
            {
                ReturnToOriginalRotation(m_IsMouseDown);
            }
        }

        void OnMouseUp()
        {
            ReturnToOriginalRotation();
        }

        void ReturnToOriginalRotation(bool keepMouseDown = false)
        {
            transform.rotation = m_OriginalRotation;
            m_IsMouseDown = keepMouseDown;

            if (!m_IsMouseDown)
            {
                m_DidPlayNote = false;

                EventManager.Instance.Notify(GameEvents.Keyboard.CheckAnswer);
            }
        }

        void PressKey()
        {
            m_IsMouseDown = true;
            transform.Rotate(m_Rotation, 0.0f, 0.0f);

            if (!m_DidPlayNote)
            {
                m_DidPlayNote = true;

                EventManager.Instance.Notify(GameEvents.Keyboard.PlayNote, m_NoteIndex);
            }
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

        void OnOpenLetter(int index)
        {
            if (index == 3)
            {
                SetCollider(false);
            }
        }

        void OnCloseLetter(int index)
        {
            if (index == 3)
            {
                SetCollider(true);
            }
        }

        private void Update()
        {
            if (m_Collider.enabled)
            {
                if (Input.GetKeyDown(m_RealKeyboard))
                {
                    PressKey();
                }
                if (Input.GetKeyUp(m_RealKeyboard))
                {
                    ReturnToOriginalRotation();
                }
            }
        }
    }
}