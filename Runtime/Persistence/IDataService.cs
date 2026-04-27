using System.Collections.Generic;
using UnityEngine;

namespace Zero53.Persistence
{
    public interface IDataService<TData>
        where TData : ISavable
    {
        /// <summary>
        /// 保存存档
        /// </summary>
        /// <param name="data"></param>
        /// <param name="overwrite">是否覆盖</param>
        void Save(TData data, bool overwrite = true);
        
        /// <summary>
        /// 加载存档
        /// </summary>
        /// <param name="name">存档名</param>
        /// <returns></returns>
        TData Load(string name);
        
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