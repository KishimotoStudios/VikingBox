//==============================================================================
// Copyright (c) 2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using KishiTech.Core.Events;
using UnityEngine;

namespace VikingBox
{
    public class EndMessage : MonoBehaviour
    {
        UnityEngine.UI.Text m_Text;

        void Awake()
        {
            m_Text = GetComponent<UnityEngine.UI.Text>();
            m_Text.enabled = false;
        }

        void OnEnable()
        {
            EventManager.Instance.Subscribe(GameEvents.Puzzle.EndGame, OnEndGame);
        }

        void OnDisable()
        {
            EventManager.Instance.Unsubscribe(GameEvents.Puzzle.EndGame, OnEndGame);
        }

        void OnEndGame()
        {
            m_Text.enabled = true;
        }
    }
}