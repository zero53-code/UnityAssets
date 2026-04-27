using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Zero53.Persistence
{
    [Serializable]
    public class FileDataService<TData> : IDataService<TData>
        where TData : IName, ISavable
    {
        [SerializeReference] public ISerializer serializer;
        [SerializeField] public string dataPath;
        [SerializeField] public string fileExtension = "save";
        
        private string GetPathToFile(string filename)
        {
            return Path.Combine(dataPath, string.Concat(filename, ".", fileExtension));
        }
        
        public void Save(TData data, bool overwrite = true)
        {
            var fileLocation = GetPathToFile(data.name);

            if (!overwrite && File.Exists(fileLocation))
            {
                throw new IOException($"The file '{data.name}.{fileExtension}' already exists and cannot be overwritten.'");
            }
            
            File.WriteAllBytes(fileLocation, serializer.Serialize(data));
        }

        public TData Load(string name)
        {
            var fileLocation = GetPathToFile(name);
            
            return serializer.Deserialize<TData>(File.ReadAllBytes(fileLocation));
        }

        public void Delete(string name)
        {
            var fileLocation = GetPathToFile(name);
            if (File.Exists(fileLocation)) File.Delete(fileLocation);
        }

        public void DeleteAll()
        {
            foreach (var filePath in Directory.GetFiles(dataPath))
            {
                if (Path.GetExtension(filePath) == fileExtension)
                {
                    File.Delete(filePath);
                }
            }
        }

        public IEnumerable<string> ListSaves()
        {
            return Directory.GetFiles(dataPath)
                .Where(filePath => Path.GetExtension(filePath) == fileExtension)
                .Select(Path.GetFileNameWithoutExtension);
        }
    }
}