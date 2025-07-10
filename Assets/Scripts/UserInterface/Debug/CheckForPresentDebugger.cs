#if UNITY_EDITOR
using UnityEngine;
using System.Linq;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class CheckForPrecanceDebugger : MonoBehaviour
{
    [InitializeOnLoadMethod]
    private static void CheckDebugModeBeforeBuild()
    {
        BuildPlayerWindow.RegisterBuildPlayerHandler(BuildWithDebugCheck);
    }

    private static void BuildWithDebugCheck(BuildPlayerOptions buildOptions)
    {
        List<EditorBuildSettingsScene> Scenes = EditorBuildSettings.scenes.ToList();
        List<string> scenePaths = Scenes.Select(scene => scene.path).ToList();

        foreach (string scenePath in scenePaths)
        {
            var currentScene = EditorSceneManager.OpenScene(scenePath);

            GameControllerScript[] gamecontrolls = Resources.FindObjectsOfTypeAll<GameControllerScript>();
            foreach (GameControllerScript gamecontroll in gamecontrolls)
            {
                if (gamecontroll.debugMode)
                {
                    if (EditorUtility.DisplayDialog("Warning: Debug Mode Enabled", "Debug mode is currently enabled. Do you want to proceed with the build?", "Yes, Continue", "Cancel Build"))
                    {
                        Debug.Log("If you're reading this that means this works");
                        BuildPipeline.BuildPlayer(buildOptions);
                    }
                    else
                    {
                        Debug.Log("Build canceled due to Debug Mode being enabled.");
                        return;
                    }
                }
            }

            EditorSceneManager.SaveScene(currentScene);
            EditorSceneManager.CloseScene(currentScene, true);
        }

        BuildPipeline.BuildPlayer(buildOptions);
    }
}
#endif