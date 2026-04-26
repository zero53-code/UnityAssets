namespace Zero53.LogicChain
{
    public class Combined<TA, TB, TC> : IProcessor<TA, TC>
    {
        private readonly IProcessor<TA, TB> _first;
        private readonly IProcessor<TB, TC> _second;

        public Combined(IProcessor<TA, TB> first, IProcessor<TB, TC> second)
        {
            _first = first;
            _second = second;
        }

        public TC Process(TA input)
        {
            return _second.Process(_first.Process(input));
        }
    }
}