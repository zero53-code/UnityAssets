namespace Zero53.LogicChain
{
    public class Chain<TIn, TOut>
    {
        private readonly IProcessor<TIn, TOut> _processor;

        private Chain(IProcessor<TIn, TOut> processor)
        {
            _processor = processor;
        }

        /// <summary>
        /// 静态工厂方法
        /// </summary>
        /// <param name="processor">处理器对象</param>
        /// <returns>Chain 对象</returns>
        public static Chain<TIn, TOut> Start(IProcessor<TIn, TOut> processor)
        {
            return new Chain<TIn, TOut>(processor);
        }

        public Chain<TIn, TNext> Then<TNext>(IProcessor<TOut, TNext> next)
        {
            var combined = new Combined<TIn, TOut, TNext>(_processor, next);
            return new Chain<TIn, TNext>(combined);
        }
        
        public TOut Run(TIn input) => _processor.Process(input);

        /// <summary>
        /// 编译为委托类型
        /// </summary>
        /// <returns>委托对象</returns>
        public ProcessorDelegate<TIn, TOut> Compile()
        {
            return input => _processor.Process(input);
        }
    }
}