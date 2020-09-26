//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class YearCheck : MonoBehaviour
    {
        enum InitialYear
        {
            None,
            BasedOnTime,
            BasedOnDate,
        }

        [SerializeField]
        InitialYear m_InitialYear;

        [SerializeField]
        int m_YearToCheck;

        YearDigit[] m_YearDigits;

        void Start()
        {
            m_YearDigits = transform.GetComponentsInChildren<YearDigit>();
            SetInitialValue();
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe(GameEvents.Year.CheckAnswer, OnCheckYear);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(GameEvents.Year.CheckAnswer, OnCheckYear);
        }

        void OnCheckYear()
        {
            int currentYear = m_YearDigits[0].Digit * 1000
                + m_YearDigits[1].Digit * 100
                + m_YearDigits[2].Digit * 10
                + m_YearDigits[3].Digit;

            //Debug.Log($"YearCheck: current year is {currentYear}, expected: {m_YearToCheck}");

            if (currentYear == m_YearToCheck)
            {
                EventManager.Instance.Notify(GameEvents.Year.CorrectAnswer);
            }
        }

        void SetInitialValue()
        {
            int[] initialValues = { 0, 0, 0, 0 };

            if (m_InitialYear == InitialYear.BasedOnTime)
            {
                int hour = System.DateTime.Now.Hour;
                int minute = System.DateTime.Now.Minute;
                initialValues[0] = hour / 10;
                initialValues[1] = hour - (initialValues[0] * 10);
                initialValues[2] = minute / 10;
                initialValues[3] = minute - (initialValues[2] * 10);
            }
            else if (m_InitialYear == InitialYear.BasedOnDate)
            {
                int day = System.DateTime.Now.Day;
                int month = System.DateTime.Now.Month;
                initialValues[0] = day / 10;
                initialValues[1] = day - (initialValues[0] * 10);
                initialValues[2] = month / 10;
                initialValues[3] = month - (initialValues[3] * 10);
            };

            if (m_InitialYear != InitialYear.None)
            {
                for (int i = 0; i < initialValues.Length; ++i)
                {
                    EventManager.Instance.Notify(GameEvents.Year.SetInitialValue, m_YearDigits[i].transform.parent.GetComponentInChildren<YearArrow>(), initialValues[i]);
                }
            }
        }
    }
}