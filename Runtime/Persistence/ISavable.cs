namespace Zero53.Persistence
{
    public interface ISavable
    {
        SerializableGuid id { get; set; }
    }
}