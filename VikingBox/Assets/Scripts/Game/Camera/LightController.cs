//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using System.Collections;
using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class LightController : MonoBehaviour
    {
        [SerializeField]
        float m_TurnOffLightSpeed = 10.0f;

        Light m_Light;

        void Awake()
        {
            m_Light = GetComponent<Light>();
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe(GameEvents.Puzzle.LastEnvelopeUnlocked, OnLastEnvelopeUnlocked);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.LastEnvelopeUnlocked, OnLastEnvelopeUnlocked);
        }

        void OnLastEnvelopeUnlocked()
        {
            StartCoroutine(TurnOffLight());
        }

        IEnumerator TurnOffLight()
        {
            while (m_Light.range > 0.0f)
            {
                m_Light.range -= Time.deltaTime * m_TurnOffLightSpeed;
                yield return new WaitForEndOfFrame();
            }

            m_Light.range = 0.0f;

            EventManager.Instance.Notify(GameEvents.Puzzle.EndGame);
        }
    }
}