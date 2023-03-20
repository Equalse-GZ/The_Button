using UnityEngine;

namespace Game.UI
{
    public class AvatarsSelectionMenuCard : MonoBehaviour
    {
        [SerializeField] private AvatarsSelectionMenu _menu;
        [SerializeField] private Game.Avatars.Avatar _avatarData;

        public void SelectAvatar()
        {
            _menu.SelectAvatar(_avatarData);
        }
    }
}
