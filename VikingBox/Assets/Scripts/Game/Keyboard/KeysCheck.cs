//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public enum KeyboardMode
    {
        Puzzle,
        Free
    }

    public class KeysCheck : MonoBehaviour
    {
        [SerializeField]
        AudioClip[] m_AudioNotes;

        [SerializeField]
        int[] m_NoteSequenceToCheck;

        int[] m_CurrentNoteSequence;
        int m_CurrentNoteIndex;
        bool m_IsCorrectSequence;

        AudioSource m_AudioSource;

        KeyboardMode m_Mode;

        void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();

            m_CurrentNoteSequence = new int[m_NoteSequenceToCheck.Length];
            ResetCurrentNoteSequence();

            m_Mode = KeyboardMode.Free;
        }

        void ResetCurrentNoteSequence()
        {
            for (int i = 0; i < m_CurrentNoteSequence.Length; ++i)
            {
                m_CurrentNoteSequence[i] = -1;
            }
            m_CurrentNoteIndex = 0;
            m_IsCorrectSequence = false;
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe(GameEvents.Keyboard.ChangeMode, OnChangeMode);
            EventManager.Instance.Subscribe<int>(GameEvents.Keyboard.PlayNote, OnPlayNote);

            if (m_Mode == KeyboardMode.Puzzle)
            {
                SetupPuzzleMode();
            }
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(GameEvents.Keyboard.ChangeMode, OnChangeMode);
            EventManager.Instance.Unsubscribe<int>(GameEvents.Keyboard.PlayNote, OnPlayNote);
            EventManager.Instance.Unsubscribe(GameEvents.Keyboard.CheckAnswer, OnCheckNotes);
        }

        void SetupPuzzleMode()
        {
            m_Mode = KeyboardMode.Puzzle;
            ResetCurrentNoteSequence();
            EventManager.Instance.Unsubscribe(GameEvents.Keyboard.CheckAnswer, OnCheckNotes);
            EventManager.Instance.Subscribe(GameEvents.Keyboard.CheckAnswer, OnCheckNotes);
        }

        void OnChangeMode()
        {
            // TODO: Put a switch model on the keyboard so the player can choose between "unlock" (puzzle mode) and "play" (free mode).
            // For now, I'm only changing the keyboard tab shader color.

            if (m_Mode == KeyboardMode.Free)
            {
                SetupPuzzleMode();
            }
            else
            {
                m_Mode = KeyboardMode.Free;
                EventManager.Instance.Unsubscribe(GameEvents.Keyboard.CheckAnswer, OnCheckNotes);
            }

            EventManager.Instance.Notify(GameEvents.Keyboard.AfterChangeMode, m_Mode);
        }

        void OnPlayNote(int noteIndex)
        {
            if (m_Mode == KeyboardMode.Puzzle)
            {
                if (m_CurrentNoteIndex < m_CurrentNoteSequence.Length)
                {
                    m_CurrentNoteSequence[m_CurrentNoteIndex] = noteIndex;

                    if (m_CurrentNoteSequence[m_CurrentNoteIndex] == m_NoteSequenceToCheck[m_CurrentNoteIndex])
                    {
                        m_IsCorrectSequence = true;

                        m_AudioSource.PlayOneShot(m_AudioNotes[noteIndex]);
                    }
                    else
                    {
                        m_IsCorrectSequence = false;

                        m_AudioSource.PlayOneShot(m_AudioNotes[m_AudioNotes.Length - 1]);
                    }

                    ++m_CurrentNoteIndex;
                }
            }
            else
            {
                m_AudioSource.PlayOneShot(m_AudioNotes[noteIndex]);
            }
        }

        void OnCheckNotes()
        {
            if (m_Mode == KeyboardMode.Puzzle)
            {
                if (m_IsCorrectSequence)
                {
                    if (m_CurrentNoteIndex == m_NoteSequenceToCheck.Length)
                    {
                        EventManager.Instance.Notify(GameEvents.Keyboard.CorrectAnswer);
                        EventManager.Instance.Notify(GameEvents.Keyboard.AfterChangeMode, KeyboardMode.Free);
                    }
                }
                else
                {
                    ResetCurrentNoteSequence();
                }
            }
        }
    }
}