using Game.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class LeaderboardMenu : MonoBehaviour
    {
        [SerializeField] private Button _individual;
        [SerializeField] private Button _team;

        private void Start()
        {
            _individual.gameObject.SetActive(false);
            _team.gameObject.SetActive(false);

            _individual.onClick.AddListener(() => 
            {
                GameManager.ScreensController.ShowScreen<LeaderBoardScreen>();
                GameManager.UserInterface.GetScreen<LeaderBoardScreen>().LoadPersons();
             });

            _team.onClick.AddListener(() =>
            {
                GameManager.ScreensController.ShowScreen<LeaderBoardScreen>();
                GameManager.UserInterface.GetScreen<LeaderBoardScreen>().LoadTeams();
            });
        }

        public void Toggle()
        {
            _individual.gameObject.SetActive(!_individual.gameObject.activeSelf);
            _team.gameObject.SetActive(!_team.gameObject.activeSelf);
        }
    }
}
