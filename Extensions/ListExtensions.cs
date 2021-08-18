using System.Collections.Generic;

namespace Fralle.Core
{
  public static class ListExtension
  {
    public static T PopAt<T>(this List<T> list, int index)
    {
      T r = list[index];
      list.RemoveAt(index);
      return r;
    }
  }
}
