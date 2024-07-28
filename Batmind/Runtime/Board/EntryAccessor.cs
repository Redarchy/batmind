using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Batmind.Board
{
    [Serializable]
    public class EntryAccessor<TValue>
    {
        [SerializeField]
        [HideInInspector]
        public int EntryKeyHash;
        
        [NonSerialized]
        [JsonIgnore]
        public BlackboardEntry RuntimeEntry;

        [JsonIgnore]
        public AnyValue Value => RuntimeEntry.Value;
        
    }
}