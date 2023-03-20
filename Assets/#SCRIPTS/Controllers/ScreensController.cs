using Game.Core;
using Game.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Controllers
{
    public class ScreensController : MonoBehaviour
    {
        private Dictionary<Type, ScreenUI> screensMap = new Dictionary<Type, ScreenUI>();

        public void Initialize()
        {
            AddScreen<GamingScreen>();
            AddScreen<BonusShopScreen>();
            AddScreen<ProfileScreen>();
            AddScreen<LeaderBoardScreen>();
            AddScreen<BonusRepositoryScreen>();
            AddScreen<RegistrationScreen>();
            AddScreen<LoginScreen>();

            foreach (ScreenUI screen in screensMap.Values)
                screen.Hide();
        }

        public void ShowScreen<T>() where T : ScreenUI
        {
            Type type = typeof(T);
            if (screensMap.ContainsKey(type) == false)
                throw new Exception("The specified screen does not exist");

            foreach (ScreenUI screen in screensMap.Values)
                screen.Hide();

            screensMap[type].Show();
        }

        private void AddScreen<T>() where T : ScreenUI
        {
            screensMap.Add(GameManager.UserInterface.GetScreen<T>().GetType(), GameManager.UserInterface.GetScreen<T>());
        }
    }
}

