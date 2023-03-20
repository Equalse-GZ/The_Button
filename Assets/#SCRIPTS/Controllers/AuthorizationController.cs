using Game.Controllers;
using Game.Core;
using Game.Data;
using Game.UI;
using Game.Web;
using System;
using UnityEngine;

namespace Game.Auth
{
    public class AuthorizationController : MonoBehaviour
    {
        public Action<UserData> AuthorizationSuccessfull;
        private const string LOGIN_KEY = "AUTH_LOGIN_KEY";
        private const string PASSWORD_KEY = "AUTH_PASS_KEY";
        private string _dataBaseURL;

        private WebRequestSender _webRequestSender;
        private ScreensController _screensController;

        private RegistrationScreen _registrationScreen;
        private LoginScreen _loginScreen;

        public void Initialize(string dataBaseURL,  WebRequestSender webRequestSender, UserInterface ui, ScreensController screensController)
        {
            _dataBaseURL = dataBaseURL;

            _webRequestSender = webRequestSender;
            _screensController = screensController;

            _registrationScreen = ui.GetScreen<RegistrationScreen>();
            _registrationScreen.Initialize(this);

            _loginScreen = ui.GetScreen<LoginScreen>();
            _loginScreen.Initialize(this);

            if(PlayerPrefs.GetString(LOGIN_KEY) != "" && PlayerPrefs.GetString(PASSWORD_KEY) != "")
            {
                UserData data = new UserData();
                data.Login = PlayerPrefs.GetString(LOGIN_KEY);
                data.Password = PlayerPrefs.GetString(PASSWORD_KEY);
            
                Login(data);
            }

            _screensController.ShowScreen<LoginScreen>();
        }

        public void Login(UserData data)
        {
            WWWForm form = new WWWForm();
            form.AddField("Type", "Logging");
            form.AddField("Login", data.Login);
            form.AddField("Password", data.Password);

            PlayerPrefs.SetString(LOGIN_KEY, data.Login);
            PlayerPrefs.SetString(PASSWORD_KEY, data.Password);
            _webRequestSender.SendData<UserData>(_dataBaseURL, form, OnDataRecieved);
        }

        public void Register(UserData data)
        {
            WWWForm form = new WWWForm();
            form.AddField("Type", "Registering");
            form.AddField("Login", data.Login);
            form.AddField("Password", data.Password);
            form.AddField("Icon", data.Icon);

            PlayerPrefs.SetString(LOGIN_KEY, data.Login);
            PlayerPrefs.SetString(PASSWORD_KEY, data.Password);
            _webRequestSender.SendData<UserData>(_dataBaseURL, form, OnDataRecieved);
        }

        public void LogOut()
        {
            _screensController.ShowScreen<LoginScreen>();
        }

        private void OnDataRecieved(UserData data,  WebOperationStatus webOperationStatus)
        {
            if (webOperationStatus == WebOperationStatus.Failed)
                throw new Exception("Getting data failed");

            if(data.ErrorData.IsEmpty == false)
            {
                _registrationScreen.UpdateMessage(data.ErrorData.Message);
                _loginScreen.UpdateMessage(data.ErrorData.Message);
                print(data.ErrorData.Message);
                return;
            }

            _registrationScreen.ResetFields();
            _loginScreen.ResetFields();

            AuthorizationSuccessfull?.Invoke(data);
        }
    }
}
