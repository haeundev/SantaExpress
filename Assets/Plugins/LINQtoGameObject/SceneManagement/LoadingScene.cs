using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    private string nextScene;
    private string loadingPerStr;
    private float loadingPer;
    //private float timer = 0f;

    public Text progressText = null;
    public Slider progressSlider = null;
    public Text loadingText = null;

    private void Start()
    {
        if (SceneManagement.NextScene != null)
        {
            nextScene = SceneManagement.NextScene;
            StartCoroutine(this.LoadScene());
        }
        else
        {
            CatLog.ELog("NextScene Info is Null");
        }
    }

    IEnumerator LoadScene()
    {
        yield return null;

        //Loading Scene에서 불러올 씬을 비동기 로드, Process 가 0.9일 때 자동으로 넘어가는 것을
        //방지하기 위해 allowSceneActive를 false로 설정후 로딩 진행
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;

            //페이크 로딩 시간 미적용 (안이쁨)
            #region NOT_FAKE_TIME

            //timer += Time.deltaTime;
            //
            //if(op.progress < 0.9f)
            //{
            //    progressSlider.value = Mathf.MoveTowards(progressSlider.value, op.progress, timer);
            //
            //    if(progressSlider.value >= op.progress)
            //    {
            //        timer = 0f;
            //    }
            //}
            //else
            //{
            //    progressSlider.value = Mathf.MoveTowards(progressSlider.value, 1.0f, timer);
            //
            //    if(progressSlider.value <= 1.0f)
            //    {
            //        op.allowSceneActivation = true;
            //        yield break;
            //    }
            //}

            #endregion

            //페이크 로딩 시간 적용 (이쁨)
            #region FAKE_TIME

            //Slider Type Loading :: Fake Time
            if (progressSlider.value < 0.9f)
            {
                progressSlider.value = Mathf.MoveTowards(progressSlider.value, 0.9f, Time.deltaTime);
            }
            else if (op.progress >= 0.9f)
            {
                progressSlider.value = Mathf.MoveTowards(progressSlider.value, 1.0f, Time.deltaTime);
            }

            //if(progressSlider.value >= 1f)
            //{
            //    loadingText.text = "Press SpaceBar !";
            //}

            //if(Input.GetKeyDown(KeyCode.Space) && progressSlider.value >= 1f && op.progress >= 0.9f)
            //{
            //    NextSceneCommand !
            //}

            if (progressSlider.value >= 1f)
            {
                op.allowSceneActivation = true;
                yield break;
            }

            if(progressText != null)
            {
                //Get Integer, Progress Persentage Text
                loadingPer = progressSlider.value * 100;
                loadingPerStr = string.Format("{0} {1}", Mathf.FloorToInt(loadingPer).ToString(), "%");
                progressText.text = loadingPerStr;
            }


            #endregion
        }
    }
}
