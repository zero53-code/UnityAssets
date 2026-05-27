namespace Zero53.Gas.AttributeSet.Processor
{
    public interface IChangeProcessor
    {
        void Process(GameplayAttributeSet attributeSet, Name attributeName, ref float value);
    }
}