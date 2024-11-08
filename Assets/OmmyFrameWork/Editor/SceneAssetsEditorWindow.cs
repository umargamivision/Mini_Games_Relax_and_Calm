// Ommy.Editor
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
namespace Ommy.Editor.SceneManagement
{
    [InitializeOnLoad]
    public class SceneIconsInSceneView
    {
        static SceneIconsInSceneView()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }
        static bool abc = true;
        private static void OnSceneGUI(SceneView sceneView)
        {
            Handles.BeginGUI();
            var scenes = EditorBuildSettings.scenes;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Scenes", EditorStyles.boldLabel, GUILayout.Width(50), GUILayout.Height(30)))
            {
                abc = !abc;
            }

            foreach (var scene in scenes)
            {
                if (abc && scene.enabled)
                {
                    var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
                    if (sceneAsset != null)
                    {
                        GUILayout.BeginVertical(GUILayout.Width(20)); 
                        if (GUILayout.Button(EditorGUIUtility.IconContent("d_SceneAsset Icon"), GUILayout.Width(30), GUILayout.Height(30)))
                        {
                            OpenScene(sceneAsset);
                        }
                        GUILayout.Label(sceneAsset.name, GUILayout.Width(30), GUILayout.Height(20)); 

                        GUILayout.EndVertical();
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
            //GUILayout.EndArea();

            Handles.EndGUI();
        }

        private static void OpenScene(SceneAsset sceneAsset)
        {
            if (sceneAsset != null)
            {
                string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
                if (!string.IsNullOrEmpty(scenePath))
                {
                    EditorSceneManager.OpenScene(scenePath);
                }
            }
        }
    }
}
#endif