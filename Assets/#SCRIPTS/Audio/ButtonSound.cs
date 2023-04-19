using UnityEngine;

namespace Game.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class ButtonSound : MonoBehaviour
    {
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private AudioClip _releasedSound;

        [Space(10)]
        [SerializeField] private float _clickSoundVolume = 0.45f;
        [SerializeField] private float _releasedSoundVolume = 0.025f;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayClickSound()
        {
            _audioSource.clip = _clickSound;
            _audioSource.pitch = Random.Range(0.95f, 1.1f);
            _audioSource.volume = _clickSoundVolume;
            _audioSource.Play();
        }

        public void PlayReleasedSound()
        {
            _audioSource.clip = _releasedSound;
            _audioSource.pitch = Random.Range(0.95f, 1.1f);
            _audioSource.volume = _releasedSoundVolume;
            _audioSource.Play();
        }
    }
}
