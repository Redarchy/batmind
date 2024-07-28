namespace Batmind.Utils
{
    public static class StringExtensions
    {
        public static int ComputeFNV1aHash(this string source)
        {
            uint hash = 2166136261;
            
            foreach (var character in source)
            {
                hash = (hash ^ character) * 16777619;
            }

            return unchecked((int) hash);
        }
    }
}