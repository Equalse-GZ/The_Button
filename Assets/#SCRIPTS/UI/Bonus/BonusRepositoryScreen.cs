using Game.Bonuses;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class BonusRepositoryScreen : ScreenUI
    {
        [SerializeField] private BonusRepositoryCardReusable _cardReusableTemplate;
        [SerializeField] private BonusRepositoryCardTemporary _cardTemporaryTemplate;
        [SerializeField] private Transform _content;

        Dictionary<int, BonusRepositoryCard> _cards = new Dictionary<int, BonusRepositoryCard>();

        public T AddCard<T>(IBonus bonus) where T : BonusRepositoryCard
        {
            if (typeof(T) == typeof(BonusRepositoryCardReusable)) return (T)CreateCard(_cardReusableTemplate, bonus);
            else return (T)CreateCard(_cardTemporaryTemplate, bonus);
        }

        public void DeleteCard(int id)
        {
            if (_cards.ContainsKey(id) == false)
                throw new System.Exception("There is no card with this ID");

            Destroy(_cards[id].gameObject);
            _cards.Remove(id);
        }

        public T GetCard<T>(int id) where T : BonusRepositoryCard
        {
            if (_cards.ContainsKey(id) == false)
                throw new System.Exception("There is no card with this ID");

            return (T)_cards[id];
        }

        public T GetCard<T>(IBonus bonus) where T : BonusRepositoryCard
        {
            foreach (var card in _cards.Values)
            {
                if (card.Owner.ID == bonus.ID)
                    return (T)card;
            }

            throw new System.Exception("The card does not exist");
        }

        public bool ContainsCard(IBonus bonus)
        {
            foreach (var card in _cards.Values)
            {
                if(card.Owner.ID == bonus.ID)
                    return true;
            }

            return false;
        }

        public void Disable()
        {
            foreach (var card in _cards.Values)
                Destroy(card.gameObject);

            _cards.Clear();
        }

        private BonusRepositoryCard CreateCard(BonusRepositoryCard template, IBonus bonus)
        {
            int id = 0;
            while (_cards.ContainsKey(id))
                id++;

            BonusRepositoryCard card = Instantiate(template, _content);
            card.transform.SetAsFirstSibling();
            card.ID = id;
            card.UpdateTotalInfo(bonus);

            _cards[id] = card;
            return card;
        }
    }
}
