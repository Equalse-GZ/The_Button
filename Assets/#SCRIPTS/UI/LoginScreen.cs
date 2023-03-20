using Game.Auth;
using Game.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class LoginScreen : ScreenUI
    {
        [SerializeField] private InputField _loginInput;
        [SerializeField] private InputField _passwordInput;
        [SerializeField] private Text _message;

        private AuthorizationController _authorizationController;

        public void Initialize(AuthorizationController authorizationController)
        {
            _authorizationController = authorizationController;

            _message.text = "";

            _loginInput.onValueChanged.AddListener((str) => _loginInput.text = str.Replace(" ", ""));
            _passwordInput.onValueChanged.AddListener((str) => _passwordInput.text = str.Replace(" ", ""));
        }

        public void TryLogin()
        {
            if (CheckInputsAreEmpty() == true)
            {
                UpdateMessage("Не все поля заполнены!");
                return;
            }

            UserData userData = new UserData();
            userData.Login = _loginInput.text;
            userData.Password = _passwordInput.text;

            _loginInput.text = "";
            _passwordInput.text = "";

            _authorizationController.Login(userData);
        }

        public void ResetFields()
        {
            _loginInput.text = "";
            _passwordInput.text = "";
            _message.text = "";
        }

        public void UpdateMessage(string message)
        {
            _message.text = message;
        }

        private bool CheckInputsAreEmpty() => _loginInput.text == "" || _passwordInput.text == "";
    }
}

