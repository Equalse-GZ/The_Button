using Game.Data;
using UnityEngine;

namespace Game.UI
{
    public class TeamInactiveBlock : MonoBehaviour
    {
        [SerializeField] private CreateTeamScreen _createTeamScreen;
        [SerializeField] private ConnectTeamScreen _connectTeamScreen;

        public void Initialize(UserData data)
        {
            _createTeamScreen.Initialize(data);
            _connectTeamScreen.Initialize(data);
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
            _createTeamScreen.gameObject.SetActive(false);
            _connectTeamScreen.gameObject.SetActive(false);
        }

        public void Disable()
        {

        }
    }
}
