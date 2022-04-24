using UnityEditor;
using UnityEditor.SceneManagement;

public static class ToolbarMenus 
{
    private const string NightTownScene = "Assets/SantaExpress/NightTown.unity";
    [MenuItem("SantaExpress/OpenScene - NightTown", false, 100)]
    private static void LoadTownScene()
    {
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene(NightTownScene);
    }
}
