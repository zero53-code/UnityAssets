namespace Zero53.Persistence
{
    public interface IBinding
    {
        SerializableGuid id { get; }
        void OnSave(ISavable data);
        void OnLoad(ISavable data);
    }
}