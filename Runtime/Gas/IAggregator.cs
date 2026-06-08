using System.Collections.Generic;

namespace Zero53.Gas
{
    public interface IAggregator
    {
        float Aggregate(float baseValue, IList<Modifier> modifiers);
    }
}