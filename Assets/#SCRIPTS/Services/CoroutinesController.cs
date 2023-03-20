using System.Collections;
using UnityEngine;

namespace Game.Services
{
    public sealed class CoroutinesController : MonoBehaviour
    {
        private static CoroutinesController m_instance;
        private static CoroutinesController instance
        {
            get
            {
                if(m_instance == null)
                {
                    var go = new GameObject("[COROUTINE CONTROLLER]");
                    m_instance = go.AddComponent<CoroutinesController>();
                    DontDestroyOnLoad(go);
                }

                return m_instance;
            }
        }

        public static Coroutine StartRoutine(IEnumerator enumerator)
        {
            return instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine routine)
        {
            instance.StopCoroutine(routine);
        }
    }
    
}
