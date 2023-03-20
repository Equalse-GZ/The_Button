using Game.Bonuses;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(Animator))]
    public class RarityBonusMessage : MonoBehaviour
    {
        [SerializeField] private Text _bonusNameText;

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void ChangeInfo(RarityBonus bonus)
        {
            _bonusNameText.text = bonus.Name;
        }

        public void Throw()
        {
            _animator.SetTrigger("Show");
        }
    }
}
