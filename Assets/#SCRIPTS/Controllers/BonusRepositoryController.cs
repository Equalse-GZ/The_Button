using Game.Bonuses;
using Game.Core;
using Game.Data;
using Game.Handlers;
using Game.UI;
using Game.Web;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Controllers
{
    public class BonusRepositoryController : MonoBehaviour
    {
        public UnityEvent InitializedEvent = new UnityEvent();

        [Header("Handlers")]
        [SerializeField] private TemporaryBonusesHandler _temporaryBonusesHandler;
        [SerializeField] private ConstantBonusesHandler _constantBonusesHandler;
        [SerializeField] private RarityBonusesHandler _rarityBonusesHandler;

        [Header("Stock Bonuses")]
        [SerializeField] private List<ConstantBonus> _stockConstantBonuses = new List<ConstantBonus>();
        [SerializeField] private List<TemporaryBonus> _stockTemporaryBonuses = new List<TemporaryBonus>();
        [SerializeField] private List<RarityBonus> _stockRarityBonuses = new List<RarityBonus>();

        private List<BonusData> _temporaryBonusesData = new List<BonusData>();
        private List<BonusData> _constantBonusesData = new List<BonusData>();

        private string _dataBaseURL;

        public void Initialize(string dataBaseURL)
        {
            _dataBaseURL = dataBaseURL;
            _rarityBonusesHandler.Initialize(_stockRarityBonuses);
            //SyncController.DataRecievedEvent.AddListener(OnSync);
            LoadBonusesFromServer();
        }

        public void AddBonus(IBonus bonus)
        {
            if (bonus is TemporaryBonus)
            {
                var m_bonus = Instantiate((TemporaryBonus)bonus);
                _temporaryBonusesHandler.AddBonus(m_bonus, 0, true);
            }

            if (bonus is ConstantBonus)
            {
                var m_bonus = Instantiate((ConstantBonus)bonus);
                _constantBonusesHandler.AddBonus(m_bonus, true);
            }
        }

        public void RemoveBonus(IBonus bonus)
        {
            if (bonus is TemporaryBonus)
            {
                if (_temporaryBonusesHandler.Bonuses.Contains((TemporaryBonus)bonus) == false)
                    return;

                _temporaryBonusesHandler.RemoveBonus((TemporaryBonus)bonus);
            }
        }

        public void RemoveAllBonuses(IBonus bonus)
        {
            if (bonus is TemporaryBonus)
                _temporaryBonusesHandler.RemoveAllBonuses((TemporaryBonus)bonus);
        }

        public bool ContainsBonus(IBonus bonus)
        {
            if (bonus is TemporaryBonus) return _temporaryBonusesHandler.Bonuses.Contains((TemporaryBonus)bonus);
            else return _constantBonusesHandler.ContainsBonus<ConstantDisposableBonus>((ConstantBonus)bonus);
        }

        public void Disable()
        {
            _temporaryBonusesHandler.Disable();
            _constantBonusesHandler.Disable();
            _rarityBonusesHandler.Disable();

            _temporaryBonusesData.Clear();
            _constantBonusesData.Clear();

            GameManager.UserInterface.GetScreen<BonusRepositoryScreen>().Disable();
        }

        /*private void OnSync(GlobalData data)
        {
            Disable();
            BonusesData bonusesData = new BonusesData();
            bonusesData.Bonuses = data.Bonuses;
            InitializeBonuses(bonusesData, WebOperationStatus.Succesfull);
        }*/

        private void LoadBonusesFromServer()
        {
            WWWForm form = new WWWForm();

            form.AddField("Type", "BonusGetAll");
            form.AddField("UserID", GameManager.UserData.ID);

            GameManager.WebRequestSender.GetBonusesData(GameManager.Config.DataBaseUrl, form, InitializeBonuses);
        }
        
        private void InitializeBonuses(BonusesData data, WebOperationStatus status)
        {
            foreach (var bonus in data.Bonuses)
            {
                if (bonus.Type == "CONSTANT") 
                    _constantBonusesData.Add(bonus);
                else 
                    _temporaryBonusesData.Add(bonus);
            }

            _constantBonusesHandler.Initialize(_stockConstantBonuses, _constantBonusesData);
            _temporaryBonusesHandler.Initialize(_stockTemporaryBonuses, _temporaryBonusesData);

            //Event
            InitializedEvent.Invoke();
            InitializedEvent.RemoveAllListeners();
        }
    }
}
