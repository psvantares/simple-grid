using System;
using UnityEngine;

namespace Game.Utilities
{
    public static class Storage
    {
        public static T Load<T>(string key)
        {
            var jsonData = PlayerPrefs.GetString(key);

            if (string.IsNullOrEmpty(jsonData)) return default;

            try
            {
                return JsonUtility.FromJson<T>(jsonData);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Can't load {nameof(T)} from key \"{key}\": {exception.Message}");
            }

            return default;
        }

        public static void Save(object data, string key)
        {
            var jsonData = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, jsonData);
            PlayerPrefs.Save();
        }
    }
}