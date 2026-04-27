using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zero53.Singletons;

namespace Zero53.Persistence
{
    public abstract class SavaLoadSystemBase<TGameData, TDataService> : PersistenceSingleton<SavaLoadSystemBase<TGameData, TDataService>> 
        where TGameData : IName, ISavable, new()
        where TDataService : class, IDataService<TGameData>
    {
        [SerializeField] public TGameData gameData;
        [SerializeField] public TDataService dataService;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        protected void Bind<T, TData>(TData data)
            where T : UnityEngine.Object, IBind<TData>
            where TData : ISavable, new()
        {
            var entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();
            
            if (entity == null) return;

            entity.Bind(data ?? new TData { id = entity.id });
        }

        protected void Bind<T, TData>(List<TData> dataList)
            where T : UnityEngine.Object, IBind<TData>
            where TData : ISavable, new()
        {
            var entities = FindObjectsByType<T>(FindObjectsSortMode.None);

            foreach (var entity in entities)
            {
                var data = dataList.FirstOrDefault(data => data.id == entity.id);
                data ??= new TData { id = entity.id };
                dataList.Add(data);
                entity.Bind(data);
            }
        }

        public abstract void NewGame();

        public abstract void SaveGame();

        public abstract void LoadGame(string gameName);

        public abstract void DeleteGame(string gameName);

        public abstract void OnSceneLoaded(Scene scene, LoadSceneMode mode);
    }
}