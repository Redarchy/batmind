using System;
using Batmind.Utils;

namespace Batmind.Board
{
    [Serializable]
    public class BlackboardKey : IEquatable<BlackboardKey>
    {
        public string name;
        public int hashedKey;

        public BlackboardKey(string name)
        {
            this.name = name;
            hashedKey = name.ComputeFNV1aHash();
        }

        public bool Equals(BlackboardKey other) => hashedKey == other.hashedKey;

        public override bool Equals(object obj)
        {
            if (obj is string keyAsString)
            {
                return string.Equals(name, keyAsString);
            }
            
            return obj is BlackboardKey other && Equals(other);
        }

        public override int GetHashCode() => hashedKey;
        public override string ToString() => name;
        
#if UNITY_EDITOR
        public void OnValidate()
        {
            hashedKey = name.ComputeFNV1aHash();
        }
#endif
        
    }
}