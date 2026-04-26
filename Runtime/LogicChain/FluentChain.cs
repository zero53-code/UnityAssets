using System;

namespace Zero53.LogicChain
{
    public abstract class FluentChain<TIn, TOut, TDerived>
        where TDerived : FluentChain<TIn, TOut, TDerived>
    {
        private readonly IProcessor<TIn, TOut> _processor;
        
        protected FluentChain(IProcessor<TIn, TOut> processor)
        {
            this._processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        protected TNextSelf Then<TNext, TNextSelf, TProcessor>(
            TProcessor nextProcessor,
            ChainFactory<TIn, TNext, TNextSelf> factory)
            where TNextSelf : FluentChain<TIn, TNext, TNextSelf>
            where TProcessor : class, IProcessor<TOut, TNext>
        {
            if (nextProcessor == null) throw new ArgumentNullException(nameof(nextProcessor));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            
            return factory(new Combined<TIn,TOut,TNext>(_processor, nextProcessor));
        }

        public TOut Run(TIn input)
        {
            if (_processor == null) throw new InvalidOperationException("Processor is not initialized. Use Chain.Start() to begin a chain.");
            return _processor.Process(input);
        }

        public ProcessorDelegate<TIn, TOut> Compile()
        {
            if (_processor == null) throw new InvalidOperationException("Processor is not initialized. Use Chain.Start() to start a chain.");
            return input => _processor.Process(input);
        }
    }
}