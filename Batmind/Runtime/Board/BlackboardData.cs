using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace Batmind.Board
{
    [CreateAssetMenu(fileName = "New Blackboard Data", menuName = "Blackboard/Blackboard Data")]
    public class BlackboardData : ScriptableObject
    {
        public List<BlackboardEntryData> entries = new();

        public void SetValuesOnBlackboard(Blackboard blackboard)
        {
            foreach (var entry in entries)
            {
                entry.SetValueOnBlackboard(blackboard);
            }
        }
    }

    [Serializable]
    public class BlackboardEntryData : ISerializationCallbackReceiver
    {
        public string keyName;
        public AnyValue.ValueType valueType;
        public AnyValue value;

        public void SetValueOnBlackboard(Blackboard blackboard)
        {
            var key = blackboard.GetOrRegisterKey(keyName);
            setValueDispatchTable[value.type](blackboard, key, value);
        }

        // Dispatch table to set different types of value on the blackboard
        static Dictionary<AnyValue.ValueType, Action<Blackboard, BlackboardKey, AnyValue>> setValueDispatchTable = new()
        {
            {AnyValue.ValueType.Int, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue)},
            {AnyValue.ValueType.Float, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue)},
            {AnyValue.ValueType.Bool, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue)},
            {AnyValue.ValueType.String, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue)},
            {AnyValue.ValueType.Vector3, (blackboard, key, anyValue) => blackboard.SetValue(key, anyValue)},
        };

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize() => value.type = valueType;
    }

    [Serializable]
    public class AnyValue
    {
        public enum ValueType
        {
            Int,
            Float,
            Bool,
            String,
            Vector3
        }

        public ValueType type;

        // Storage for different types of values
        public int IntValue;
        public float FloatValue;
        public bool BoolValue;
        public string StringValue;
        public Vector3 Vector3Value;
        // Add more types as needed, but remember to add them to the dispatch table above and the custom Editor

        // Implicit conversion operators to convert AnyValue to different types
        public static implicit operator int(AnyValue value) => value.ConvertValue<int>();
        public static implicit operator float(AnyValue value) => value.ConvertValue<float>();
        public static implicit operator bool(AnyValue value) => value.ConvertValue<bool>();
        public static implicit operator string(AnyValue value) => value.ConvertValue<string>();
        public static implicit operator Vector3(AnyValue value) => value.ConvertValue<Vector3>();

        T ConvertValue<T>()
        {
            return type switch
            {
                ValueType.Int => AsInt<T>(IntValue),
                ValueType.Float => AsFloat<T>(FloatValue),
                ValueType.Bool => AsBool<T>(BoolValue),
                ValueType.String => (T) (object) StringValue,
                ValueType.Vector3 => AsVector3<T>(Vector3Value),
                _ => throw new NotSupportedException($"Not supported value type: {typeof(T)}")
            };
        }

        // Helper methods for safe type conversions of the value types without the cost of boxing
        T AsBool<T>(bool value) => typeof(T) == typeof(bool) && value is T correctType ? correctType : default;
        T AsInt<T>(int value) => typeof(T) == typeof(int) && value is T correctType ? correctType : default;
        T AsFloat<T>(float value) => typeof(T) == typeof(float) && value is T correctType ? correctType : default;
        T AsVector3<T>(Vector3 value) => typeof(T) == typeof(Vector3) && value is T correctType ? correctType : default;

        public static ValueType FromExplicitType(Type explicitType)
        {
            switch (explicitType)
            {
                case Type t when t == typeof(int):
                    return ValueType.Int;
                case Type t when t == typeof(float):
                    return ValueType.Float;
                case Type t when t == typeof(bool):
                    return ValueType.Bool;
                case Type t when t == typeof(Vector3):
                    return ValueType.Vector3;
                case Type t when t == typeof(string):
                    return ValueType.String;
            }

            return ValueType.Int;
        }
    }
}