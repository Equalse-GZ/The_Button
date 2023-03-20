using Game.Bonuses;
using Game.Core;
using Game.Data;
using Game.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Handlers
{
    public class TemporaryBonusesHandler : MonoBehaviour
    {
        public List<TemporaryBonus> Bonuses = new List<TemporaryBonus>();
        private BonusRepositoryScreen _bonusRepositoryScreen;

        public void Initialize(List<TemporaryBonus> stockBonuses, List<BonusData> bonusesData)
        {
            _bonusRepositoryScreen = GameManager.UserInterface.GetScreen<BonusRepositoryScreen>();

            for (int i = 0; i < bonusesData.Count; i++)
            {
                for (int j = 0; j < stockBonuses.Count; j++)
                {
                    if (bonusesData[i].ID == stockBonuses[j].ID)
                        AddBonus(Instantiate(stockBonuses[j]), bonusesData[i].AppearanceTime, false);
                }
            }
        }

        public void AddBonus(TemporaryBonus newBonus, int passedTime, bool sendDataOnServer)
        {
            if (sendDataOnServer == true)
                SendDataOnServer(newBonus);

            var card = _bonusRepositoryScreen.AddCard<BonusRepositoryCardTemporary>(newBonus);
            newBonus.CardID = card.ID;

            Bonuses.Add(newBonus);
            newBonus.Use(passedTime, OnSecondPassed, OnTimeIsUp);
        }

        public void RemoveAllBonuses(TemporaryBonus bonus)
        {
            List<int> ids = new List<int>();
            for (int i = 0; i < Bonuses.Count; i++)
            {
                if (bonus.ID == Bonuses[i].ID)
                    ids.Add(i);
            }

            int j = 0;
            foreach (var id in ids)
            {
                Bonuses[id-j].Disable();
                _bonusRepositoryScreen.DeleteCard(Bonuses[id-j].CardID);
                DeleteBonusOnServer(Bonuses[id-j]);
                Bonuses.RemoveAt(id - j);
                j++;
            }
        }

        public void RemoveBonus(TemporaryBonus bonus)
        {
            if (Bonuses.Contains(bonus) == false)
                return;

            bonus.Disable();
            _bonusRepositoryScreen.DeleteCard(bonus.CardID);
            Bonuses.Remove(bonus);
            DeleteBonusOnServer(bonus);
        }

        public void Disable()
        {
            foreach (var bonus in Bonuses)
                bonus.Disable();

            Bonuses.Clear();
        }

        private void SendDataOnServer(TemporaryBonus bonus)
        {
            WWWForm form = new WWWForm();

            form.AddField("Type", "BonusAdd");
            form.AddField("UserID", GameManager.UserData.ID);
            form.AddField("BonusID", bonus.ID);
            form.AddField("BonusType", "TEMPORARY");
            form.AddField("BonusCount", 1);

            GameManager.WebRequestSender.SendData<BonusData>(GameManager.Config.DataBaseUrl, form, null);
        }

        private void DeleteBonusOnServer(TemporaryBonus bonus)
        {
            WWWForm form = new WWWForm();

            form.AddField("Type", "BonusDelete");
            form.AddField("UserID", GameManager.UserData.ID);
            form.AddField("BonusID", bonus.ID);

            GameManager.WebRequestSender.SendData<BonusData>(GameManager.Config.DataBaseUrl, form, null);
        }

        private void OnSecondPassed(TemporaryBonus bonus, int remainingTime)
        {
            _bonusRepositoryScreen.GetCard<BonusRepositoryCardTemporary>(bonus.CardID).UpdateTimer(bonus.ActionTime, remainingTime);
        }

        private void OnTimeIsUp(TemporaryBonus bonus)
        {
            GameManager.TicketsBankController.AddTickets(bonus.Revenue, this);
            GameManager.BonusRepositoryController.RemoveBonus(bonus);
        }
    }
}
