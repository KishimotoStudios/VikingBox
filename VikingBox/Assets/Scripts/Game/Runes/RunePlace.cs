//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class RunePlace : MonoBehaviour
    {
        [SerializeField]
        Rune m_CorrectRune;

        Rune m_CurrentRune;

        Collider m_Collider;

        void Awake()
        {
            m_Collider = GetComponent<Collider>();

            m_CurrentRune = null;
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe<Rune, Transform>(GameEvents.Runes.RuneInPlace, OnRuneInPlace);
            //EventManager.Instance.Subscribe<Transform>(GameEvents.Runes.RuneRemoved, OnRuneRemoved);
            EventManager.Instance.Subscribe(GameEvents.Runes.CorrectAnswer, OnCorrectAnswer);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe<Rune, Transform>(GameEvents.Runes.RuneInPlace, OnRuneInPlace);
            //EventManager.Instance.Unsubscribe<Transform>(GameEvents.Runes.RuneRemoved, OnRuneRemoved);
            EventManager.Instance.Unsubscribe(GameEvents.Runes.CorrectAnswer, OnCorrectAnswer);
        }

        void OnRuneInPlace(Rune rune, Transform place)
        {
            if (place == transform)
            {
                m_CurrentRune = rune;
                EventManager.Instance.Notify(GameEvents.Runes.CheckAnswer);
            }
        }

        void OnCorrectAnswer()
        {
            m_Collider.enabled = false;
        }

        //void OnRuneRemoved(Transform place)
        //{
        //    if (place == transform)
        //    {
        //        m_CurrentRune = null;
        //    }
        //}

        void OnTriggerExit(Collider other)
        {
            if (m_CurrentRune == other.transform.GetComponent<Rune>())
            {
                m_CurrentRune = null;
            }
        }

        public bool IsCorrect()
        {
            return m_CurrentRune == m_CorrectRune;
        }

        public bool IsEmpty()
        {
            return m_CurrentRune == null;
        }
    }
}