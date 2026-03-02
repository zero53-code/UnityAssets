using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.Events;
#endif

namespace Zero53.ObserverPattern
{
    /// <summary>
    /// 观察者类
    /// </summary>
    [Serializable]
    public class ValueObserver<T>
    {
        [SerializeField] private T value;
        [SerializeField] private UnityEvent<T> onValueChanged;

        public ValueObserver(T value, UnityAction<T> callback = null)
        {
            this.value = value;
            onValueChanged = new UnityEvent<T>();
            if (callback != null) onValueChanged.AddListener(callback);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public T GetValue() => value;
        
        /// <summary>
        /// 设置值
        /// 当新值与旧值不同时触发 onValueChanged 事件
        /// </summary>
        /// <param name="newValue">新值</param>
        public void SetValue(T newValue)
        {
            if (Equals(value, newValue)) return;
            value = newValue;
            Invoke();
        }

        /// <summary>
        /// 调用事件回调
        /// </summary>
        public void Invoke()
        {
            onValueChanged?.Invoke(value);
        }
        
        /// <summary>
        /// 添加 onValueChanged 事件回调
        /// </summary>
        /// <param name="callback">回调</param>
        public void AddListener(UnityAction<T> callback)
        {
            if (callback == null) return;
            onValueChanged ??= new UnityEvent<T>();

#if UNITY_EDITOR
            UnityEventTools.AddPersistentListener(onValueChanged, callback);
#else
            onValueChanged.AddListener(callback);
#endif
        }

        /// <summary>
        /// 移除 onValueChanged 事件回调
        /// </summary>
        /// <param name="callback">回调</param>
        public void RemoveListener(UnityAction<T> callback)
        {
            if (callback == null) return;

#if UNITY_EDITOR
            UnityEventTools.RemovePersistentListener(onValueChanged, callback);
#else
            onValueChanged?.RemoveListener(callback);
#endif
        }

        /// <summary>
        /// 移除 onValueChanged 中所有的事件回调
        /// </summary>
        public void RemoveAllListeners()
        {
            if (onValueChanged == null) return;
#if UNITY_EDITOR
            typeof(UnityEventBase)
                !.GetField("m_PersistentCalls", BindingFlags.Instance | BindingFlags.NonPublic)
                !.GetValue(onValueChanged)
                !.GetType()
                !.GetMethod("Clear")
                !.Invoke(onValueChanged, null);
#else    
            onValueChanged.RemoveAllListeners();
#endif
        }

        public void Dispose()
        {
            RemoveAllListeners();
            value = default;
            onValueChanged = null;
        }
    }
}