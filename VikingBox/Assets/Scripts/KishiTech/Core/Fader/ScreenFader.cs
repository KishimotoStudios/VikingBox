//==============================================================================
// Copyright (c) 2017-2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KishiTech.Core.Fader
{
    // How to use this component:
    // 1. Add a GameObject of type UIImage (menu: GameObject > UI > Image) in your scene.
    // 2. Configure the UIImage by setting:
    //    - Anchors Min(0, 0) and Max(1, 1).
    //    - Pivot (0, 0).
    //    - Left = 0, Top = 0, Right = 0, Bottom = 0.
    // 3. Link the UIImage to the m_FadeImage below (via Editor).
    // 4. Hopefully, all other attributes exposed in the editor are self-explanatory.

    public class ScreenFader : MonoBehaviour
    {
        public enum FadeType
        {
            FadeIn,
            FadeOut,
            FadeInAfterSceneLoaded,
            Unused
        };

        [SerializeField]
        private bool m_FadeInAfterSceneLoaded = true;
        [SerializeField]
        private bool m_SkippableFadeInAfterSceneLoaded = false;
        [SerializeField]
        private string m_NextScene = "__!EDIT_THIS_VALUE!__";
        [SerializeField]
        private bool m_SkippableWithAnyKey = false;
        [SerializeField]
        private float m_FadeDuration = 1.0f;
        [SerializeField]
        private float m_FadeDelay = 0.0f;
        [SerializeField]
        private UnityEngine.UI.Image m_FadeImage = null;
        [SerializeField]
        private Color m_FadeInColor = Color.black;
        public Color FadeInColor
        {
            get { return m_FadeInColor; }
            set { m_FadeInColor = value; }
        }

        [SerializeField]
        private Color m_FadeOutColor = Color.black;
        public Color FadeOutColor
        {
            get { return m_FadeOutColor; }
            set { m_FadeOutColor = value; }
        }

        private bool m_IsFading;
        public bool IsFading
        {
            get { return m_IsFading; }
        }

        private FadeType m_FadeType;
        private System.Action m_OnFadeEndCallback;
        private Coroutine m_FadeCoroutine;

        public void FadeInToNextScene()
        {
            StartFade(FadeType.FadeIn, LoadNextScene);
        }

        public void FadeOutToNextScene()
        {
            StartFade(FadeType.FadeOut, LoadNextScene);
        }

        public void StartFade(FadeType fadeType, System.Action onFadeEndCallback, bool hideFaderAfterFinished = false)
        {
            if (m_IsFading)
            {
#if _KISHITECH_UNITY_DEBUG_LOG_
                Debug.LogWarning("ScreenFader is already fading, ignoring request...");
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_

                return;
            }

            if (fadeType == FadeType.FadeIn || fadeType == FadeType.FadeInAfterSceneLoaded)
                m_FadeCoroutine = StartCoroutine(FadeIn(fadeType, m_FadeDuration, m_FadeDelay, onFadeEndCallback, hideFaderAfterFinished));
            else
                m_FadeCoroutine = StartCoroutine(FadeOut(m_FadeDuration, m_FadeDelay, onFadeEndCallback, hideFaderAfterFinished));
        }

        void Awake()
        {
            ResetInternal();
        }

        private void ResetInternal()
        {
            m_IsFading = false;
            m_FadeType = FadeType.Unused;
            m_OnFadeEndCallback = null;
            m_FadeCoroutine = null;
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (m_FadeInAfterSceneLoaded)
            {
                m_FadeImage.color = m_FadeInColor;
                m_FadeImage.gameObject.SetActive(true);

                StartFade(FadeType.FadeInAfterSceneLoaded, HideFadeRect);
            }
        }

        private void OnSceneUnloaded(Scene scene)
        {
            HideFadeRect();
            ResetInternal();
        }

        private bool CheckSkip()
        {
            if (Input.anyKey && m_IsFading
                && (m_FadeType == FadeType.FadeInAfterSceneLoaded && m_SkippableFadeInAfterSceneLoaded
                || m_FadeType != FadeType.FadeInAfterSceneLoaded && m_SkippableWithAnyKey))
            {
#if _KISHITECH_UNITY_DEBUG_LOG_
                Debug.LogWarning($"Skipping fade {m_FadeType}...");
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_

                if (m_FadeCoroutine != null)
                    StopCoroutine(m_FadeCoroutine);

                m_OnFadeEndCallback?.Invoke();

                ResetInternal();

                return true;
            }

            return false;
        }

        private IEnumerator FadeIn(FadeType fadeType, float duration, float delay, System.Action onFadeInEndedCallback, bool hideFaderAfterFinished)
        {
            m_IsFading = true;
            m_FadeType = fadeType;
            m_OnFadeEndCallback = onFadeInEndedCallback;

#if _KISHITECH_UNITY_DEBUG_LOG_
            Debug.Log($"FadeIn will start after delay of {delay} seconds.");
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_

            yield return new WaitForSeconds(delay);

            m_FadeImage.color = m_FadeInColor;
            m_FadeImage.gameObject.SetActive(true);

            float time = 0.0f;
            Color fadeColor = m_FadeImage.color;

            while (time <= duration)
            {
                if (CheckSkip())
                    yield break;

                fadeColor.a = 1.0f - (time / duration);
                m_FadeImage.color = fadeColor;

                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            fadeColor.a = 0.0f;
            m_FadeImage.color = fadeColor;

            onFadeInEndedCallback?.Invoke();

            ResetInternal();

            if (hideFaderAfterFinished)
                HideFadeRect();

#if _KISHITECH_UNITY_DEBUG_LOG_
            Debug.Log($"FadeIn ended.");
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_
        }

        private IEnumerator FadeOut(float duration, float delay, System.Action onFadeOutEndedCallback, bool hideFaderAfterFinished)
        {
            m_IsFading = true;
            m_FadeType = FadeType.FadeOut;
            m_OnFadeEndCallback = onFadeOutEndedCallback;

#if _KISHITECH_UNITY_DEBUG_LOG_
            Debug.Log($"FadeOut will start after delay of {delay} seconds.");
#endif /// #if _KISHITECH_UNITY_DEBUG_LOG_

            yield return new WaitForSeconds(delay);

            m_FadeImage.color = m_FadeOutColor;
            m_FadeImage.gameObject.SetActive(true);

            float time = 0.0f;
            Color fadeColor = m_FadeImage.color;

            while (time <= duration)
            {
                if (CheckSkip())
                    yield break;

                fadeColor.a = (time / duration);
                m_FadeImage.color = fadeColor;

                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            fadeColor.a = 1.0f;
            m_FadeImage.color = fadeColor;

            onFadeOutEndedCallback?.Invoke();

            ResetInternal();

            if (hideFaderAfterFinished)
                HideFadeRect();

#if _KISHITECH_UNITY_DEBUG_LOG_
            Debug.Log($"FadeOut ended.");
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_
        }

        private void HideFadeRect()
        {
            m_FadeImage.gameObject.SetActive(false);
        }

        private void LoadNextScene()
        {
#if _KISHITECH_UNITY_DEBUG_LOG_
            Debug.Log($"Async loading scene {m_NextScene}...");
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_

            SceneManager.LoadSceneAsync(m_NextScene);
        }
    }
}