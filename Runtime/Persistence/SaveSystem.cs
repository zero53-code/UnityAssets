using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.Singletons;
using Zero53.Utils;

namespace Zero53.Persistence
{
    public interface IGameSaveData : ISavable, IBind, IEnumerable<ISavable>
    {
        
    }
    
    public abstract class SaveSystem<TSelf, TGameData> : MonoSingleton<SaveSystem<TSelf, TGameData>>
        where TSelf : SaveSystem<TSelf, TGameData>
        where TGameData : IGameSaveData, new()
    {
        [SerializeField] protected TGameData gameData;
        [SerializeReference] protected IDataService dataService;
        
        private readonly List<(IBinding binding, ISavable savable)> _bindings = new();
        private readonly Dictionary<SerializableGuid, ISavable> _guidToData = new();
        private readonly Dictionary<SerializableGuid, IBinding> _guidToBinding = new();

        public void Bind<T, TData>(TData data = default)
            where T : Object, IBinding
            where TData : ISavable, new()
        {
            var obj = FindObjectsByType<T>(FindObjectsSortMode.None).First();
            if (obj == null)
            {
                Debug.LogError($"No object of type {typeof(T).Name}");
                return;
            }
            Bind(obj, data);
        }

        public void Bind<T, TData>(T obj, TData data = default)
            where T : IBinding
            where TData : ISavable, new()
        {
            data ??= new TData();
            data.id = obj.id;
            _bindings.Add((obj, data));
            _guidToBinding[obj.id] = obj;
            _guidToData[data.id] = data;
        }

        public void Unbind<T, TData>(T obj)
            where T : IBinding
            where TData : ISavable, new()
        {
            if (obj == null) return;
            
            _bindings.RemoveAll(tuple => tuple.binding.Equals(obj));
            _guidToData.Remove(obj.id);
            _guidToBinding.Remove(obj.id);
        }

        public T GetData<T>(SerializableGuid id)
            where T : ISavable
        {
            if (_guidToData.TryGetValue(id, out var data) && data is T tData)
            {
                return tData;
            }

            return default;
        }

        public T GetBinding<T>(SerializableGuid id)
            where T : IBinding
        {
            if (_guidToBinding.TryGetValue(id, out var binding) && binding is T tBinding)
            {
                return tBinding;
            }
            
            return default;
        }
        
        [Button, HorizontalGroup("Line 1")]
        public virtual void NewGame()
        {
            gameData = new TGameData();
        }

        [Button, HorizontalGroup("Line 1")]
        public void Save(string gameSaveName)
        {
            RemoveNullBindings();
            foreach (var (binding, bindingData) in _bindings)
            {
                binding.OnSave(bindingData);
            }
            
            dataService.Save(gameData, gameSaveName);
        }

        [Button, HorizontalGroup("Line 2")]
        public void Load(string gameSaveName)
        {
            var loadData = dataService.Load<TGameData>(gameSaveName);
            if (loadData == null) return;

            gameData = loadData;
            
            _bindings.Clear();
            _guidToBinding.Clear();
            _guidToData.Clear();
            
            gameData.Bind();
            foreach (var savable in gameData)
            {
                _guidToData[savable.id] = savable;
            }
            
            foreach (var (binding, bindingData) in _bindings)
            {
                print($"Loading: {binding}, {bindingData}");
                binding.OnLoad(bindingData);
            }
        }

        [Button, HorizontalGroup("Line 2")]
        public virtual void Delete(string gameSaveName)
        {
            dataService.Delete(gameSaveName);
        }
        
        private void RemoveNullBindings()
        {
            _bindings.RemoveAll(b => b.binding.IsNull());
        }
    }
}