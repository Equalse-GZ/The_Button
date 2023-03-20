using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class MainMenuTab : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _button;
        [SerializeField] private Transform _indicatorTransform;

        public Button Button => _button;
        public Transform IndicatorTransform => _indicatorTransform;

        public virtual void Initialize() { }
    }
}
