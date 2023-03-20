namespace Game.Data
{
    [System.Serializable]
    public class TeamMemberData : IData
    {
        public int TeamID;
        public TeamRole Role;
    }

    public enum TeamRole
    {
        Admin,
        Member
    }
}
