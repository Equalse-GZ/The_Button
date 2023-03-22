using System.Collections;
using UnityEngine;

namespace Game.Animations
{
    public class SinglePulseAnimation : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _sizeAnimation;
        [SerializeField] private float _duration;

        private Vector3 _startObjectSize;

        private void Awake()
        {
            _startObjectSize = this.gameObject.transform.localScale;
        }

        public void Play()
        {
            Stop();
            StartCoroutine(StartAnimationRoutine());
        }

        public void Stop()
        {
            this.gameObject.transform.localScale = _startObjectSize;
            StopCoroutine(StartAnimationRoutine());
        }

        private IEnumerator StartAnimationRoutine()
        {
            float expiredSeconds = 0;
            float progress = 0;

            while(progress < 1)
            {
                expiredSeconds += Time.deltaTime;
                progress = expiredSeconds / _duration;

                this.gameObject.transform.localScale = Vector3.one * _sizeAnimation.Evaluate(progress);

                yield return null;
            }
        }
    }
}
