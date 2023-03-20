using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Services
{
    public class GlobalTimer : MonoBehaviour
    {
        public UnityEvent SecondPassedEvent = new UnityEvent();
        public int SecondsPassed = 0;

        private Coroutine _coroutine;

        public void Initialize()
        {
            _coroutine = StartCoroutine(StartTimer());
        }

        public void Disable()
        {
            StopCoroutine(_coroutine);
            SecondPassedEvent.RemoveAllListeners();
        }

        private IEnumerator StartTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                SecondsPassed++;
                SecondPassedEvent.Invoke();
            }
        }

        private void OnDisable()
        {
            StopCoroutine(StartTimer());
            SecondPassedEvent.RemoveAllListeners();
        }
    }
}
