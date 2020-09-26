//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using System.Collections;
using UnityEngine;
using KishiTech.Core.Events;
using KishiTech.Core.Fader;

namespace VikingBox
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        float m_ResetInputDelaySeconds = 1.0f;
        //[SerializeField]
        //Color m_ResetColor = Color.white;

        ScreenFader m_ScreenFader;

        bool m_CanResetGame;

        void Awake()
        {
            m_ScreenFader = GetComponent<ScreenFader>();

            m_CanResetGame = false;
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe(GameEvents.Fade.FadeInMenuStarted, OnFadeInMenu);
            EventManager.Instance.Subscribe(GameEvents.Fade.FadeOutMenuStarted, OnFadeOutMenu);
            EventManager.Instance.Subscribe(GameEvents.Fade.FadeInGameStarted, OnFadeInGame);
            EventManager.Instance.Subscribe(GameEvents.Puzzle.EndGame, OnEndGame);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(GameEvents.Fade.FadeInMenuStarted, OnFadeInMenu);
            EventManager.Instance.Unsubscribe(GameEvents.Fade.FadeOutMenuStarted, OnFadeOutMenu);
            EventManager.Instance.Unsubscribe(GameEvents.Fade.FadeInGameStarted, OnFadeInGame);
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.EndGame, OnEndGame);
        }

        void OnFadeInMenu()
        {
            m_ScreenFader.StartFade(ScreenFader.FadeType.FadeIn, OnFadeInMenuFinished, true);
        }

        void OnFadeInMenuFinished()
        {
            EventManager.Instance.Notify(GameEvents.Fade.FadeInMenuFinished);
        }

        void OnFadeOutMenu()
        {
            m_ScreenFader.StartFade(ScreenFader.FadeType.FadeOut, OnFadeOutMenuFinished, false);
        }

        void OnFadeOutMenuFinished()
        {
            EventManager.Instance.Notify(GameEvents.Fade.FadeOutMenuFinished);
        }

        void OnFadeInGame()
        {
            m_ScreenFader.StartFade(ScreenFader.FadeType.FadeIn, OnFadeInGameFinished, true);
        }

        void OnFadeInGameFinished()
        {
            EventManager.Instance.Notify(GameEvents.Fade.FadeInGameFinished);
        }

        void OnEndGame()
        {
            StartCoroutine(WaitForInput(m_ResetInputDelaySeconds));
        }

        IEnumerator WaitForInput(float delay)
        {
            yield return new WaitForSeconds(delay);

            m_CanResetGame = true;
        }

        void OnFadeOutResetGame()
        {
            StartCoroutine(ResetGame(m_ResetInputDelaySeconds));
        }

        IEnumerator ResetGame(float delay)
        {
            yield return new WaitForSeconds(delay);

            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        void Update()
        {
            if (m_CanResetGame && Input.GetMouseButtonDown(0))
            {
                m_CanResetGame = false;

                //m_ScreenFader.FadeOutColor = m_ResetColor;
                //m_ScreenFader.StartFade(ScreenFader.FadeType.FadeOut, OnFadeOutResetGame);

                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
    }
}