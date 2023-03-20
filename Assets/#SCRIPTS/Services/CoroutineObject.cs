using System;
using System.Collections;

using UnityEngine;

namespace Game.Services
{
    public class CoroutineObject<T> : MonoBehaviour
    {
        public MonoBehaviour Owner { get; private set; }
        public Coroutine Coroutine { get; private set; }
        public Func<T, IEnumerator> Routine { get; private set; }

        public bool IsProcessing => Coroutine != null;

        public CoroutineObject(MonoBehaviour owner, Func<T, IEnumerator> routine)
        {
            Owner = owner;
            Routine = routine;
        }

        private IEnumerator Process(T arg)
        {
            yield return Routine.Invoke(arg);
            Coroutine = null;
        }

        public void Start(T arg)
        {
            Stop();
            Coroutine = Owner.StartCoroutine(Process(arg));
        }

        public void Stop()
        {
            if (IsProcessing)
            {
                Owner.StopCoroutine(Coroutine);
                Coroutine = null;
            }
        }
    }
}
