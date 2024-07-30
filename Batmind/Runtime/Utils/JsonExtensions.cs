using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Batmind.Tree.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Batmind.Utils
{
    public static class JsonExtensions
    {
        
        public static string ToJson(this BehaviourTree tree)
        {
            var json = JsonConvert.SerializeObject(tree, GetSerializationSettings());
            return json;
        }

        public static BehaviourTree FromJson<TTree>(this string treeJson) where TTree : BehaviourTree
        {
            var behaviourTree = JsonConvert.DeserializeObject<TTree>(treeJson, GetSerializationSettings());
            return behaviourTree;
        }

        private static JsonSerializerSettings GetSerializationSettings()
        {
            var settings = new JsonSerializerSettings();
            
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            settings.TypeNameHandling = TypeNameHandling.All;
            settings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;
            settings.ContractResolver = new PrivateSerializeFieldContractResolver();
            
            return settings;
        }
    }
    
    public class PrivateSerializeFieldContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization).ToList();

            // Include private fields marked with [SerializeField]
 
            var serializeFieldProperties = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttribute<SerializeField>() != null)
                .Select(f => base.CreateProperty(f, memberSerialization))
                .ToList();

            foreach (var property in serializeFieldProperties)
            {
                property.Writable = true;
                property.Readable = true;
            }

            properties.AddRange(serializeFieldProperties);
            
            
            return properties;
        }
    }

}