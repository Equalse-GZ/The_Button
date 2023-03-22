using Game.Core;
using Game.Data;
using Game.Services;
using Game.Web;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class LeaderBoardScreen : ScreenUI
    {
        [SerializeField] private Text _titleText;
        [SerializeField] private LeaderboardPersonCard _personCardTemplate;
        [SerializeField] private LeaderboardTeamCard _teamCardTemplate;
        [SerializeField] private Transform _content;
        [SerializeField] private GameObject _loadingObject;

        [Header("Place Window")]
        [SerializeField] GameObject _placeWindowGameObject;
        [SerializeField] private Text _placeInfo;

        private List<LeaderboardCard> _cards = new List<LeaderboardCard>();

        private int _playerPlace;

        public override void Initialize()
        {
            base.Initialize();
        }

        public void LoadPersons()
        {
            _loadingObject.SetActive(true);
            _titleText.text = "œÂÒÓÌ‡Î¸Ì˚È ÀË‰Â·Ó‰";

            WWWForm form = new WWWForm();
            form.AddField("Type", "LeadersPersonPlace");
            form.AddField("UserName", GameManager.UserData.Login);
            GameManager.WebRequestSender.SendData<UserData>(GameManager.Config.DataBaseUrl, form, (data, status) =>
            {
                _placeInfo.text = $"¬¿ÿ≈ Ã≈—“Œ: {NumberConverter.NumToString(data.Place)}\n ¡¿À¿Õ—: {NumberConverter.NumToString(GameManager.TicketsBankController.Tickets)}";

                WWWForm form = new WWWForm();
                form.AddField("Type", "LeadersPerson");
                GameManager.WebRequestSender.GetUsersData<UserData>(GameManager.Config.DataBaseUrl, form, CreatePersonCards);
            });
        }

        public void LoadTeams()
        {
            _loadingObject.SetActive(true);
            _titleText.text = " ÓÏ‡Ì‰Ì˚È ÀË‰Â·Ó‰";

            if (GameManager.UserData.PlayerData.TeamData.Name == "")
            {
                _placeInfo.text = "¬€ Õ≈ —Œ—“Œ»“≈ ¬  ŒÃ¿Õƒ≈";

                WWWForm form = new WWWForm();
                form.AddField("Type", "LeadersTeam");
                GameManager.WebRequestSender.GetTeamsData<TeamData>(GameManager.Config.DataBaseUrl, form, CreateTeamCards);
            }

            else
            {
                WWWForm form = new WWWForm();
                form.AddField("Type", "LeadersTeamPlace");
                form.AddField("TeamName", GameManager.UserData.PlayerData.TeamData.Name);
                GameManager.WebRequestSender.SendData<TeamData>(GameManager.Config.DataBaseUrl, form, (data, status) =>
                {
                    _placeInfo.text = $"¬¿ÿ≈  ŒÃ¿ÕƒÕŒ≈ Ã≈—“Œ: {NumberConverter.NumToString(data.Place)}\n ¡¿À¿Õ—  ŒÃ¿Õƒ€: {NumberConverter.NumToString(GameManager.UserData.PlayerData.TeamData.Tickets)}";

                    WWWForm form = new WWWForm();
                    form.AddField("Type", "LeadersTeam");
                    GameManager.WebRequestSender.GetTeamsData<TeamData>(GameManager.Config.DataBaseUrl, form, CreateTeamCards);
                });
            }
        }

        public override void Hide()
        {
            base.Hide();
            ClearCards();
            _placeWindowGameObject.SetActive(false);
        }

        private void CreatePersonCards(UsersData data, WebOperationStatus status)
        {
            _loadingObject.SetActive(false);
            int i = 1;
            foreach (var user in data.Users)
            {
                var card = Instantiate(_personCardTemplate, _content);
                card.UpdateInfo(user, i);
                _cards.Add(card);
                i++;
            }

            _placeWindowGameObject.SetActive(true);
        }

        private void CreateTeamCards(TeamsData data, WebOperationStatus status)
        {
            _loadingObject.SetActive(false);
            int i = 1;
            foreach (var team in data.Teams)
            {
                var card = Instantiate(_teamCardTemplate, _content);
                card.UpdateInfo(team, i);
                _cards.Add(card);
                i++;
            }

            _placeWindowGameObject.SetActive(true);
        }

        private void ClearCards()
        {
            foreach (LeaderboardCard card in _cards)
                Destroy(card.gameObject);

            _cards.Clear();
        }
    }
}
