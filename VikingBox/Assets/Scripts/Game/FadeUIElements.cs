//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using System.Collections;
using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    // Copy-pasted+modified from KishiTech.Core.Fader.ScreenFader...
    public class FadeUIElements : MonoBehaviour
    {
        [SerializeField]
        float m_FadeDuration = 1.0f;
        [SerializeField]
        float m_FadeDelay = 0.0f;
        [SerializeField]
        UnityEngine.UI.Graphic[] m_FadeUIElements;

        Color[] m_FadeUIElementsColor;

        bool m_IsFading;

        void Start()
        {
            ResetInternal();
            m_FadeUIElements = transform.GetComponentsInChildren<UnityEngine.UI.Graphic>();
            m_FadeUIElementsColor = new Color[m_FadeUIElements.Length];
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe(GameEvents.Fade.FadeInMenuStarted, OnFadeInMenu);
            EventManager.Instance.Subscribe(GameEvents.Fade.FadeOutMenuStarted, OnFadeOutMenu);
            EventManager.Instance.Subscribe(GameEvents.Fade.FadeInGameStarted, OnFadeInGame);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(GameEvents.Fade.FadeInMenuStarted, OnFadeInMenu);
            EventManager.Instance.Unsubscribe(GameEvents.Fade.FadeOutMenuStarted, OnFadeOutMenu);
            EventManager.Instance.Unsubscribe(GameEvents.Fade.FadeInGameStarted, OnFadeInGame);
        }

        void OnFadeInMenu()
        {
            StartFadeOut();
        }

        void OnFadeOutMenu()
        {
            StartFadeIn();
        }

        void OnFadeInGame()
        {
            //for (int i = 0; i < m_FadeUIElements.Length; ++i)
            //{
            //    m_FadeUIElements[i].gameObject.SetActive(false);
            //}
            gameObject.SetActive(false);
        }

        public void StartFadeIn()
        {
            if (!m_IsFading)
            {
                StartCoroutine(FadeIn(m_FadeDuration, m_FadeDelay));
            }
        }

        public void StartFadeOut()
        {
            if (!m_IsFading)
            {
                StartCoroutine(FadeOut(m_FadeDuration, m_FadeDelay));
            }
        }

        void ResetInternal()
        {
            m_IsFading = false;
        }

        IEnumerator FadeIn(float duration, float delay)
        {
            m_IsFading = true;

            yield return new WaitForSeconds(delay);

            for (int i = 0; i < m_FadeUIElements.Length; ++i)
            {
                m_FadeUIElementsColor[i] = m_FadeUIElements[i].color;
                m_FadeUIElements[i].gameObject.SetActive(true);
            }

            float time = 0.0f;

            while (time <= duration)
            {
                for (int i = 0; i < m_FadeUIElements.Length; ++i)
                {
                    m_FadeUIElementsColor[i].a = 1.0f - (time / duration);
                    m_FadeUIElements[i].color = m_FadeUIElementsColor[i];
                }

                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            for (int i = 0; i < m_FadeUIElements.Length; ++i)
            {
                m_FadeUIElementsColor[i].a = 0.0f;
                m_FadeUIElements[i].color = m_FadeUIElementsColor[i];
            }

            ResetInternal();
        }

        IEnumerator FadeOut(float duration, float delay)
        {
            m_IsFading = true;

            yield return new WaitForSeconds(delay);

            for (int i = 0; i < m_FadeUIElements.Length; ++i)
            {
                m_FadeUIElementsColor[i] = m_FadeUIElements[i].color;
                m_FadeUIElements[i].gameObject.SetActive(true);
            }

            float time = 0.0f;

            while (time <= duration)
            {
                for (int i = 0; i < m_FadeUIElements.Length; ++i)
                {
                    m_FadeUIElementsColor[i].a = (time / duration);
                    m_FadeUIElements[i].color = m_FadeUIElementsColor[i];
                }

                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            for (int i = 0; i < m_FadeUIElements.Length; ++i)
            {
                m_FadeUIElementsColor[i].a = 1.0f;
                m_FadeUIElements[i].color = m_FadeUIElementsColor[i];
            }

            ResetInternal();
        }
    }
}