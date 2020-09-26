//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class Rune : MonoBehaviour
    {
        [SerializeField]
        string m_RuneName;

        Camera m_Camera;

        Vector3 m_OriginalPosition;
        Vector3 m_Position;

        bool m_IsDragging;
        Transform m_CurrentRunePlace;

        Collider m_Collider;

        void Awake()
        {
            m_Collider = GetComponent<Collider>();
            m_Collider.enabled = false;

            m_OriginalPosition = transform.position;
            m_Position = Vector3.zero;
            m_IsDragging = false;
            m_CurrentRunePlace = null;
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe(GameEvents.Puzzle.SecondStarted, OnPuzzleStarted);
            EventManager.Instance.Subscribe(GameEvents.Runes.CorrectAnswer, OnCorrectAnswer);
            EventManager.Instance.Subscribe<int>(GameEvents.Letter.Open, OnOpenLetter);
            EventManager.Instance.Subscribe<int>(GameEvents.Letter.Close, OnCloseLetter);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.SecondStarted, OnPuzzleStarted);
            EventManager.Instance.Unsubscribe(GameEvents.Runes.CorrectAnswer, OnCorrectAnswer);
            EventManager.Instance.Unsubscribe<int>(GameEvents.Letter.Open, OnOpenLetter);
            EventManager.Instance.Unsubscribe<int>(GameEvents.Letter.Close, OnCloseLetter);
        }

        void Start()
        {
            m_Camera = Camera.main;
        }

        void OnMouseDown()
        {
            m_IsDragging = true;
        }

        void OnMouseUp()
        {
            StopDragging();
        }

        void StopDragging()
        {
            m_IsDragging = false;

            if (m_CurrentRunePlace == null
                || m_CurrentRunePlace != null && !m_CurrentRunePlace.GetComponent<RunePlace>().IsEmpty())
            {
                EventManager.Instance.Notify(GameEvents.Audio.Play, "SFX_StoneOut");

                transform.position = m_OriginalPosition;
                m_CurrentRunePlace = null;
            }
            else
            {
                EventManager.Instance.Notify(GameEvents.Audio.Play, "SFX_StoneIn");

                transform.position = m_CurrentRunePlace.position;
                EventManager.Instance.Notify(GameEvents.Runes.RuneInPlace, this, m_CurrentRunePlace);
            }
        }

        void Update()
        {
            if (m_IsDragging)
            {
                m_Position = Input.mousePosition;
                m_Position.z = m_Camera.transform.position.y - m_OriginalPosition.y;
                transform.position = m_Camera.ScreenToWorldPoint(m_Position);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            SetCurrentRunePlace(other.transform);
        }

        void OnTriggerStay(Collider other)
        {
            SetCurrentRunePlace(other.transform);
        }

        void OnTriggerExit(Collider other)
        {
            if (m_CurrentRunePlace == other.transform)
            {
                m_CurrentRunePlace = null;
            }
        }

        void SetCurrentRunePlace(Transform other)
        {
            if (m_CurrentRunePlace == null || m_CurrentRunePlace != other)
            {
                m_CurrentRunePlace = other;
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
            gameObject.SetActive(false);
        }

        void OnOpenLetter(int index)
        {
            if (index == 2)
            {
                SetCollider(false);
            }
        }

        void OnCloseLetter(int index)
        {
            if (index == 2)
            {
                SetCollider(true);
            }
        }
    }
}