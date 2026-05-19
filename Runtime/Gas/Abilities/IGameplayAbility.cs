using JetBrains.Annotations;

namespace Zero53.Gas.Abilities
{
    public interface IGameplayAbility
    {
        /// <summary>
        /// 执行技能
        /// </summary>
        void Execute();

        /// <summary>
        /// 技能是否正在执行
        /// </summary>
        bool isExecuting { get; }
        
        /// <summary>
        /// 获取技能时调用
        /// </summary>
        void OnGive(AbilitySystem abilitySystem) {}
        
        /// <summary>
        /// 移除技能时调用
        /// </summary>
        void OnRemove() {}
        
        /// <summary>
        /// 执行技能前调用
        /// </summary>
        void OnPreExecute() {}
        
        /// <summary>
        /// 技能被取消执行时调用
        /// </summary>
        void OnCancel() {}
        
        /// <summary>
        /// 技能结束、取消、打断时调用
        /// </summary>
        void OnEnd() {}
    }
}