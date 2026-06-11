using System;

namespace Zero53.Gas
{
    [Serializable]
    public abstract class GameplayCue
    {
        protected internal virtual void OnStart() 
        {}
        
        protected internal abstract void OnUpdate(float deltaTime);
        
        protected internal virtual void OnRemove()
        {}
    }
}