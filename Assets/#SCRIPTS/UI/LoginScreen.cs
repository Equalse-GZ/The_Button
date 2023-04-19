using Game.Auth;
using Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class LoginScreen : ScreenUI
    {
        [SerializeField] private InputField _loginInput;
        [SerializeField] private InputField _passwordInput;
        [SerializeField] private Text _message;
        [SerializeField] private GameObject _loadingObject;

        private AuthorizationController _authorizationController;

        public void Initialize(AuthorizationController authorizationController)
        {
            _authorizationController = authorizationController;

            _message.text = "";
            _loadingObject.SetActive(false);

            _loginInput.onValueChanged.AddListener((str) => _loginInput.text = str.Replace(" ", ""));
            _passwordInput.onValueChanged.AddListener((str) => _passwordInput.text = str.Replace(" ", ""));
        }

        public void TryLogin()
        {
            if (CheckInputsAreEmpty(out List<AuthorizationInputField> inputs) == true)
            {
                UpdateMessage("Не все поля заполнены!");
                foreach (var input in inputs)
                    input.PlayPulseAnimation();

                return;
            }

            UserData userData = new UserData();
            userData.Login = _loginInput.text;
            userData.Password = _passwordInput.text;

            _loginInput.text = "";
            _passwordInput.text = "";

            _loadingObject.SetActive(true);
            _authorizationController.Login(userData);
        }

        public void ResetFields()
        {
            _loginInput.text = "";
            _passwordInput.text = "";
            _message.text = "";
            _loadingObject.SetActive(false);
        }

        public void UpdateMessage(string message)
        {
            _message.text = message;
            _loadingObject.SetActive(false);
        }

        private bool CheckInputsAreEmpty(out List<AuthorizationInputField> inputs)
        {
            inputs = new List<AuthorizationInputField>();

            if(_loginInput.text == "")
                inputs.Add(_loginInput.GetComponent<AuthorizationInputField>());

            if(_passwordInput.text == "")
                inputs.Add(_passwordInput.GetComponent<AuthorizationInputField>());

            return inputs.Count != 0;
        }
    }
}

