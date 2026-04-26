namespace Zero53.Gas
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