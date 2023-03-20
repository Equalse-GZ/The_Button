namespace Game.Data
{
    [System.Serializable]
    public class BonusData : IData
    {
        public int ID;
        public string Type;
        public int AppearanceTime;
        public int Count;
    }
}
