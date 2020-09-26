//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class Envelope : MonoBehaviour
    {
        [SerializeField]
        bool m_IsFirstEnvelope;
        [SerializeField]
        string m_MaterialIndexName;
        [SerializeField]
        string m_MaterialIndexEnvelopeName;

        Material m_Material;
        Collider m_Collider;

        void Awake()
        {
            m_Material = GetComponent<MeshRenderer>().material;
            m_Material.SetInt(m_MaterialIndexName, 0);
            m_Material.SetInt(m_MaterialIndexEnvelopeName, m_IsFirstEnvelope ? 0 : 1);

            m_Collider = GetComponent<Collider>();
            m_Collider.enabled = m_IsFirstEnvelope;
        }

        void OnDestroy()
        {
            Destroy(m_Material);
        }

        void OnEnable()
        {
            if (m_IsFirstEnvelope)
            {
                EventManager.Instance.Subscribe(GameEvents.Puzzle.FirstStarted, OnFirstPuzzleStarted);
            }
            else
            {
                EventManager.Instance.Subscribe(GameEvents.Puzzle.ThirdUnlocked, OnThirdPuzzleUnlocked);
            }
        }

        void OnDisable()
        {
            if (m_IsFirstEnvelope)
            {
                EventManager.Instance.Unsubscribe(GameEvents.Puzzle.FirstStarted, OnFirstPuzzleStarted);
            }
            else
            {
                EventManager.Instance.Unsubscribe(GameEvents.Puzzle.ThirdUnlocked, OnThirdPuzzleUnlocked);
            }
        }

        void OnMouseUpAsButton()
        {
            if (m_IsFirstEnvelope)
            {
                m_Collider.enabled = false;
                EventManager.Instance.Notify(GameEvents.Letter.FirstLetter);
            }
            else
            {
                gameObject.SetActive(false);
                EventManager.Instance.Notify(GameEvents.Letter.LastLetter);
            }
        }

        void OnFirstPuzzleStarted()
        {
            gameObject.SetActive(false);
        }

        void OnThirdPuzzleUnlocked()
        {
            m_Collider.enabled = true;
        }
    }
}