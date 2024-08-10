using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Batmind.Board
{
    [Serializable]
    public class Blackboard
    {
        private Dictionary<string, BlackboardKey> _keyRegistry = new();

        [SerializeField]
        private List<BlackboardEntry> _entries = new();

        [JsonIgnore]
        public List<Action> PassedActions { get; } = new();

        public void AddAction(Action action)
        {
            PassedActions.Add(action);
        }

        public void ClearActions() => PassedActions.Clear();

        // public void Debug()
        // {
        //     foreach (var entry in _entries)
        //     {
        //         var entryType = entry.Value.GetType();
        //
        //         if (entryType.IsGenericType && entryType.GetGenericTypeDefinition() == typeof(BlackboardEntry<>))
        //         {
        //             var valueProperty = entryType.GetProperty("Value");
        //             if (valueProperty == null) continue;
        //             var value = valueProperty.GetValue(entry.Value);
        //             UnityEngine.Debug.Log($"Key: {entry.Key}, Value: {value}");
        //         }
        //     }
        // }

        public bool TryGetValue<T>(BlackboardKey key, out T value)
        {
            var entry = _entries.FirstOrDefault(e => e is BlackboardEntry<T> castedEntry && castedEntry.Key == key) as BlackboardEntry<T>;
            
            if (entry != null && entry.Key == key)
            {
                value = default;
                return true;
            }

            value = default;
            return false;
        }
        
        public bool TryGetEntry(BlackboardKey key, out BlackboardEntry entry)
        {
            entry = _entries.FirstOrDefault(e => e is BlackboardEntry castedEntry && castedEntry.Key == key);
            
            return entry != null && entry.Key == key;
        }

        public void SetValue(BlackboardKey key, BlackboardEntry value)
        {
            var entry = _entries.FirstOrDefault(e => e.Key == key);
            
            if (entry != null)
            {
                entry.ValueType = value.ValueType;
                entry.Value = value.Value;
                
                return;
            }

            value.Key = key;
            
            _entries.Add(value);
        }

        public void SetValue(BlackboardKey key, AnyValue value)
        {
            var entry = _entries.FirstOrDefault(e => e is BlackboardEntry castedEntry && castedEntry.Key == key);
            if (entry != null)
            {
                entry.Value = value;
                return;
            }

            // _entries.Add(value);
        }

        public BlackboardKey GetOrRegisterKey(string keyName)
        {
            if (!_keyRegistry.TryGetValue(keyName, out BlackboardKey key))
            {
                key = new BlackboardKey(keyName);
                _keyRegistry[keyName] = key;
            }

            return key;
        }

        public void Remove(BlackboardKey key)
        {
            var entry = _entries.FirstOrDefault(e => e.Key == key);
            _entries.Remove(entry);
        }

        public bool ContainsKeyWithName(string keyName)
        {
            return _entries.Any(e => string.Equals(e.Key.name, keyName));
        }
        
        public bool ContainsKeyWithHash(int hashedKey)
        {
            return _entries.Any(e => string.Equals(e.Key.hashedKey, hashedKey));
        }
        
        public bool ContainsKeyWithHash(int hashedKey, out BlackboardKey key)
        {
            key = _entries.FirstOrDefault(e => string.Equals(e.Key.hashedKey, hashedKey))?.Key;
            
            return key != null;
        }
        
        public void GetAllEntriesWithValueTypeNonAlloc(AnyValue.ValueType valueType, List<BlackboardEntry> entries)
        {
            foreach (var entry in _entries)
            {
                if (entry.ValueType == valueType)
                {
                    entries.Add(entry);
                }
            }
        }
        
        public void ClearEntries()
        {
            _entries.Clear();
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            foreach (var entry in _entries)
            {
                entry.OnValidate();
            }
        }

#endif
    }
}