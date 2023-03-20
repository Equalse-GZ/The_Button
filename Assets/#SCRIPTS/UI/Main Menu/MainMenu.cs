using Game.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class MainMenu : Menu
    {
        [Header("Tabs")]
        [SerializeField] private BonusShopTab _bonusShopTab;
        [SerializeField] private HomeTab _homeTab;
        [SerializeField] private ProfileTab _profileTab;

        [Header("Indicator")]
        [SerializeField] private MenuIndicator _indicatorObject;

        public override void Initialize()
        {
            base.Initialize();

            _bonusShopTab.Initialize();
            _homeTab.Initialize();
            _profileTab.Initialize();

            MoveIndicator(_homeTab);

            _homeTab.Button.onClick.AddListener(() =>
            {
                GameManager.ScreensController.ShowScreen<GamingScreen>();
                MoveIndicator(_homeTab);
            });

            _profileTab.Button.onClick.AddListener(() =>
            {
                GameManager.ScreensController.ShowScreen<ProfileScreen>();
                MoveIndicator(_profileTab);
            });

            _bonusShopTab.PageOpenEvent.AddListener(() => MoveIndicator(_bonusShopTab));
        }

        private void MoveIndicator(MainMenuTab tab)
        {
            _indicatorObject.MoveTo(new Vector3(tab.IndicatorTransform.position.x, _indicatorObject.transform.position.y, _indicatorObject.transform.position.z));
        }
    }
}
