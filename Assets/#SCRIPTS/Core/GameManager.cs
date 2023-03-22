using Game.Auth;
using Game.Controllers;
using Game.Data;
using Game.Services;
using Game.UI;
using Game.Web;
using UnityEngine;

namespace Game.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameConfig _config;

        [SerializeField] private WebRequestSender _webRequestSender;

        [Header("Controllers")]
        [SerializeField] private AuthorizationController _authorizationController;
        [SerializeField] private AvatarsController _avatarsController;
        [SerializeField] private MainButtonController _mainButtonController;
        [SerializeField] private TicketsBankController _ticketsBankController;
        [SerializeField] private ScreensController _screensController;
        [SerializeField] private BonusRepositoryController _bonusRepositoryController;

        [Header("User Interface")]
        [SerializeField] private UserInterface _userInterface;

        [SerializeField] private GlobalTimer _globalTimer;

        public static WebRequestSender WebRequestSender { get; private set; }
        public static MainButtonController MainButtonController { get; private set; }
        public static TicketsBankController TicketsBankController { get; private set; }
        public static ScreensController ScreensController { get; private set; }
        public static BonusRepositoryController BonusRepositoryController { get; private set; }
        public static GameConfig Config { get; private set; }

        public static UserInterface UserInterface { get; private set; }
        public static InteractionService InteractionService { get; private set; }
        public static GlobalTimer GlobalTimer { get; private set; }
        public static UserData UserData { get; set; }
        public static AvatarsController AvatarsController { get; set; }

        private static AuthorizationController AuthorizationController;

        private int _userID = 0;

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        public void Initialize() 
        {
            Application.targetFrameRate = 120;

            Config = _config;
            WebRequestSender = _webRequestSender;

            _ticketsBankController.Initialize(0, _userID);
            TicketsBankController = _ticketsBankController;

            _userInterface.Initialize();
            UserInterface = _userInterface;

            _screensController.Initialize();
            ScreensController = _screensController;

            _authorizationController.Initialize(_config.DataBaseUrl, _webRequestSender, _userInterface, _screensController);
            _authorizationController.AuthorizationSuccessfull += Run;
        }

        public void Run(UserData userData) 
        {
            UserData = userData;
            _userID = userData.ID;
            TicketsBankController.Initialize(userData.PlayerData.Tickets, _userID);

            _mainButtonController.Initialize();
            MainButtonController = _mainButtonController;

            _globalTimer.Initialize();
            GlobalTimer = _globalTimer;

            _bonusRepositoryController.Initialize(Config.DataBaseUrl);
            BonusRepositoryController = _bonusRepositoryController;

            AvatarsController = _avatarsController;

            UserInterface.GetScreen<ProfileScreen>().UpdateInfo(userData);

            ScreensController.ShowScreen<GamingScreen>();
            _authorizationController.AuthorizationSuccessfull -= Run;
        }

        public void Stop() 
        {
            MainButtonController.Disable();
            GlobalTimer.Disable();
            BonusRepositoryController.Disable();
            UserInterface.GetScreen<ProfileScreen>().Disable();
            UserInterface.Disable();
            TicketsBankController.Disable();
            Save();

            _authorizationController.AuthorizationSuccessfull += Run;
        }

        public void Save() 
        {
            WWWForm form = new WWWForm();
            form.AddField("Type", "Save");
            form.AddField("ID", _userID);
            form.AddField("Tickets", TicketsBankController.Tickets);

            _webRequestSender.SendData<UserData>(_config.DataBaseUrl, form, null);
        }
    }
}
