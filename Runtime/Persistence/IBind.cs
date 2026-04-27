namespace Zero53.Persistence
{
    public interface IBind<in TData> where TData : ISavable
    {
        SerializableGuid id { get; set; }
        void Bind(TData data);
    }
}