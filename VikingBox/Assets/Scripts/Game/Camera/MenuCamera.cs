//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using System.Collections;
using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class MenuCamera : MonoBehaviour
    {
        [SerializeField]
        float m_RotationSpeed = 30.0f;
        [SerializeField]
        float m_StartDelaySeconds = 1.0f;
        [SerializeField]
        float m_RotationMultiplier = 20.0f;

        bool m_CanAcceptInput;

        void Awake()
        {
            m_CanAcceptInput = false;
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe(GameEvents.Fade.FadeInMenuFinished, OnFadeInMenuFinished);
            EventManager.Instance.Subscribe(GameEvents.Fade.FadeOutMenuFinished, OnFadeOutMenuFinished);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(GameEvents.Fade.FadeInMenuFinished, OnFadeInMenuFinished);
            EventManager.Instance.Unsubscribe(GameEvents.Fade.FadeOutMenuFinished, OnFadeOutMenuFinished);
        }

        void Start()
        {
            EventManager.Instance.Notify(GameEvents.Fade.FadeInMenuStarted);
        }

        void Update()
        {
            transform.RotateAround(Vector3.zero, Vector3.up, -m_RotationSpeed * Time.deltaTime);

            if (m_CanAcceptInput
                && Input.GetMouseButtonDown(0)
                && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()
                )
            {                
                StartCoroutine(StartGame());
            }
        }

        IEnumerator StartGame()
        {
            EventManager.Instance.Notify(GameEvents.Audio.Play, "Valhalla");
            
            m_CanAcceptInput = false;

            m_RotationSpeed *= m_RotationMultiplier;

            yield return new WaitForSeconds(m_StartDelaySeconds);

            EventManager.Instance.Notify(GameEvents.Fade.FadeOutMenuStarted);
        }

        void OnFadeInMenuFinished()
        {
            m_CanAcceptInput = true;
        }

        void OnFadeOutMenuFinished()
        {
            StartCoroutine(StartFadeInGame());
        }

        IEnumerator StartFadeInGame()
        {
            yield return new WaitForSeconds(m_StartDelaySeconds);

            EventManager.Instance.Notify(GameEvents.Fade.FadeInGameStarted);

            gameObject.SetActive(false);
        }
    }
}