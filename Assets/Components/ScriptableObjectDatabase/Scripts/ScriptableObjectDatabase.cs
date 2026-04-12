using System;
using System.Collections.Generic;
using System.Linq;
using Components.AudioSystem;
using Components.Skills;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Components
{
    public static class ScriptableObjectDatabase
    {
        private static readonly Dictionary<Type, Dictionary<string, Object>> Database = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            Database.Clear();

            Register<AudioSO>();
            Register<Skill>();
        }

        private static void Register<T>() where T : Object
        {
            var type = typeof(T);

            if (Database.ContainsKey(type))
            {
                Debug.LogWarning($"Trying to register duplicate type {type}");
                return;
            }

            Database[type] = new Dictionary<string, Object>();

            T[] templates = Resources.LoadAll<T>("");
            foreach (var template in templates)
            {
                Database[type][template.name] = template;
            }

            Debug.Log($"Registered new type {type}");
        }

        /// <summary>
        /// Return a Scriptable Object from the database by its name and type
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>(string name) where T : Object
        {
            var type = typeof(T);

            if (Database.TryGetValue(type, out var dictionary))
            {
                if (dictionary.TryGetValue(name, out var obj))
                {
                    return obj as T;
                }
            }

            Debug.LogError("Unable to find a scriptable object with name " + name + " of type " + type);
            return null;
        }

        public static List<T> GetAll<T>() where T : Object
        {
            var result = new List<T>();

            var type = typeof(T);

            if (Database.TryGetValue(type, out var dictionary))
            {
                // Equivalent of a foreach loop
                result.AddRange(dictionary.Values.Select(obj => obj as T));
            }
            return result;
        }
    }
}
