using FuzzySharp;

namespace FafCarsApi.Helpers;

public static class FuzzySearchHelper {
  public static IEnumerable<string> Sort(List<string> list, string token, int filterScore = 0) {
    return list
      .Select<string, (string Item, int Score)>(item => (Item: item, Score: Fuzz.Ratio(item.ToLower(), token.ToLower())))
      .OrderByDescending(tuple => tuple.Score)
      .Where(tuple => tuple.Score >= filterScore)
      .Select(tuple => tuple.Item);
  }
  
  public static IEnumerable<T> Sort<T>(List<(string Str, T Data)> list, string token, int filterScore = 0) {
    return list
      .Select(item => (Item: item, Score: Fuzz.Ratio(item.Str.ToLower(), token.ToLower())))
      .OrderByDescending(tuple => tuple.Score)
      .Where(tuple => tuple.Score >= filterScore)
      .Select(tuple => tuple.Item.Data);
  }
}
