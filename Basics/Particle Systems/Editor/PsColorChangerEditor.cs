#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Fralle.Core.Basics
{
  [CustomEditor(typeof(PsColorChanger))]
  public class PsColorChangerEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      if (GUILayout.Button("Change Color"))
        ((PsColorChanger)target).ChangeColor();
      if (GUILayout.Button("Swap \"Current\" with \"New\" colors"))
        ((PsColorChanger)target).SwapCurrentWithNewColors();
    }
  }
}
#endif
