#if UNITY_EDITOR
using UnityEditor;

namespace Fralle.Core
{
  internal class SavedBool
  {
    private bool value;
    private string name;

    public bool Value
    {
      get => value;
      set
      {
        if (this.value == value)
        {
          return;
        }

        this.value = value;
        EditorPrefs.SetBool(name, value);
      }
    }

    public SavedBool(string name, bool value)
    {
      this.name = name;
      this.value = EditorPrefs.GetBool(name, value);
    }
  }
}
#endif
