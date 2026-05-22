namespace Zero53.Gas.Attributes.Processor
{
    public interface IChangeProcessor
    {
        void Process(GameplayAttributeSet attributeSet, Name attributeName, ref float value);
    }
}