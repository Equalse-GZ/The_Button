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
        [SerializeField] private SyncController _syncController;

        [Header("Utils")]
        [SerializeField] private PoolManager _poolManager;
        [SerializeField] private GameTimer _gameTimer;

        [Header("User Interface")]
        [SerializeField] private UserInterface _userInterface;

        [SerializeField] private GlobalTimer _globalTimer;

        public static WebRequestSender WebRequestSender { get; private set; }
        public static MainButtonController MainButtonController { get; private set; }
        public static TicketsBankController TicketsBankController { get; private set; }
        public static ScreensController ScreensController { get; private set; }
        public static BonusRepositoryController BonusRepositoryController { get; private set; }
        public static SyncController SyncController { get; private set; }
        public static GameConfig Config { get; private set; }

        public static PoolManager PoolManager;

        public static UserInterface UserInterface { get; private set; }
        public static InteractionService InteractionService { get; private set; }
        public static GlobalTimer GlobalTimer { get; private set; }
        public static UserData UserData { get; set; }
        public static AvatarsController AvatarsController { get; set; }

        private static AuthorizationController AuthorizationController;

        private int _userID = 0;

        private void Awake()
        {
            Application.targetFrameRate = 144;

            Config = _config;
            WebRequestSender = _webRequestSender;

            _ticketsBankController.Initialize();
            TicketsBankController = _ticketsBankController;

            _userInterface.Initialize();
            UserInterface = _userInterface;

            _screensController.Initialize();
            ScreensController = _screensController;

            GetGameUntilTime();
        }

        public void Initialize(GameUntilTime untilTime, WebOperationStatus webOperationStatus) 
        {
            if(untilTime.TimeLeft <= 0)
            {
                ScreensController.ShowScreen<GameOverScreen>();
                return;
            }

            _globalTimer.Initialize();
            GlobalTimer = _globalTimer;

            _gameTimer.Initialize(untilTime);
            _gameTimer.TimeOutEvent.AddListener(Stop);

            _authorizationController.Initialize(_config.DataBaseUrl, _webRequestSender, _userInterface, _screensController);
            _authorizationController.AuthorizationSuccessfull += Run;
        }

        public void Run(UserData userData) 
        {
            UserData = userData;
            _userID = userData.ID;

            ScreensController.ShowScreen<LoadingScreen>();

            _syncController.Initialize(userData);
            SyncController = _syncController;

            TicketsBankController.UpdateTickets(userData.PlayerData.Tickets);

            _mainButtonController.Initialize();
            MainButtonController = _mainButtonController;

            _bonusRepositoryController.Initialize(Config.DataBaseUrl);
            BonusRepositoryController = _bonusRepositoryController;
            BonusRepositoryController.InitializedEvent.AddListener(OnGameInitialized);

            AvatarsController = _avatarsController;
            UserInterface.GetScreen<ProfileScreen>().UpdateInfo(userData);
        }

        private void GetGameUntilTime()
        {
            WWWForm form = new WWWForm();
            form.AddField("Type", "ET");

            WebRequestSender.SendData<GameUntilTime>(Config.DataBaseUrl, form, Initialize);
        }

        public void Stop() 
        {
            MainButtonController.Disable();
            GlobalTimer.Disable();
            BonusRepositoryController.Disable();
            UserInterface.GetScreen<ProfileScreen>().Disable();
            UserInterface.Disable();
            SyncController.Disable();
            TicketsBankController.Save();
            Save();

            _gameTimer.TimeOutEvent.RemoveListener(Stop);
            _authorizationController.AuthorizationSuccessfull += Run;
        }

        public void Save()
        {
            WWWForm form = new WWWForm();
            form.AddField("Type", "Save");
            form.AddField("ID", _userID);
            form.AddField("Tickets", TicketsBankController.Tickets.ToString());

            WebRequestSender.SendData<UserData>(Config.DataBaseUrl, form, null);
        }

        private void OnGameInitialized()
        {
            PoolManager = _poolManager;
            ScreensController.ShowScreen<GamingScreen>();
            _authorizationController.AuthorizationSuccessfull -= Run;
        }
    }
}
