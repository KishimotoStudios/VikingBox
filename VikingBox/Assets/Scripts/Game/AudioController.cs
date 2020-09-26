//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField]
        string[] m_AudioName;

        [SerializeField]
        AudioClip[] m_AudioClip;

        Dictionary<string, AudioClip> m_Dictionary;

        AudioSource m_AudioSource;

        void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();

            int count = m_AudioName.Length;
            m_Dictionary = new Dictionary<string, AudioClip>(count);
            for (int i = 0; i < count; ++i)
            {
                m_Dictionary.Add(m_AudioName[i], m_AudioClip[i]);
            }
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe<string>(GameEvents.Audio.Play, OnPlayAudio);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe<string>(GameEvents.Audio.Play, OnPlayAudio);
        }

        void OnPlayAudio(string audioName)
        {
            if (m_Dictionary.ContainsKey(audioName))
            {
                m_AudioSource.PlayOneShot(m_Dictionary[audioName]);
            }
        }
    }
}