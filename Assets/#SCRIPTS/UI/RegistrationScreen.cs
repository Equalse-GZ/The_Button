using Game.Auth;
using Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class RegistrationScreen : ScreenUI
    {
        [SerializeField] private Text _message;
        [SerializeField] private InputField _loginInput;
        [SerializeField] private InputField _password1Input;
        [SerializeField] private InputField _password2Input;

        [Space(15)]
        [SerializeField] private AvatarsSelectionMenu _avatarsSelectionMenu;

        [Space(10)]
        [SerializeField] private List<string> _weekPasswords = new List<string>();

        private AuthorizationController _authorizationController;

        public void Initialize(AuthorizationController authorizationController)
        {
            _authorizationController = authorizationController;
            _message.text = "";

            _loginInput.onValueChanged.AddListener((str) => _loginInput.text = str.Replace(" ", ""));
            _password1Input.onValueChanged.AddListener((str) => _password1Input.text = str.Replace(" ", ""));
            _password2Input.onValueChanged.AddListener((str) => _password2Input.text = str.Replace(" ", ""));

            _avatarsSelectionMenu.Initialize();
        }

        public void TryRegister()
        {
            if(CheckInputsAreEmpty(out List<AuthorizationInputField> inputs) == true)
            {
                UpdateMessage("Не все поля заполнены!");
                foreach (var input in inputs)
                    input.PlayPulseAnimation();

                return;
            }

            if(CheckLoginLenght(out string loginMessage) == false)
            {
                UpdateMessage(loginMessage);
                _loginInput.GetComponent<AuthorizationInputField>().PlayPulseAnimation();

                return;
            }

            if(CheckPasswordLenght(out string passwordMessage) == false)
            {
                UpdateMessage(passwordMessage);
                _password1Input.GetComponent<AuthorizationInputField>().PlayPulseAnimation();
                _password2Input.GetComponent<AuthorizationInputField>().PlayPulseAnimation();

                return;
            }

            if(CheckPasswordsEqual() == false)
            {
                UpdateMessage("Пароли не совпадают!");
                _password1Input.GetComponent<AuthorizationInputField>().PlayPulseAnimation();
                _password2Input.GetComponent<AuthorizationInputField>().PlayPulseAnimation();
                return;
            }

            if(CheckPasswordsValidation() == false)
            {
                UpdateMessage("Ваш пароль довольно ненадёжен!");
                _password1Input.GetComponent<AuthorizationInputField>().PlayPulseAnimation();
                _password2Input.GetComponent<AuthorizationInputField>().PlayPulseAnimation();
                return;
            }

            if(_avatarsSelectionMenu.AvatarIsSelected() == false)
            {
                UpdateMessage("Пожалуйста, выберите своего аватара");
                _avatarsSelectionMenu.PlayPulseAnimation();
                return;
            }

            _message.text = "";

            UserData userData = new UserData();
            userData.ID = 0;
            userData.Login = _loginInput.text;
            userData.Password = _password1Input.text;
            userData.Icon = _avatarsSelectionMenu.GetSelectedAvatar().Name;

            _loginInput.text = "";
            _password1Input.text = "";
            _password2Input.text = "";
            
            _authorizationController.Register(userData);
        }

        public void ResetFields()
        {
            _loginInput.text = "";
            _password1Input.text = "";
            _password2Input.text = "";
            _message.text = "";
            _avatarsSelectionMenu.ResetSelectedAvatar();
        }

        public void UpdateMessage(string message)
        {
            _message.text = message;
        }

        private bool CheckPasswordsEqual() => _password1Input.text == _password2Input.text;

        private bool CheckPasswordsValidation()
        {
            foreach (var password in _weekPasswords)
            {
                if (_password1Input.text == password)
                    return false;
            }

            return true;
        }

        private bool CheckLoginLenght(out string message)
        {
            message = "";

            if(_loginInput.text.Length > 12)
            {
                message = "Максимальная длина логина - 12 символов";
                return false;
            }

            if(_loginInput.text.Length < 5)
            {
                message = "Минимальная длина логина - 5 символов";
                return false;
            }

            return true;
        }

        private bool CheckPasswordLenght(out string message)
        {
            message = "";

            if (_password1Input.text.Length < 5)
            {
                message = "Минимальная длина пароля - 8 символов";
                return false;
            }

            return true;
        }

        private bool CheckInputsAreEmpty(out List<AuthorizationInputField> inputs)
        {
            inputs = new List<AuthorizationInputField>();

            if (_loginInput.text == "")
                inputs.Add(_loginInput.GetComponent<AuthorizationInputField>());

            if (_password1Input.text == "")
                inputs.Add(_password1Input.GetComponent<AuthorizationInputField>());

            if (_password2Input.text == "")
                inputs.Add(_password2Input.GetComponent<AuthorizationInputField>());

            return inputs.Count != 0;
        }
    }
}
