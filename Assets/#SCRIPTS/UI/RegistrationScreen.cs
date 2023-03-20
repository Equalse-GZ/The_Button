using Game.Auth;
using Game.Data;
using System.Collections;
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
            if(CheckInputsAreEmpty() == true)
            {
                UpdateMessage("Не все поля заполнены!");
                return;
            }

            if(CheckPasswordsEqual() == false)
            {
                UpdateMessage("Пароли не совпадают!");
                return;
            }

            if(_avatarsSelectionMenu.AvatarIsSelected() == false)
            {
                UpdateMessage("Пожалуйста, выберите своего аватара");
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
        private bool CheckInputsAreEmpty() => _loginInput.text == "" || _password1Input.text == "" || _password2Input.text == "";
    }
}
