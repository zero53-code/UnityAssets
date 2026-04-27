using System.Text;
using UnityEngine;

namespace Zero53.Persistence
{
    public class JsonSerializer : ISerializer
    {
        public byte[] Serialize<T>(T obj)
        {
#if UNITY_EDITOR
            var data = JsonUtility.ToJson(obj, true);
#else
            var data = JsonUtility.ToJson(obj, false);
#endif

            return Encoding.GetEncoding("UTF-8").GetBytes(data);
        }

        public T Deserialize<T>(byte[] data)
        {
            return JsonUtility.FromJson<T>(Encoding.GetEncoding("UTF-8").GetString(data));
        }
    }
}