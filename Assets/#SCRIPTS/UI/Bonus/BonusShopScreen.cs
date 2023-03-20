using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class BonusShopScreen : ScreenUI
    {
        [SerializeField] private List<BonusShopPage> _pages = new List<BonusShopPage>();
        [SerializeField] private BonusShopBalanceUI _balanceUI;

        private Dictionary<Type, BonusShopPage> _pagesMap = new Dictionary<Type, BonusShopPage>();

        public override void Initialize()
        {
            base.Initialize();

            foreach (BonusShopPage page in _pages)
            {
                page.gameObject.SetActive(false);
                page.Initialize();
                _pagesMap.Add(page.GetType(), page);
            }

            _balanceUI.Initialize();
        }

        public void OpenPage<T>() where T : BonusShopPage
        {
            Type type = typeof(T);
            if (_pagesMap.ContainsKey(type) == false)
                throw new Exception("The specified Page does not exist");

            foreach (BonusShopPage page in _pagesMap.Values)
                page.Hide();

            _pagesMap[type].Show();
        }

        public T GetPage<T>() where T : BonusShopPage
        {
            Type type = typeof(T);
            if (_pagesMap.ContainsKey(type) == false)
                throw new Exception("The specified Page does not exist");

            return (T)_pagesMap[type];
        }
    }
}
