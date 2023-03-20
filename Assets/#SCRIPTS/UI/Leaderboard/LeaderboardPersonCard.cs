using Game.Core;
using Game.Data;
using Game.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class LeaderboardPersonCard : LeaderboardCard
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _nickNameText;
        [SerializeField] private Text _balanceText;
        [SerializeField] private Text _placeText;

        public void UpdateInfo(UserData user, int place)
        {
            _icon.sprite = GameManager.AvatarsController.GetAvatar(user.Icon).Icon;
            _nickNameText.text = user.Login;
            _balanceText.text = "������: " + NumberConverter.NumToString(user.PlayerData.Tickets);
            _placeText.text = "�����: " + NumberConverter.NumToString(place);
        }
    }
}
