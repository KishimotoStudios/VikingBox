//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using System.Collections;
using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    // So many things were written in a rush, just to get things working... Goodbye, maintenance and I'm sorry, future me.
    public class Letter : MonoBehaviour
    {
        [SerializeField]
        float m_FirstLastInputDelaySeconds = 1.0f;

        [SerializeField]
        float m_OpenCloseLerpPercentage = 0.1f; // 0.025f;

        [SerializeField]
        float m_MouseOverZ = 0.1f;

        [SerializeField]
        float[] m_Height;
        
        [SerializeField]
        string m_MaterialProperty;

        Vector3 m_OpenedPosition;
        Vector3 m_ClosedPosition;
        Vector3 m_CurrentPosition;

        float m_OriginalZ;
        int m_CurrentIndex;
        
        enum OpenStatus
        {
            Closed,
            Opening,
            Opened,
            Closing
        };
        OpenStatus m_OpenStatus;

        MeshRenderer m_MeshRenderer;
        Material m_Material;
        Collider m_Collider;

        bool m_CanAcceptInput;

        void Awake()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
            m_Material = m_MeshRenderer.sharedMaterial;//.material;
            m_Collider = GetComponent<Collider>();

            SetColliderAndMesh(false);

            m_OpenedPosition = Vector3.zero;
            m_CurrentPosition = m_ClosedPosition = transform.position;
            m_OriginalZ = transform.position.z;
            SetIndex(0);

            m_CanAcceptInput = !IsFirstOrLastIndex();
        }

        //void OnDestroy()
        //{
        //    Destroy(m_Material);
        //}

        void OnEnable()
        {
            EventManager.Instance.Subscribe(GameEvents.Puzzle.FirstUnlocked, OnPuzzleUnlocked);
            EventManager.Instance.Subscribe(GameEvents.Puzzle.SecondUnlocked, OnPuzzleUnlocked);
            EventManager.Instance.Subscribe(GameEvents.Puzzle.ThirdUnlocked, OnPuzzleUnlocked);
            EventManager.Instance.Subscribe(GameEvents.Camera.ScreenShakeStarted, OnScreenShakeStarted);
            EventManager.Instance.Subscribe(GameEvents.Camera.ScreenShakeFinished, OnScreenShakeFinished);
            EventManager.Instance.Subscribe(GameEvents.Letter.FirstLetter, OnFirstLetter);
            EventManager.Instance.Subscribe(GameEvents.Letter.LastLetter, OnLastLetter);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.FirstUnlocked, OnPuzzleUnlocked);
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.SecondUnlocked, OnPuzzleUnlocked);
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.ThirdUnlocked, OnPuzzleUnlocked);
            EventManager.Instance.Unsubscribe(GameEvents.Camera.ScreenShakeStarted, OnScreenShakeStarted);
            EventManager.Instance.Unsubscribe(GameEvents.Camera.ScreenShakeFinished, OnScreenShakeFinished);
            EventManager.Instance.Unsubscribe(GameEvents.Letter.FirstLetter, OnFirstLetter);
            EventManager.Instance.Unsubscribe(GameEvents.Letter.LastLetter, OnLastLetter);
        }

        void OnMouseEnter()
        {
            if (m_OpenStatus == OpenStatus.Closed)
            {
                m_CurrentPosition = m_ClosedPosition;
                m_CurrentPosition.z -= m_MouseOverZ;
                transform.position = m_CurrentPosition;
            }
        }

        void OnMouseExit()
        {
            if (m_OpenStatus == OpenStatus.Closed)
            {
                ReturnToClosedPosition();
            }
        }

        void OnMouseUpAsButton()
        {
            //Debug.Log($"m_CanAcceptInput: {m_CanAcceptInput}, m_OpenStatus? {m_OpenStatus}");

            if (m_OpenStatus == OpenStatus.Closed)
            {
                StartCoroutine(OpenLetter());
            }
            else if (m_CanAcceptInput && m_OpenStatus == OpenStatus.Opened)
            {
                if (!IsLastIndex())
                {
                    StartCoroutine(CloseLetter());
                }
                else
                {
                    CloseLetterInstantly();
                }
            }
        }

        void ReturnToClosedPosition()
        {
            transform.position = m_ClosedPosition;
        }

        void SetColliderAndMesh(bool isEnabled)
        {
            m_Collider.enabled = isEnabled;
            m_MeshRenderer.enabled = isEnabled;
        }

        void OnScreenShakeStarted()
        {
            SetColliderAndMesh(false);
        }

        void OnScreenShakeFinished()
        {
            SetColliderAndMesh(m_CurrentIndex < m_Height.Length - 1 || m_OpenStatus == OpenStatus.Opened);
        }

        void OnFirstLetter()
        {
            SetColliderAndMesh(true);
            //OpenLetterInstantly();
            StartCoroutine(OpenLetter());
        }

        void OnLastLetter()
        {
            OnPuzzleUnlocked();
            SetColliderAndMesh(true);
            OpenLetterInstantly();
            //StartCoroutine(OpenLetter());
        }

        void OnPuzzleUnlocked()
        {
            //Debug.LogWarning($"OnPuzzleUnlocked: {m_CurrentIndex}");

            SetIndex(m_CurrentIndex + 1);
        }

        void SetIndex(int index)
        {
            if (index < m_Height.Length)
            {
                m_CurrentIndex = index;
                m_OpenedPosition.y = m_Height[m_CurrentIndex];
                m_ClosedPosition.y = m_Height[m_CurrentIndex];
                m_ClosedPosition.z = m_OriginalZ;
                m_CurrentPosition = m_ClosedPosition;
                transform.position = m_ClosedPosition;

                m_OpenStatus = OpenStatus.Closed;

                m_Material.SetInt(m_MaterialProperty, m_CurrentIndex);
            }
            else
            {
                //gameObject.SetActive(false);
                SetColliderAndMesh(false);
            }
        }

        void OpenLetterInstantly()
        {
            EventManager.Instance.Notify(GameEvents.Audio.Play, "SFX_PaperIn");

            EventManager.Instance.Notify(GameEvents.Letter.Open, m_CurrentIndex);

            AfterOpenLetter();
        }

        IEnumerator OpenLetter()
        {
            EventManager.Instance.Notify(GameEvents.Audio.Play, "SFX_PaperIn");

            EventManager.Instance.Notify(GameEvents.Letter.Open, m_CurrentIndex);

            m_OpenStatus = OpenStatus.Opening;

            while (Vector3.Distance(m_CurrentPosition, m_OpenedPosition) > 0.01f)
            {
                m_CurrentPosition = Vector3.Lerp(m_CurrentPosition, m_OpenedPosition, m_OpenCloseLerpPercentage);
                transform.position = m_CurrentPosition;

                yield return new WaitForEndOfFrame();
            }

            AfterOpenLetter();
        }

        void AfterOpenLetter()
        {
            m_CurrentPosition = m_OpenedPosition;
            transform.position = m_OpenedPosition;

            m_OpenStatus = OpenStatus.Opened;

            m_CanAcceptInput = !IsFirstOrLastIndex();
            if (!m_CanAcceptInput)
            {
                StartCoroutine(DelayToAcceptInput(m_FirstLastInputDelaySeconds));
            }
        }

        void CloseLetterInstantly()
        {
            EventManager.Instance.Notify(GameEvents.Audio.Play, "SFX_PaperOut");

            AfterCloseLetter();
        }

        IEnumerator CloseLetter()
        {
            EventManager.Instance.Notify(GameEvents.Audio.Play, "SFX_PaperOut");

            m_OpenStatus = OpenStatus.Closing;

            while (Vector3.Distance(m_CurrentPosition, m_ClosedPosition) > 0.01f)
            {
                m_CurrentPosition = Vector3.Lerp(m_CurrentPosition, m_ClosedPosition, m_OpenCloseLerpPercentage);
                transform.position = m_CurrentPosition;

                yield return new WaitForEndOfFrame();
            }

            AfterCloseLetter();
        }

        void AfterCloseLetter()
        {
            m_CanAcceptInput = !IsFirstOrLastIndex();

            m_CurrentPosition = m_ClosedPosition;
            transform.position = m_ClosedPosition;

            m_OpenStatus = OpenStatus.Closed;

            EventManager.Instance.Notify(GameEvents.Letter.Close, m_CurrentIndex);

            RunIfFirstOrLastLetter();
        }

        // This will do for this particular "jam" game, but I'd probably refactor it.
        void RunIfFirstOrLastLetter()
        {
            if (IsFirstIndex())
            {
                SetColliderAndMesh(false);
                OnPuzzleUnlocked();
                EventManager.Instance.Notify(GameEvents.Puzzle.FirstStarted);                
            }
            else if (IsLastIndex())
            {
                SetColliderAndMesh(false);
                EventManager.Instance.Notify(GameEvents.Puzzle.LastEnvelopeUnlocked);
            }
        }

        IEnumerator DelayToAcceptInput(float delayInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);

            m_CanAcceptInput = true;
        }

        bool IsFirstIndex()
        {
            return m_CurrentIndex == 0;
        }

        bool IsLastIndex()
        {
            return m_CurrentIndex == m_Height.Length - 1;
        }

        bool IsFirstOrLastIndex()
        {
            return m_CurrentIndex == 0 || m_CurrentIndex == m_Height.Length - 1;
        }
    }
}