using Game.Core;
using Game.Data;
using Game.Web;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class CreateTeamScreen : MonoBehaviour
    {
        [SerializeField] private InputField _teamNameInput;
        [SerializeField] private Text _errorMessage;
        private UserData _userData;

        public void Initialize(UserData userData)
        {
            _userData = userData;
            _errorMessage.text = "";
            _teamNameInput.onValueChanged.AddListener((s) => _teamNameInput.text = s.Replace(" ", ""));
        }

        public void CreateTeam()
        {
            if(_teamNameInput.text.Length < 5)
            {
                _errorMessage.text = "Название команды должно состоять минимум из 5 символов";
                return;
            }

            if(_teamNameInput.text.Length > 12)
            {
                _errorMessage.text = "Название команды должно состоять не более чем из 12 символов";
                return;
            }

            TeamData teamData = new TeamData();
            teamData.Name = _teamNameInput.text;

            WWWForm form = new WWWForm();
            form.AddField("Type", "TeamCreate");
            form.AddField("TeamName", teamData.Name);
            form.AddField("UserID", _userData.ID);
            form.AddField("Role", "Admin");

            GameManager.WebRequestSender.SendData<TeamData>(GameManager.Config.DataBaseUrl, form, OnDataRecieved);
            _errorMessage.text = "";
        }

        private void OnDataRecieved(TeamData data, WebOperationStatus status)
        {
            if (status == WebOperationStatus.Failed)
                throw new Exception("Getting data failed");

            if (data.ErrorData.IsEmpty == false)
            {
                _errorMessage.text = data.ErrorData.Message;
                return;
            }

            GameManager.UserInterface.GetScreen<ProfileScreen>().OnTeamConnected(data);
        }
    }
}
