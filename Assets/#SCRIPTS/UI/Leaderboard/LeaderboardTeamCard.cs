using Game.Data;
using Game.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class LeaderboardTeamCard : LeaderboardCard
    {
        [SerializeField] private Text _teamNameText;
        [SerializeField] private Text _balanceText;
        [SerializeField] private Text _membersText;
        [SerializeField] private Text _placeText;
        [SerializeField] private Image _awardIcon;


        public void UpdateInfo(TeamData team, int place)
        {
            _awardIcon.gameObject.SetActive(place == 1);

            _teamNameText.text = team.Name.ToUpper();
            _balanceText.text = "Баланс: " + NumberConverter.NumToString(team.Tickets) + " б";
            _membersText.text = NumberConverter.NumToString(team.MembersCount);
            _placeText.text = NumberConverter.NumToString(place);
        }
    }
}
