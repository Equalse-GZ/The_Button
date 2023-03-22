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
        [SerializeField] private Image _crownIcon;
        [SerializeField] private Image _placeBG;

        [Header("Colors")]
        [SerializeField] private Color _firstPlaceBGColor;
        [SerializeField] private Color _firstPlaceTextColor;

        [Space(5)]
        [SerializeField] private Color _secondPlaceBGColor;
        [SerializeField] private Color _secondPlaceTextColor;

        [Space(5)]
        [SerializeField] private Color _thirdPlaceBGColor;
        [SerializeField] private Color _thirdPlaceTextColor;

        [Space(5)]
        [SerializeField] private Color _otherPlaceBGColor;
        [SerializeField] private Color _otherPlaceTextColor;

        public void UpdateInfo(UserData user, int place)
        {
            _crownIcon.gameObject.SetActive(false);
            _placeText.text = place.ToString();

            if (place == 1)
            {
                ChangePlaceColor(_firstPlaceBGColor, _firstPlaceTextColor);
                _crownIcon.gameObject.SetActive(true);
            }

            else if (place == 2)
                ChangePlaceColor(_secondPlaceBGColor, _secondPlaceTextColor);

            else if (place == 3)
                ChangePlaceColor(_thirdPlaceBGColor, _thirdPlaceTextColor);

            else
                ChangePlaceColor(_otherPlaceBGColor, _otherPlaceTextColor);

            _icon.sprite = GameManager.AvatarsController.GetAvatar(user.Icon).Icon;
            _nickNameText.text = user.Login.ToUpper();
            _balanceText.text = "Баланс: " + NumberConverter.NumToString(user.PlayerData.Tickets) + " б";
        }

        private void ChangePlaceColor(Color bgColor, Color textColor)
        {
            _placeBG.color = bgColor;
            _placeText.color = textColor;
        }
    }
}
