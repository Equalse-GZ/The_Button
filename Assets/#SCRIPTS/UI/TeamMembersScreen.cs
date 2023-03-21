using Game.Core;
using Game.Data;
using Game.Web;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TeamMembersScreen : MonoBehaviour
    {
        [SerializeField] private TeamMemberCard _cardTemplate;
        [SerializeField] private Transform _cardsContent;
        [SerializeField] private GameObject _loadingObject;

        private List<TeamMemberCard> _memberCards = new List<TeamMemberCard>();

        private TeamData _team;
        private UserData _user;

        public void UpdateTeam(UserData user, TeamData team)
        {
            _user = user;
            _team = team;
        }

        public void LoadMembers()
        {
            _loadingObject.SetActive(true);
            ClearMembersContent();

            WWWForm form = new WWWForm();
            form.AddField("Type", "TeamMembers");
            form.AddField("TeamName", _team.Name);

            GameManager.WebRequestSender.GetUsersData<UserData>(GameManager.Config.DataBaseUrl, form, CreateCards);
        }

        public void CreateCards(UsersData data, WebOperationStatus status)
        {
            _loadingObject.SetActive(false);
            foreach (var user in data.Users)
            {
                TeamMemberCard card = Instantiate(_cardTemplate, _cardsContent);
                card.UpdateInfo(user, _team);

                if (_user.PlayerData.TeamData.Role != "Admin")
                    card.HideKickButton();

                _memberCards.Add(card);
            }
        }

        private void ClearMembersContent()
        {
            foreach (TeamMemberCard card in _memberCards)
            {
                if (card == null)
                    continue;

                card.Disable();
                Destroy(card.gameObject);
            }

            _memberCards.Clear();
        }
    }
}
