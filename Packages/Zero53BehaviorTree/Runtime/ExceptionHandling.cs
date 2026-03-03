
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
        Throw = 0,
        /// <summary>
        /// 返回 Success
        /// </summary>
        ReturnSuccess = 1,
        /// <summary>
        /// 返回 Failure
        /// </summary>
        ReturnFailure = 2,
    }
}