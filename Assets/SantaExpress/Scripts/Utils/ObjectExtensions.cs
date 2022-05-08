using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

public static class ObjectExtensions
{
    private static T PeekRandom<T>(this IList<T> list)
    {
        if (list == null || list.Count == 0)
            return default;
        var rnd = new Random();
        var index = rnd.Next(0, list.Count);
        return list[index];
    }
    
    public static T PeekRandom<T>(this IEnumerable<T> enumerable) where T : class
    {
        return enumerable == null ? default : PeekRandom(enumerable.ToList());
    }
}