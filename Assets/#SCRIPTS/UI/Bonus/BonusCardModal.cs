using Game.Core;
using Game.Bonuses;
using Game.Services;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Game.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class BonusCardModal : MonoBehaviour
    {
        protected UnityEvent BonusBoughtSuccessfull = new UnityEvent();

        [SerializeField] private Image _icon;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _priceText;

        [Space(10)]
        [SerializeField] private Image _headerBG;
        [SerializeField] private Image _iconBG;
        [SerializeField] private Image _footerBG;

        [Header("Sounds")]
        [SerializeField] private AudioClip _transactionSuccessSound;
        [SerializeField] private List<AudioClip> _transactionFailedSounds = new List<AudioClip>();

        [SerializeField] private float _transactionSuccessSoundVolume = 0.8f;
        [SerializeField] private float _transactionFailedSoundVolume = 0.8f;

        protected IBonus bonus;
        private AudioSource _audioSource;

        protected void UpdateBaseInfo(IBonus bonus)
        {
            if(bonus.Icon != null)
                _icon.sprite = bonus.Icon;

            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();

            this.bonus = bonus;
            _icon.color = bonus.CardColor;

            _headerBG.color = new Color(bonus.CardColor.r, bonus.CardColor.g, bonus.CardColor.b, _headerBG.color.a);
            _iconBG.color = new Color(bonus.CardColor.r, bonus.CardColor.g, bonus.CardColor.b, _iconBG.color.a);
            _footerBG.color = new Color(bonus.CardColor.r, bonus.CardColor.g, bonus.CardColor.b, _footerBG.color.a);

            _nameText.color = new Color(bonus.CardColor.r, bonus.CardColor.g, bonus.CardColor.b, _nameText.color.a);
            _nameText.text = bonus.Name;
            _priceText.color = new Color(bonus.CardColor.r, bonus.CardColor.g, bonus.CardColor.b, _priceText.color.a);
            _priceText.text = $"÷ена: {NumberConverter.NumToString(bonus.Price)}" + " б";
        }

        public virtual void BuyBonus()
        {
            if (GameManager.TicketsBankController.IsEnoughTickets((uint)bonus.Price) == true)
            {
                GameManager.TicketsBankController.SpendTickets((uint)bonus.Price, this);
                GameManager.BonusRepositoryController.AddBonus(bonus);

                _audioSource.clip = _transactionSuccessSound;
                _audioSource.pitch = Random.Range(0.95f, 1.1f);
                _audioSource.volume = _transactionSuccessSoundVolume;
                _audioSource.Play();
            }
            else
            {
                _audioSource.clip = _transactionFailedSounds[Random.Range(0, _transactionFailedSounds.Count)];
                _audioSource.pitch = Random.Range(0.95f, 1.1f);
                _audioSource.volume = _transactionFailedSoundVolume;
                _audioSource.Play();
            }
        }
    }
}
