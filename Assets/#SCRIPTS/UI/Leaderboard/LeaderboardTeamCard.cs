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

        public void UpdateInfo(TeamData team, int place)
        {
            _teamNameText.text = team.Name;
            _balanceText.text = "Баланс: " + NumberConverter.NumToString(team.Tickets);
            _membersText.text = "Участники: " + NumberConverter.NumToString(team.MembersCount);
            _placeText.text = "Место: " + NumberConverter.NumToString(place);
        }
    }
}
