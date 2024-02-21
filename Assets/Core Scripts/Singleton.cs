using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T>: MonoBehaviour where T : MonoBehaviour
{
    static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindAnyObjectByType<T>();

            }
            if (m_instance == null)
            {
                Debug.LogError("Doesn't have singleton of type " + typeof(T).Name + " in this scene!!");
            }
            return m_instance;
        }
    }
}
