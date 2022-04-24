using UnityEngine.SceneManagement;

public class SceneManagement : Singleton<SceneManagement>
{
    private readonly string startScene =   "StartScene";
    private readonly string mainScene =    "MainGameScene";
    private readonly string loadingScene = "LoadingScene";

    public static string NextScene;

    public void LoadStartScene() => this.CallLoadingScene(startScene);

    public void LoadMainScene() => this.CallLoadingScene(mainScene);

    public void CallLoadingScene(string targetScene)
    {
        NextScene = targetScene;
        SceneManager.LoadScene(loadingScene);
    }
}

