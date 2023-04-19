using Game.Bonuses;
using Game.Core;
using Game.UI;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Handlers
{
    public class RarityBonusesHandler : MonoBehaviour
    {
        [SerializeField] private RarityBonusMessage _message;

        [SerializeField] private List<RarityBonus> _bonuses = new List<RarityBonus>();
        private BonusRepositoryScreen _bonusRepositoryScreen;

        [SerializeField] private bool _bonusIsWorking = false;

        public void Initialize(List<RarityBonus> rarityBonuses)
        {
            _message.gameObject.SetActive(true);
            _bonusRepositoryScreen = GameManager.UserInterface.GetScreen<BonusRepositoryScreen>();
            GameManager.MainButtonController.ClickEvent.AddListener(DetermineBonus);
            foreach (var bonus in rarityBonuses)
                _bonuses.Add(Instantiate(bonus));
        }

        public void Disable()
        {
            GameManager.MainButtonController.ClickEvent.RemoveListener(DetermineBonus);
            StopAllCoroutines();
            _bonusIsWorking = false;
        }

        private void DetermineBonus()
        {
            if (_bonusIsWorking == true)
                return;

            float chance = Random.Range(0f, 3f);
            List<float> occurrenceChances = new List<float>();

            foreach (var bonus in _bonuses)
            {
                if(chance < bonus.OccurrenceChance)
                    occurrenceChances.Add(bonus.OccurrenceChance);
            }

            if (occurrenceChances.Count == 0) return;

            occurrenceChances.Sort();
            float minOccurrenceChance = occurrenceChances[0];

            List<RarityBonus> luckyBonuses = new List<RarityBonus>();
            foreach (var bonus in _bonuses)
            {
                if(bonus.OccurrenceChance == minOccurrenceChance)
                    luckyBonuses.Add(bonus);
            }
            

            if (luckyBonuses.Count == 0) return;
            if (luckyBonuses.Count == 1) UseBonus(luckyBonuses[0]);

            else
            {
                int luckyBonusNumber = Random.Range(0, luckyBonuses.Count);
                UseBonus(luckyBonuses[luckyBonusNumber]);
            }

            occurrenceChances.Clear();
            luckyBonuses.Clear();
        }

        private void UseBonus(RarityBonus bonus)
        {
            _bonusIsWorking = true;
            bonus.WorkCompleted.AddListener(OnBonusWorkCompleted);

            if(bonus is RarityTemporaryBonus)
            {
                var m_bonus = (RarityTemporaryBonus)Instantiate(bonus);
                var card = _bonusRepositoryScreen.AddCard<BonusRepositoryCardTemporary>(bonus);
                m_bonus.WorkCompleted.AddListener(OnBonusWorkCompleted);
                m_bonus.CardID = card.ID;
                m_bonus.Use(OnSecondPassed, OnTimeIsOut);
            }

            if (bonus is RarityIncrementalBonus)
            {
                var m_bonus = (RarityIncrementalBonus)Instantiate(bonus);
                m_bonus.WorkCompleted.AddListener(OnBonusWorkCompleted);
                m_bonus.Use();
            }

            if (bonus is RarityClickableBonus)
            {
                var m_bonus = (RarityClickableBonus)Instantiate(bonus);
                m_bonus.WorkCompleted.AddListener(OnBonusWorkCompleted);
                m_bonus.Use();
            }

            if (bonus is RarityStopableBonus)
            {
                var m_bonus = (RarityStopableBonus)Instantiate(bonus);
                m_bonus.WorkCompleted.AddListener(OnBonusWorkCompleted);
                m_bonus.Use();
            }

            _message.ChangeInfo(bonus);
            _message.Throw();

        }

        private void OnBonusWorkCompleted(RarityBonus bonus)
        {
            _bonusIsWorking = false;
            bonus.WorkCompleted.RemoveListener(OnBonusWorkCompleted);
        }

        private void OnSecondPassed(RarityTemporaryBonus bonus, int remainingTime)
        {
            _bonusRepositoryScreen.GetCard<BonusRepositoryCardTemporary>(bonus.CardID).UpdateTimer(bonus.ActionTime, remainingTime);
        }

        private void OnTimeIsOut(RarityTemporaryBonus bonus)
        {
            _bonusRepositoryScreen.DeleteCard(bonus.CardID);
        }
    }
}
