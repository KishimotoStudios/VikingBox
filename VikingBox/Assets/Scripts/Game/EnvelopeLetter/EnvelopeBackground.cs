//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using UnityEngine;
using KishiTech.Core.Events;

namespace VikingBox
{
    public class EnvelopeBackground : MonoBehaviour
    {
        void OnEnable()
        {
            EventManager.Instance.Subscribe(GameEvents.Puzzle.FirstStarted, OnFirstPuzzleStarted);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.FirstStarted, OnFirstPuzzleStarted);
        }

        void OnFirstPuzzleStarted()
        {
            gameObject.SetActive(false);
        }
    }
}