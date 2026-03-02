namespace Zero53.AbilitySystem
{
    public interface IProcess
    {
        void OnUpdate(float deltaTime);
    }

    public interface IProcessEnd
    {
        bool isEnd { get; }
    }
    
    public interface IProcessEndEvent
    {
        void OnEnd();
    }
}