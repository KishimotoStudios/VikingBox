//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class YearDigit : MonoBehaviour
    {
        int m_Digit = 0;
        public int Digit
        {
            get { return m_Digit; }
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe<Transform>(GameEvents.Year.DigitUp, OnDigitUp);
            EventManager.Instance.Subscribe<Transform>(GameEvents.Year.DigitDown, OnDigitDown);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe<Transform>(GameEvents.Year.DigitUp, OnDigitUp);
            EventManager.Instance.Unsubscribe<Transform>(GameEvents.Year.DigitDown, OnDigitDown);
        }

        void OnDigitUp(Transform yearCog)
        {
            if (yearCog == transform)
            {
                Inc();
                EventManager.Instance.Notify(GameEvents.Year.CheckAnswer);
            }
        }

        void OnDigitDown(Transform yearCog)
        {
            if (yearCog == transform)
            {
                Dec();
                EventManager.Instance.Notify(GameEvents.Year.CheckAnswer);
            }
        }

        void Inc()
        {
            ++m_Digit;
            if (m_Digit > 9)
            {
                m_Digit = 0;
            }
        }

        void Dec()
        {
            --m_Digit;
            if (m_Digit < 0)
            {
                m_Digit = 9;
            }
        }
    }
}