using Game.Bonuses;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class BonusRepositoryCardReusable : BonusRepositoryCard
    {
        [Header("Unique")]
        [SerializeField] private Text _count;

        public string GetBonusName => _nameText.text;

        public void UpdateCount(int count) => _count.text = "Количество: " + count.ToString();
        public void IncreaseBonusCount()
        {
            int oldCount = int.Parse(_count.text.Replace("Количество: ", ""));
            _count.text = "Количество: " + (oldCount + 1).ToString();
        }
    }
}
