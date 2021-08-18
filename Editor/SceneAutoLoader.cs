#if (UNITY_EDITOR)
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.Core
{
  /// <summary>
  /// Scene auto loader.
  /// </summary>
  /// <description>
  /// This class adds a File > Scene Autoload menu containing options to select
  /// a "master scene" enable it to be auto-loaded when the user presses play
  /// in the editor. When enabled, the selected scene will be loaded on play,
  /// then the original scene will be reloaded on stop.
  ///
  /// Based on an idea on this thread:
  /// http://forum.unity3d.com/threads/157502-Executing-first-scene-in-build-settings-when-pressing-play-button-in-editor
  /// </description>
  [InitializeOnLoad]
  static class SceneAutoLoader
  {
    // Static constructor binds a playmode-changed callback.
    // [InitializeOnLoad] above makes sure this gets executed.
    static SceneAutoLoader()
    {
      EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    // Menu items to select the "master" scene and control whether or not to load it.
    [MenuItem("File/Scene Autoload/Select Master Scene...")]
    static void SelectMasterScene()
    {
      string masterScene = EditorUtility.OpenFilePanel("Select Master Scene", Application.dataPath, "unity");
      masterScene = masterScene.Replace(Application.dataPath, "Assets");  //project relative instead of absolute path
      if (string.IsNullOrEmpty(masterScene))
        return;
      MasterScene = masterScene;
      LoadMasterOnPlay = true;
    }

    [MenuItem("File/Scene Autoload/Load Master On Play", true)]
    static bool ShowLoadMasterOnPlay() => !LoadMasterOnPlay;

    [MenuItem("File/Scene Autoload/Load Master On Play")]
    static void EnableLoadMasterOnPlay() => LoadMasterOnPlay = true;

    [MenuItem("File/Scene Autoload/Don't Load Master On Play", true)]
    static bool ShowDontLoadMasterOnPlay() => LoadMasterOnPlay;

    [MenuItem("File/Scene Autoload/Don't Load Master On Play")]
    static void DisableLoadMasterOnPlay() => LoadMasterOnPlay = false;

    static void OnPlayModeChanged(PlayModeStateChange state)
    {
      if (!LoadMasterOnPlay || SceneManager.GetActiveScene().path == MasterScene)
      {
        return;
      }

      // Started Playing
      if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
      {
        PreviousScene = SceneManager.GetActiveScene().path;
        keepMasterSceneOpen = MasterSceneIsOpen;

        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() && !keepMasterSceneOpen)
        {
          try
          {
            EditorSceneManager.OpenScene(MasterScene, OpenSceneMode.Additive);
          }
          catch
          {
            Debug.LogError($"error: scene not found: {MasterScene}");
            EditorApplication.isPlaying = false;

          }
        }
        else
        {
          EditorApplication.isPlaying = false;
        }
      }

      // Stopped Playing
      if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode || keepMasterSceneOpen)
        return;
      try
      {
        EditorSceneManager.CloseScene(SceneManager.GetSceneByPath(MasterScene), true);
      }
      catch
      {
        Debug.LogError($"error: scene not found: {PreviousScene}");
      }
    }

    // Properties are remembered as editor preferences.
    const string CEditorPrefLoadMasterOnPlay = "Fralle.SceneAutoLoader.LoadMasterOnPlay";
    const string CEditorPrefMasterScene = "Fralle.SceneAutoLoader.MasterScene";
    const string CEditorPrefPreviousScene = "Fralle.SceneAutoLoader.PreviousScene";

    static bool LoadMasterOnPlay
    {
      get => EditorPrefs.GetBool(CEditorPrefLoadMasterOnPlay, false);
      set => EditorPrefs.SetBool(CEditorPrefLoadMasterOnPlay, value);
    }

    static bool MasterSceneIsOpen
    {
      get
      {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
          if (SceneManager.GetSceneAt(i).path == MasterScene)
            return true;
        }
        return false;
      }
    }
    static bool keepMasterSceneOpen;
    static string MasterScene
    {
      get => EditorPrefs.GetString(CEditorPrefMasterScene, "Master.unity");
      set => EditorPrefs.SetString(CEditorPrefMasterScene, value);
    }

    static string PreviousScene
    {
      get => EditorPrefs.GetString(CEditorPrefPreviousScene, SceneManager.GetActiveScene().path);
      set => EditorPrefs.SetString(CEditorPrefPreviousScene, value);
    }
  }
}
#endif
