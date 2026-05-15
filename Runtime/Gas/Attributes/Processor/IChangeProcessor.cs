namespace Zero53.Gas.Attributes.Processor
{
    public interface IChangeProcessor
    {
        void Process(GameplayAttribute attribute, ref float value);
    }
}