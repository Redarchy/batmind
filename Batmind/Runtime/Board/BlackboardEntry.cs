using System;

namespace Batmind.Board
{
    [Serializable]
    public class BlackboardEntry<T> : BlackboardEntry
    {
        public override bool Equals(object obj) => obj is BlackboardEntry<T> other && other.Key == Key;
        public override int GetHashCode() => Key.GetHashCode();

    }

    [Serializable]
    public class BlackboardEntry
    {
        public BlackboardKey Key;
        public AnyValue.ValueType ValueType;
        public AnyValue Value;

#if UNITY_EDITOR
        public void OnValidate()
        {
            Key.OnValidate();
        }
#endif
    }
}