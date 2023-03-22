using Game.Controllers;
using Game.Core;
using Game.Data;
using Game.Services;
using Game.Web;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ProfileScreen : ScreenUI
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _nickNameText;
        [SerializeField] private Text _ticketsText;

        [SerializeField] private TeamActiveBlock _teamActiveBlock;
        [SerializeField] private TeamInactiveBlock _teamInactiveBlock;

        private UserData _user;

        public override void Initialize()
        {
            base.Initialize();
            GameManager.TicketsBankController.TicketsChangedEvent.AddListener(() => _ticketsText.text = "Баланс: " + NumberConverter.NumToString(GameManager.TicketsBankController.Tickets) + " б");
        }

        public void UpdateInfo(UserData data)
        {
            _icon.sprite = GameManager.AvatarsController.GetAvatar(data.Icon).Icon;
            _nickNameText.text = data.Login.ToUpper();

            _teamInactiveBlock.Initialize(data);
            _teamActiveBlock.Initialize(data);

            if (data.PlayerData.TeamData.Name == "")
            {
                _teamActiveBlock.Hide();
                _teamInactiveBlock.Show();
                return;
            }
            else
            {
                _teamActiveBlock.Show();
                _teamInactiveBlock.Hide();

                _teamActiveBlock.UpdateTeamInfo(data.PlayerData.TeamData);
            }
        }

        public void OnTeamConnected(TeamData data)
        {
            _teamActiveBlock.Show();
            _teamInactiveBlock.Hide();

            _teamActiveBlock.UpdateTeamInfo(data);
            GameManager.UserData.PlayerData.TeamData = data;
        }

        public void LeaveFromTeam(UserData user, TeamData team)
        {
            WWWForm form = new WWWForm();

            form.AddField("Type", "TeamLeave");
            form.AddField("TeamName", team.Name);
            form.AddField("UserID", user.ID);

            GameManager.WebRequestSender.SendData<TeamData>(GameManager.Config.DataBaseUrl, form, OnPlayerLeavedFromTeam);
        }

        private void OnPlayerLeavedFromTeam(TeamData data, WebOperationStatus status)
        {
            _teamActiveBlock.Hide();
            _teamInactiveBlock.Show();

            GameManager.TicketsBankController.AddTickets(data.Tickets, this);
            GameManager.UserData.PlayerData.TeamData.Name = "";
        }

        public void Disable()
        {
            _teamActiveBlock.Disable();
            _teamInactiveBlock.Disable();
        }
    }
}
