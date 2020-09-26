//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using System.Collections;
using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class CameraController : MonoBehaviour
    {
        [Header("Next Camera Position")]
        [SerializeField]
        float m_NextPositionDelay = 0.025f;
        [SerializeField]
        float m_LerpPercentage = 0.1f;

        [Header("Screen Shake Config")]
        [SerializeField]
        float m_ScreenShakeDelay = 0.1f;
        [SerializeField]
        float m_ScreenShakeOffset = 0.075f;

        [SerializeField]
        Vector3[] m_PuzzlePosition;

        Vector3 m_OriginalPosition;
        int m_CurrentPosition;
        
        Vector3 m_ShakePosition;
        bool m_IsScreenShaking;

        void Awake()
        {
            m_CurrentPosition = 0;
            m_OriginalPosition = m_PuzzlePosition[m_CurrentPosition];

            m_ShakePosition = Vector3.zero;
            m_IsScreenShaking = false;
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe(GameEvents.Year.CorrectAnswer, ScreenShake);
            EventManager.Instance.Subscribe(GameEvents.Runes.CorrectAnswer, ScreenShake);
            EventManager.Instance.Subscribe(GameEvents.Keyboard.CorrectAnswer, ScreenShake);
            EventManager.Instance.Subscribe(GameEvents.Puzzle.FirstEnvelopeUnlocked, OnPuzzleUnlocked);
            EventManager.Instance.Subscribe(GameEvents.Puzzle.LastEnvelopeUnlocked, OnPuzzleUnlocked);
            EventManager.Instance.Subscribe(GameEvents.Puzzle.FirstUnlocked, OnPuzzleUnlocked);
            EventManager.Instance.Subscribe(GameEvents.Puzzle.SecondUnlocked, OnPuzzleUnlocked);
            EventManager.Instance.Subscribe(GameEvents.Puzzle.ThirdUnlocked, OnPuzzleUnlocked);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(GameEvents.Year.CorrectAnswer, ScreenShake);
            EventManager.Instance.Unsubscribe(GameEvents.Runes.CorrectAnswer, ScreenShake);
            EventManager.Instance.Unsubscribe(GameEvents.Keyboard.CorrectAnswer, ScreenShake);
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.FirstEnvelopeUnlocked, OnPuzzleUnlocked);
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.LastEnvelopeUnlocked, OnPuzzleUnlocked);
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.FirstUnlocked, OnPuzzleUnlocked);
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.SecondUnlocked, OnPuzzleUnlocked);
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.ThirdUnlocked, OnPuzzleUnlocked);
        }

        void ScreenShake()
        {
            StartCoroutine(StartScreenShake(m_ScreenShakeDelay));
        }

        IEnumerator StartScreenShake(float delay)
        {
            EventManager.Instance.Notify(GameEvents.Camera.ScreenShakeStarted);
            m_IsScreenShaking = true;

            yield return new WaitForSeconds(delay);

            while (m_IsScreenShaking)
            {
                m_ShakePosition.x = Random.Range(-m_ScreenShakeOffset, m_ScreenShakeOffset);
                m_ShakePosition.z = Random.Range(-m_ScreenShakeOffset, m_ScreenShakeOffset);
                transform.position = m_OriginalPosition + m_ShakePosition;

                yield return new WaitForEndOfFrame();
            }
        }

        void OnPuzzleUnlocked()
        {
            GotoNextPosition();
        }

        void GotoNextPosition()
        {
            transform.position = m_OriginalPosition;
            m_IsScreenShaking = false;

            ++m_CurrentPosition;
            m_OriginalPosition = m_PuzzlePosition[m_CurrentPosition];
            StartCoroutine(MoveCamera(m_NextPositionDelay));
        }

        IEnumerator MoveCamera(float delay)
        {
            while (Vector3.Distance(transform.position, m_OriginalPosition) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, m_OriginalPosition, m_LerpPercentage);
                yield return new WaitForSeconds(delay);
            }
            transform.position = m_OriginalPosition;

            EventManager.Instance.Notify(GameEvents.Camera.ScreenShakeFinished);
        }
    }
}