//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using System.Collections;
using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class YearArrow : MonoBehaviour
    {
        [Header("Button Config")]
        [SerializeField]
        float m_PushDownY = 0.05f;
        [SerializeField]
        bool m_IsUpArrow;
        [SerializeField]
        float m_StepMultiplier = 3.0f;

        [Header("Year Config")]
        [SerializeField]
        Transform m_YearCog;
        [SerializeField]
        float m_RotationX = 36.0f;

        Vector3 m_OriginalPosition;
        Vector3 m_CurrentPosition;

        bool m_IsMouseDown;
        bool m_IsRotating;

        string m_EventName;

        Collider m_Collider;

        void Awake()
        {
            m_Collider = GetComponent<Collider>();
            m_Collider.enabled = false;

            m_CurrentPosition = m_OriginalPosition = transform.position;
            m_IsMouseDown = false;
            m_IsRotating = false;
            m_EventName = m_IsUpArrow ? GameEvents.Year.DigitUp : GameEvents.Year.DigitDown;
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe<YearArrow, int>(GameEvents.Year.SetInitialValue, OnSetInitialValue);
            EventManager.Instance.Subscribe(GameEvents.Puzzle.FirstStarted, OnPuzzleStarted);
            EventManager.Instance.Subscribe(GameEvents.Year.CorrectAnswer, OnCorrectAnswer);
            EventManager.Instance.Subscribe<int>(GameEvents.Letter.Open, OnOpenLetter);
            EventManager.Instance.Subscribe<int>(GameEvents.Letter.Close, OnCloseLetter);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe<YearArrow, int>(GameEvents.Year.SetInitialValue, OnSetInitialValue);
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.FirstStarted, OnPuzzleStarted);
            EventManager.Instance.Unsubscribe(GameEvents.Year.CorrectAnswer, OnCorrectAnswer);
            EventManager.Instance.Unsubscribe<int>(GameEvents.Letter.Open, OnOpenLetter);
            EventManager.Instance.Unsubscribe<int>(GameEvents.Letter.Close, OnCloseLetter);
        }

        void OnMouseDown()
        {
            if (!m_IsRotating)
            {
                EventManager.Instance.Notify(GameEvents.Audio.Play, "SFX_Button");

                m_CurrentPosition = m_OriginalPosition;
                m_CurrentPosition.y -= m_PushDownY;
                transform.position = m_CurrentPosition;

                m_IsMouseDown = true;
            }
        }

        void OnMouseEnter()
        {
            if (m_IsMouseDown)
            {
                OnMouseDown();
            }
        }

        void OnMouseExit()
        {
            if (!m_IsRotating)
            {
                ReturnToOriginalPosition(m_IsMouseDown);
            }
        }

        void OnMouseUp()
        {
            if (!m_IsRotating)
            {
                ReturnToOriginalPosition();
            }
        }

        void OnMouseUpAsButton()
        {
            if (!m_IsRotating)
            {
                EventManager.Instance.Notify(GameEvents.Audio.Play, "SFX_Cog");
                StartCoroutine(RotateYearCog(0.0f, m_RotationX));
            }
        }

        void OnSetInitialValue(YearArrow yearArrow, int value)
        {
            if (this == yearArrow)
            {
                for (int i = 0; i < value; ++i)
                {
                    m_YearCog.transform.Rotate(Vector3.right, m_RotationX);
                    EventManager.Instance.Notify(GameEvents.Year.DigitUp, m_YearCog);
                }
            }
        }

        void ReturnToOriginalPosition(bool keepMouseDown = false)
        {
            transform.position = m_OriginalPosition;
            m_IsMouseDown = keepMouseDown;
        }

        IEnumerator RotateYearCog(float startAngle, float endAngle)
        {
            m_IsRotating = true;

            float angle = startAngle;
            float step = ((endAngle - startAngle) / Mathf.Abs(m_RotationX)) * m_StepMultiplier;

            while (Mathf.Abs(endAngle - angle) > 0.01f)
            {
                angle += step;
                m_YearCog.transform.Rotate(Vector3.right, step);

                yield return new WaitForEndOfFrame();
            }

            EventManager.Instance.Notify(m_EventName, m_YearCog);

            ReturnToOriginalPosition();
            m_IsRotating = false;
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
            ReturnToOriginalPosition();
            GetComponent<Collider>().enabled = false;
        }

        void OnOpenLetter(int index)
        {
            if (index == 1)
            {
                SetCollider(false);
            }
        }

        void OnCloseLetter(int index)
        {
            if (index == 1)
            {
                SetCollider(true);
            }
        }
    }
}