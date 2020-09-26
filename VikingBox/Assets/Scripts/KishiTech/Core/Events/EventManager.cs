//==============================================================================
// Copyright (c) 2017-2020 Kishimoto Studios. All Rights Reserved.
// contact@kishimotostudios.com
//==============================================================================

using System;
using System.Collections.Generic;
#if _KISHITECH_UNITY_DEBUG_LOG_
using UnityEngine;
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_

namespace KishiTech.Core.Events
{
    public class EventManager
    {
        public delegate void SingleCallback();
        public delegate void GenericCallback<T>(T param);
        public delegate void GenericCallback<T1, T2>(T1 param1, T2 param2);

        private static readonly EventManager s_Instance = new EventManager();
        public static EventManager Instance
        {
            get { return s_Instance; }
        }

        private const int INITIAL_SIZE = 10;
        private Dictionary<string, Delegate> m_EventDictionary;

        private EventManager(int initialSize = INITIAL_SIZE)
        {
            m_EventDictionary = new Dictionary<string, Delegate>(initialSize);
        }

        public void Subscribe(string eventName, SingleCallback callback)
        {
#if _KISHITECH_UNITY_DEBUG_LOG_
            Debug.Log($"Subscribe({eventName}, {callback})");
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_

            if (m_EventDictionary.ContainsKey(eventName))
                m_EventDictionary[eventName] = (SingleCallback)m_EventDictionary[eventName] + callback;
            else
                m_EventDictionary.Add(eventName, callback);
        }

        public void Unsubscribe(string eventName, SingleCallback callback)
        {
#if _KISHITECH_UNITY_DEBUG_LOG_
            Debug.Log($"Unsubscribe({eventName}, {callback})");
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_

            if (m_EventDictionary.ContainsKey(eventName))
                m_EventDictionary[eventName] = (SingleCallback)m_EventDictionary[eventName] - callback;
        }

        public void Notify(string eventName)
        {
#if _KISHITECH_UNITY_DEBUG_LOG_
            Debug.Log($"Notify({eventName})");
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_

            if (m_EventDictionary.ContainsKey(eventName) && m_EventDictionary[eventName] != null)
                ((SingleCallback)m_EventDictionary[eventName])();
        }

        public void Subscribe<T>(string eventName, GenericCallback<T> callback)
        {
#if _KISHITECH_UNITY_DEBUG_LOG_
            Debug.Log($"Subscribe<T:{typeof(T)}>({eventName}, {callback})");
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_

            if (m_EventDictionary.ContainsKey(eventName))
                m_EventDictionary[eventName] = (GenericCallback<T>)m_EventDictionary[eventName] + callback;
            else
                m_EventDictionary.Add(eventName, callback);
        }

        public void Unsubscribe<T>(string eventName, GenericCallback<T> callback)
        {
#if _KISHITECH_UNITY_DEBUG_LOG_
            Debug.Log($"Unsubscribe<T:{typeof(T)}>({eventName}, {callback})");
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_

            if (m_EventDictionary.ContainsKey(eventName))
                m_EventDictionary[eventName] = (GenericCallback<T>)m_EventDictionary[eventName] - callback;
        }

        public void Notify<T>(string eventName, T param)
        {
#if _KISHITECH_UNITY_DEBUG_LOG_
            Debug.Log($"Notify<T:{typeof(T)}>({eventName}, {param})");
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_

            if (m_EventDictionary.ContainsKey(eventName) && m_EventDictionary[eventName] != null)
                ((GenericCallback<T>)m_EventDictionary[eventName])(param);
        }

        public void Subscribe<T1, T2>(string eventName, GenericCallback<T1, T2> callback)
        {
#if _KISHITECH_UNITY_DEBUG_LOG_
            Debug.Log($"Subscribe<T1:{typeof(T1)}, T2:{typeof(T2)}>({eventName}, {callback})");
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_

            if (m_EventDictionary.ContainsKey(eventName))
                m_EventDictionary[eventName] = (GenericCallback<T1, T2>)m_EventDictionary[eventName] + callback;
            else
                m_EventDictionary.Add(eventName, callback);
        }

        public void Unsubscribe<T1, T2>(string eventName, GenericCallback<T1, T2> callback)
        {
#if _KISHITECH_UNITY_DEBUG_LOG_
            Debug.Log($"Unsubscribe<T1:{typeof(T1)}, T2:{typeof(T2)}>({eventName}, {callback})");
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_

            if (m_EventDictionary.ContainsKey(eventName))
                m_EventDictionary[eventName] = (GenericCallback<T1, T2>)m_EventDictionary[eventName] - callback;
        }

        public void Notify<T1, T2>(string eventName, T1 param1, T2 param2)
        {
#if _KISHITECH_UNITY_DEBUG_LOG_
            Debug.Log($"Notify<T1:{typeof(T1)}, T2:{typeof(T2)}>({eventName}, {param1}, {param2})");
#endif // #if _KISHITECH_UNITY_DEBUG_LOG_

            if (m_EventDictionary.ContainsKey(eventName) && m_EventDictionary[eventName] != null)
                ((GenericCallback<T1, T2>)m_EventDictionary[eventName])(param1, param2);
        }
    }
}