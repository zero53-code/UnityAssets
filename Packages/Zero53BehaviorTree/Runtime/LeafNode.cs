
namespace Zero53.BehaviorTree
{
    /// <summary>
    /// 异常处理方式
    /// </summary>
    public enum ExceptionHandling
    {
        /// <summary>
        /// 直接抛出异常
        /// </summary>
        Throw,
        /// <summary>
        /// 返回 Success
        /// </summary>
        ReturnSuccess,
        /// <summary>
        /// 返回 Failure
        /// </summary>
        ReturnFailure,
    }

    /// <summary>
    /// 叶子节点
    /// </summary>
    public class LeafNode : Node
    {
        /// <summary>
        /// 异常处理方式
        /// </summary>
        public ExceptionHandling exceptionHandling { get; set; } = ExceptionHandling.Throw;

        protected LeafNode(string name = "Node", int priority = 0) : base(name, priority)
        {
        }
    }
}