using Game.Core;
using UnityEngine;

namespace Game.UI
{
    public class BonusRepositoryButton : MonoBehaviour
    {
        public void OnClick()
        {
            GameManager.ScreensController.ShowScreen<BonusRepositoryScreen>();
        }
    }
}
