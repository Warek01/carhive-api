using FuzzySharp;

namespace FafCarsApi.Helpers;

public static class FuzzySearchHelper {
  public static IEnumerable<string> Sort(List<string> list, string token) {
    return list
      .Select<string, (string Item, int Score)>(item => (Item: item, Score: Fuzz.Ratio(item, token)))
      .OrderByDescending(tuple => tuple.Score)
      .Select(tuple => tuple.Item);
  }
  
  public static IEnumerable<T> Sort<T>(List<(string Str, T Data)> list, string token) {
    return list
      .Select(item => (Item: item, Score: Fuzz.Ratio(item.Str, token)))
      .OrderByDescending(tuple => tuple.Score)
      .Select(tuple => tuple.Item.Data);
  }
}
