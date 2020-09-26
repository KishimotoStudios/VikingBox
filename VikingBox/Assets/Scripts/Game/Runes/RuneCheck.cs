//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class RuneCheck : MonoBehaviour
    {
        RunePlace[] m_RunePlaces;

        void Start()
        {
            m_RunePlaces = transform.GetComponentsInChildren<RunePlace>();
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe(GameEvents.Runes.CheckAnswer, OnCheckRunes);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(GameEvents.Runes.CheckAnswer, OnCheckRunes);
        }

        void OnCheckRunes()
        {
            bool isCorrect = true;
            for (int i = 0; i < m_RunePlaces.Length; ++i)
            {
                isCorrect &= m_RunePlaces[i].IsCorrect();
            }

            if (isCorrect)
            {
                EventManager.Instance.Notify(GameEvents.Runes.CorrectAnswer);
            }
        }
    }
}