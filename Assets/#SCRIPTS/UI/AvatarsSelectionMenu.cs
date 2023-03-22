using Game.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(SinglePulseAnimation))]
    public class AvatarsSelectionMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _unselectedAvatarObject;
        [SerializeField] private Image _icon;

        private Game.Avatars.Avatar _selectedAvatar;

        public void Initialize()
        {
            _icon.gameObject.SetActive(_selectedAvatar != null);
            _unselectedAvatarObject.SetActive(_selectedAvatar == null);

            _icon.sprite = _selectedAvatar?.Icon;
        }

        public void SelectAvatar(Game.Avatars.Avatar avatar)
        {
            _selectedAvatar = avatar;
            _icon.sprite = avatar.Icon;
            _icon.gameObject.SetActive(true);
            _unselectedAvatarObject.SetActive(false);
        }

        public void ResetSelectedAvatar()
        {
            _selectedAvatar = null;
            _icon.gameObject.SetActive(false);
            _unselectedAvatarObject.SetActive(true);
        }

        public void PlayPulseAnimation() => GetComponent<SinglePulseAnimation>().Play();

        public Game.Avatars.Avatar GetSelectedAvatar() => _selectedAvatar;
        public bool AvatarIsSelected() => _selectedAvatar != null;
    }
}
