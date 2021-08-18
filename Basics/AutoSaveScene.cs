#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.Core
{
  [InitializeOnLoad]
  public static class SaveAllOnEnterPlayMode
  {
    static SaveAllOnEnterPlayMode()
    {

#if UNITY_2017_2_OR_NEWER
      EditorApplication.playModeStateChanged += (PlayModeStateChange stateChange) =>
      {
        if (stateChange == PlayModeStateChange.ExitingEditMode)
        {
          OnLeaveEditMode();
        }
      };
#else
			EditorApplication.playmodeStateChanged = () => {
				if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying) {
					OnLeaveEditMode ();
				}
			};
#endif
    }

    static void OnLeaveEditMode()
    {
      string currSceneName = SceneManager.GetActiveScene().name;
      if (!string.IsNullOrEmpty(currSceneName) && !currSceneName.StartsWith("InitTestScene"))
      {
        Debug.Log($"Auto-Saving scenes and assets before entering play mode: { string.Join(", ", openSceneNames.ToArray())}");
        EditorSceneManager.SaveOpenScenes();
      }
      AssetDatabase.SaveAssets();
    }

    static IList<string> openSceneNames
    {
      get
      {
        int numScenes = SceneManager.sceneCount;
        IList<string> openScenes = new List<string>(numScenes);
        for (int currSceneIndex = 0; currSceneIndex < numScenes; ++currSceneIndex)
        {
          openScenes.Add(SceneManager.GetSceneAt(currSceneIndex).name);
        }
        return openScenes;
      }
    }
  }
}
#endif
