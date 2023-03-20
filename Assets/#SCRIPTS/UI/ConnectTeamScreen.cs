using Game.Core;
using Game.Data;
using Game.Web;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ConnectTeamScreen : MonoBehaviour
    {
        [SerializeField] private InputField _inviteCodeInputField;
        [SerializeField] private Text _errorMessage;
        
        private UserData _userData;

        public void Initialize(UserData userData)
        {
            _userData = userData;
            _errorMessage.text = "";
        }

        public void ConnectToTeam()
        {
            WWWForm form = new WWWForm();
            form.AddField("Type", "TeamConnect");
            form.AddField("InviteCode", _inviteCodeInputField.text);
            form.AddField("UserID", _userData.ID);
            form.AddField("Role", "Member");

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
