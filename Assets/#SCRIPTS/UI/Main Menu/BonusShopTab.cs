using Game.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public class BonusShopTab : MainMenuTab
    {
        public UnityEvent PageOpenEvent = new UnityEvent();

        [Header("Submenu")]
        [SerializeField] private Button _constantBonusesTab;
        [SerializeField] private Button _temporaryBonusesTab;

        public override void Initialize()
        {
            base.Initialize();

            _constantBonusesTab.gameObject.SetActive(false);
            _temporaryBonusesTab.gameObject.SetActive(false);

            _constantBonusesTab.onClick.AddListener(() =>
            {
                GameManager.ScreensController.ShowScreen<BonusShopScreen>();
                GameManager.UserInterface.GetScreen<BonusShopScreen>().OpenPage<ConstantBonusesPage>();
                PageOpenEvent?.Invoke();
            });

            _temporaryBonusesTab.onClick.AddListener(() =>
            {
                GameManager.ScreensController.ShowScreen<BonusShopScreen>();
                GameManager.UserInterface.GetScreen<BonusShopScreen>().OpenPage<TemporaryBonusesPage>();
                PageOpenEvent?.Invoke();
            });

            Button.onClick.AddListener(Toggle);
        }

        private void Toggle()
        {
            _constantBonusesTab.gameObject.SetActive(!_constantBonusesTab.gameObject.activeSelf);
            _temporaryBonusesTab.gameObject.SetActive(!_temporaryBonusesTab.gameObject.activeSelf);
        }
    }
}