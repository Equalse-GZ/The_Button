using Game.Core;
using Game.Data;
using Game.Services;
using Game.Web;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TeamActiveBlock : MonoBehaviour
    {
        //[SerializeField] private Text _teamInfo;

        [SerializeField] private Text _teamNameText;
        [SerializeField] private Text _teamBalanceText;
        [SerializeField] private Text _inviteCodeText;

        [SerializeField] private Button _leaveButton;

        [Header("Form")]
        [SerializeField] private InputField _sendTicketsInputField;
        [SerializeField] private InputField _getTicketsInputField;
        [SerializeField] private Text _errorMessage;

        [Space(10)]
        [SerializeField] private TeamMembersScreen _teamMembersScreen;

        private TeamData _teamData;
        private UserData _userData;

        public void Initialize(UserData user)
        {
            _leaveButton.onClick.AddListener(() => GameManager.UserInterface.GetScreen<ProfileScreen>().LeaveFromTeam(user, _teamData));
            _errorMessage.text = "";
            _userData = user;
        }

        public void AddTicketsToTeam()
        {
            _errorMessage.text = "";
            if (GameManager.TicketsBankController.IsEnoughTickets(int.Parse(_sendTicketsInputField.text)) == false)
            {
                _errorMessage.text = "Íåäîñòàòî÷íî ñðåäñòâ!";
                return;
            }


            WWWForm form = new WWWForm();
            form.AddField("Type", "TeamAddTickets");
            form.AddField("Tickets", int.Parse(_sendTicketsInputField.text));
            form.AddField("TeamName", _teamData.Name);

            GameManager.WebRequestSender.SendData<TeamData>(GameManager.Config.DataBaseUrl, form, SpendTickets);
        }

        public void GetTicketsFromTeam()
        {
            _errorMessage.text = "";

            WWWForm form = new WWWForm();
            form.AddField("Type", "TeamGetTickets");
            form.AddField("Tickets", int.Parse(_getTicketsInputField.text));
            form.AddField("TeamName", _teamData.Name);

            GameManager.WebRequestSender.SendData<TeamData>(GameManager.Config.DataBaseUrl, form, AddTickets);
        }

        public void UpdateTeamInfo(TeamData data)
        {
            _teamNameText.text = data.Name.ToUpper();
            _teamBalanceText.text = "Баланс команды: " + NumberConverter.NumToString(data.Tickets);
            _inviteCodeText.text = "Код приглашения: " + data.InviteCode;
            _teamData = data;
            _teamMembersScreen.UpdateTeam(_userData, data);
        }

        public void Disable()
        {
            _leaveButton.onClick.RemoveAllListeners();
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        private void AddTickets(TeamData data, WebOperationStatus status)
        {
            GameManager.TicketsBankController.AddTickets(int.Parse(_getTicketsInputField.text), this);
            _getTicketsInputField.text = "";
            if (status == WebOperationStatus.Failed)
                throw new Exception("Getting data failed");

            if (data.ErrorData.IsEmpty == false)
            {
                _errorMessage.text = data.ErrorData.Message;
                return;
            }

            UpdateTeamInfo(data);
        }

        private void SpendTickets(TeamData data, WebOperationStatus status)
        {
            GameManager.TicketsBankController.SpendTickets(int.Parse(_sendTicketsInputField.text), this);
            _sendTicketsInputField.text = "";
            if (status == WebOperationStatus.Failed)
                throw new Exception("Getting data failed");

            if (data.ErrorData.IsEmpty == false)
            {
                _errorMessage.text = data.ErrorData.Message;
                return;
            }

            UpdateTeamInfo(data);
        }
    }
}
