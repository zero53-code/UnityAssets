namespace Zero53.LogicChain
{
    public delegate TOut ProcessorDelegate<in TIn, out TOut>(TIn input);
}