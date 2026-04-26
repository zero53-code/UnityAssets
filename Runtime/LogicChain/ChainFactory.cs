namespace Zero53.LogicChain
{
    /// <summary>
    /// 用于创建 Chain 实例的工厂委托类型
    /// </summary>
    public delegate TChain ChainFactory<out TIn, in TOut, out TChain>(IProcessor<TIn, TOut> processor)
        where TChain : FluentChain<TIn, TOut, TChain>;
}