#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fralle.Core.Gameplay
{
	[CustomEditor(typeof(GameSceneSo), true)]
	public class GameSceneSoEditor : Editor
	{
		private const string NO_SCENES_WARNING = "There is no Scene associated to this location yet. Add a new scene with the dropdown below";
		private GUIStyle headerLabelStyle;
		private static readonly string[] ExcludedProperties = { "m_Script", "sceneName" };

		private string[] sceneList;
		private GameSceneSo gameSceneInspected;

		private void OnEnable()
		{
			gameSceneInspected = target as GameSceneSo;
			PopulateScenePicker();
			InitializeGuiStyles();
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.LabelField("Scene information", headerLabelStyle);
			EditorGUILayout.Space();
			DrawScenePicker();
			DrawPropertiesExcluding(serializedObject, ExcludedProperties);
		}

		private void DrawScenePicker()
		{
			var sceneName = gameSceneInspected.SceneName;
			EditorGUI.BeginChangeCheck();
			var selectedScene = sceneList.ToList().IndexOf(sceneName);

			if (selectedScene < 0)
			{
				EditorGUILayout.HelpBox(NO_SCENES_WARNING, MessageType.Warning);
			}

			selectedScene = EditorGUILayout.Popup("Scene", selectedScene, sceneList);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(target, "Changed selected scene");
				gameSceneInspected.SceneName = sceneList[selectedScene];
				MarkAllDirty();
			}
		}

		private void InitializeGuiStyles()
		{
			headerLabelStyle = new GUIStyle(EditorStyles.largeLabel)
			{
				fontStyle = FontStyle.Bold,
				fontSize = 18,
				fixedHeight = 70.0f
			};
		}

		/// <summary>
		/// Populates the Scene picker with Scenes included in the game's build index
		/// </summary>
		private void PopulateScenePicker()
		{
			var sceneCount = SceneManager.sceneCountInBuildSettings;
			sceneList = new string[sceneCount];
			for (int i = 0; i < sceneCount; i++)
			{
				sceneList[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
			}
		}

		/// <summary>
		/// Marks scenes as dirty so data can be saved
		/// </summary>
		private void MarkAllDirty()
		{
			EditorUtility.SetDirty(target);
			EditorSceneManager.MarkAllScenesDirty();
		}
	}
}
#endif
