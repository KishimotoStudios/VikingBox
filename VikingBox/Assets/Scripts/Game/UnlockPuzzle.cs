//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using System.Collections;
using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class UnlockPuzzle : MonoBehaviour
    {
        [SerializeField]
        string m_UnlockEventName;
        [SerializeField]
        string m_AfterUnlockedEventName;

        [SerializeField]
        Transform[] m_ObjectsLeft;
        [SerializeField]
        Transform[] m_ObjectsRight;

        [SerializeField]
        float m_MoveUnits = 2.0f;

        [SerializeField]
        float m_TotalSteps = 30.0f;
        [SerializeField]
        float m_Seconds = 2.0f;

        void OnEnable()
        {
            EventManager.Instance.Subscribe(m_UnlockEventName, OnCorrectAnswer);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(m_UnlockEventName, OnCorrectAnswer);
        }

        void OnCorrectAnswer()
        {
            StartCoroutine(Unlock(m_Seconds, m_TotalSteps));
        }

        IEnumerator Unlock(float seconds, float totalSteps)
        {
            //Debug.LogWarning($"{gameObject.name}.Unlock()");

            EventManager.Instance.Notify(GameEvents.Audio.Play, "SFX_Unlock");

            float step = m_MoveUnits / totalSteps;
            float delay = seconds / totalSteps;
            float currentStep = 0.0f;

            while (currentStep < m_MoveUnits)
            {
                for (int i = 0; i < m_ObjectsLeft.Length; ++i)
                {
                    m_ObjectsLeft[i].transform.Translate(-step, 0.0f, 0.0f);
                }

                for (int i = 0; i < m_ObjectsRight.Length; ++i)
                {
                    m_ObjectsRight[i].transform.Translate(step, 0.0f, 0.0f);
                }

                currentStep += step;

                yield return new WaitForSeconds(delay);
            }

            EventManager.Instance.Notify(m_AfterUnlockedEventName);
        }
    }
}