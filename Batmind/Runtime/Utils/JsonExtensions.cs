using Batmind.Tree.Nodes;
using Newtonsoft.Json;

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
            
            return settings;
        }
    }
}