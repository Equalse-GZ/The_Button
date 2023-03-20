using Game.Bonuses;
using Game.Core;
using Game.Data;
using Game.UI;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Handlers
{
    public class ConstantBonusesHandler : MonoBehaviour
    {
        [SerializeField] private List<ConstantReusableBonus> _reusableBonuses = new List<ConstantReusableBonus>();
        private List<ConstantDisposableBonus> _disposableBonuses = new List<ConstantDisposableBonus>();

        private BonusRepositoryScreen _bonusRepositoryScreen;

        public void Initialize(List<ConstantBonus> stockBonuses, List<BonusData> bonusesData)
        {
            _bonusRepositoryScreen = GameManager.UserInterface.GetScreen<BonusRepositoryScreen>();

            for (int i = 0; i < bonusesData.Count; i++)
            {
                for (int j = 0; j < stockBonuses.Count; j++)
                {
                    if (bonusesData[i].ID == stockBonuses[j].ID)
                    {
                        var bonus = Instantiate(stockBonuses[j]);
                        bonus.Count = bonusesData[i].Count;
                        AddBonus(bonus, false);
                    }
                }
            }
        }

        public void AddBonus(ConstantBonus newBonus, bool sendDataOnServer)
        {
            if (newBonus is ConstantReusableBonus)
            {
                var bonus = (ConstantReusableBonus)Instantiate(newBonus);

                if (_bonusRepositoryScreen.ContainsCard(bonus))
                {
                    foreach (var m_bonus in _reusableBonuses)
                    {
                        if (m_bonus.ID == bonus.ID)
                            m_bonus.Count += 1;
                    }

                    var card = _bonusRepositoryScreen.GetCard<BonusRepositoryCardReusable>(bonus);
                    card.IncreaseBonusCount();
                }

                else
                {
                    var card = _bonusRepositoryScreen.AddCard<BonusRepositoryCardReusable>(bonus);

                    foreach (var m_bonus in _reusableBonuses)
                    {
                        if(m_bonus.ID == bonus.ID)
                        {
                            m_bonus.Count += bonus.Count;
                            m_bonus.CardID = card.ID;
                            card.UpdateCount(m_bonus.Count);
                            return;
                        }
                    }

                    card.UpdateCount(bonus.Count);
                    bonus.CardID = card.ID;
                    _reusableBonuses.Add(bonus);
                    bonus.Use();
                }
            }

            if (newBonus is ConstantDisposableBonus)
            {
                var bonus = (ConstantDisposableBonus)Instantiate(newBonus);

                var card = _bonusRepositoryScreen.AddCard<BonusRepositoryCardReusable>(bonus);
                card.UpdateCount(1);
                bonus.CardID = card.ID;

                _disposableBonuses.Add(bonus);
                bonus.Use();
            }

            if (sendDataOnServer == true)
                SendDataOnServer(newBonus);
        }

        public bool ContainsBonus<T>(ConstantBonus bonus) where T : ConstantBonus
        {
            if (typeof(T) == typeof(ConstantReusableBonus))
            {
                foreach (var m_bonus in _reusableBonuses)
                {
                    if (m_bonus.ID == bonus.ID)
                        return true;
                }
            }

            if (typeof(T) == typeof(ConstantDisposableBonus))
            {
                foreach (var m_bonus in _disposableBonuses)
                {
                    if (m_bonus.ID == bonus.ID)
                        return true;
                }
            }
               

            return false;
        }

        public void Disable()
        {
            foreach (var bonus in _reusableBonuses)
                bonus.Disable();

            foreach (var bonus in _disposableBonuses)
                bonus.Disable();

            _disposableBonuses.Clear();
            _reusableBonuses.Clear();
        }

        private void SendDataOnServer(ConstantBonus bonus)
        {
            int count = 0;

            if (bonus is ConstantReusableBonus)
            {
                foreach (var m_bonus in _reusableBonuses)
                {
                    if (m_bonus.ID == bonus.ID)
                    {
                        count = m_bonus.Count;
                        break;
                    }
                }
            }

            else count = 1;

            WWWForm form = new WWWForm();
            form.AddField("Type", "BonusAdd");
            form.AddField("UserID", GameManager.UserData.ID);
            form.AddField("BonusID", bonus.ID);
            form.AddField("BonusType", "CONSTANT");
            form.AddField("BonusCount", count);

            GameManager.WebRequestSender.SendData<BonusData>(GameManager.Config.DataBaseUrl, form, null);
        }

        private int CalculateBonuses<T>(ConstantBonus bonus) where T : ConstantBonus
        {
            var type = typeof(T);

            if(type == typeof(ConstantReusableBonus))
            {
                foreach (var r_bonus in _reusableBonuses)
                {
                    if (bonus.ID == r_bonus.ID)
                        return bonus.Count;
                }
            }

            if (type == typeof(ConstantDisposableBonus))
            {
                foreach (var d_bonus in _disposableBonuses)
                {
                    if (bonus.ID == d_bonus.ID)
                        return bonus.Count;
                }
            }

            return 0;
        }
    }
}
