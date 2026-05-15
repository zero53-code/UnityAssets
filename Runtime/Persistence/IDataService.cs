using System.Collections.Generic;
using UnityEngine;

namespace Zero53.Persistence
{
    public interface IDataService
    {
        /// <summary>
        /// 保存存档
        /// </summary>
        /// <param name="data">游戏数据</param>
        /// <param name="name">存档名</param>
        /// <param name="overwrite">是否覆盖</param>
        void Save<TData>(TData data, string name, bool overwrite = true) where TData : ISavable;

        /// <summary>
        /// 加载存档
        /// </summary>
        /// <param name="name">存档名</param>
        /// <returns></returns>
        TData Load<TData>(string name) where TData : ISavable;
        
        /// <summary>
        /// 删除存档
        /// </summary>
        /// <param name="name">存档名</param>
        void Delete(string name);
        
        /// <summary>
        /// 删除所有存档
        /// </summary>
        void DeleteAll();
        
        /// <summary>
        /// 获取所有存档名
        /// </summary>
        /// <returns>所有存档名</returns>
        IEnumerable<string> ListSaves();
    }
}