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
        [SerializeField] private Image _bg;
        [SerializeField] private Image _crownIcon;
        [SerializeField] private Text _name;
        [SerializeField] private Text _balance;
        [SerializeField] private Button _kickButton;


        [Header("Roles")]
        [SerializeField] private Color _adminRoleBGColor;
        [SerializeField] private Color _memberRoleBGColor;
        [SerializeField] private Color _adminRoleNickNameColor;

        private Color _currentRoleBGColor;

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

            _icon.sprite = GameManager.AvatarsController.GetAvatar(user.Icon).Icon;

            _currentRoleBGColor = _memberRoleBGColor;
            _name.color = Color.white;
            if (user.PlayerData.TeamData.Role == "Admin")
            {
                _currentRoleBGColor = _adminRoleBGColor;
                _name.color = _adminRoleNickNameColor;
                _crownIcon.gameObject.SetActive(true);
                HideKickButton();
            }

            _bg.color = _currentRoleBGColor;
            _name.text = user.Login.ToUpper();
            _balance.text = "Баланс: " + NumberConverter.NumToString(user.PlayerData.Tickets) + " б";
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
