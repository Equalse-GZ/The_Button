using UnityEngine;

namespace Game.UI
{
    public class BonusShopPage : MonoBehaviour
    {
        public virtual void Initialize() { }
        public virtual void Show()
        {
            this.gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}
