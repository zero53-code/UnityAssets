namespace Zero53.Persistence
{
    public class GameData : ISavable
    {
        public string name;
        public SerializableGuid id { get; set; }
    }
}