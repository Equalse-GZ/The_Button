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
        }

        public void CreateTeam()
        {
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
            // Hide Create Team
        }
    }
}
