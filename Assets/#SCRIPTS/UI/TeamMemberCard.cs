using Game.Core;
using Game.Data;
using Game.Services;
using Game.Web;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TeamMemberCard : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _name;
        [SerializeField] private Text _balance;
        [SerializeField] private Button _kickButton;

        [Header("Roles")]
        [SerializeField] private string _adminRoleColorHEX;
        [SerializeField] private string _memberRoleColorHEX;

        private string _currentRoleColorHex;

        private UserData _userData;
        private TeamData _teamData;

        public TeamMemberCard Create()
        {
            return null;
        }

        public void UpdateInfo(UserData user, TeamData team) 
        {
            _userData = user;
            _teamData = team;

            _currentRoleColorHex = _memberRoleColorHEX;
            if (user.PlayerData.TeamData.Role == "Admin")
            {
                _currentRoleColorHex = _adminRoleColorHEX;
                HideKickButton();
            }

            _name.text = $"{user.Login}<color={_currentRoleColorHex}><size=50>        {user.PlayerData.TeamData.Role} </size></color>";
            _balance.text = "Баланс: " + NumberConverter.NumToString(user.PlayerData.Tickets);
            _kickButton.onClick.AddListener(KickPlayer);
        }

        public void HideKickButton() => _kickButton.gameObject.SetActive(false);
        public void Disable() => _kickButton.onClick.RemoveAllListeners();
        private void KickPlayer() 
        {
            WWWForm form = new WWWForm();

            print(_teamData.Name);
            print(_userData.ID);

            form.AddField("Type", "TeamLeave");
            form.AddField("TeamName", _teamData.Name);
            form.AddField("UserID", _userData.ID);

            GameManager.WebRequestSender.SendData<TeamData>(GameManager.Config.DataBaseUrl, form, Delete);
        }
        private void Delete(TeamData team, WebOperationStatus status)
        {
            Destroy(this.gameObject);
        }
    }
}
