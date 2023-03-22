using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UserInterface : MonoBehaviour
    {
        [SerializeField] private List<ScreenUI> _screens = new List<ScreenUI>();
        [SerializeField] private List<Menu> _menus = new List<Menu>();

        private Dictionary<Type, ScreenUI> _screensMap = new Dictionary<Type, ScreenUI>();
        private Dictionary<Type, Menu> _menusMap = new Dictionary<Type, Menu>();

        public void Initialize()
        {
            foreach (ScreenUI screen in _screens)
            {
                screen.Initialize();
                _screensMap.Add(screen.GetType(), screen);
            }

            foreach (Menu menu in _menus)
            {
                menu.Initialize();
                _menusMap.Add(menu.GetType(), menu);
            }
        }

        public void Disable()
        {
            foreach (Menu menu in _menus)
            {
                menu.Disable();
                _menusMap.Remove(menu.GetType());
            }
        }

        public T GetScreen<T>() where T : ScreenUI
        {
            Type type = typeof(T);
            if (_screensMap.ContainsKey(type) == false)
                throw new Exception("The specified Screen does not exist");

            return (T)_screensMap[type];
        }

        public T GetMenu<T>() where T : Menu
        {
            Type type = typeof(T);
            if (_menusMap.ContainsKey(type) == false)
                throw new Exception("The specified Menu does not exist");

            return (T)_menusMap[type];
        }
    }
}
