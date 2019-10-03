using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{

    public string sceneLoad;
    public float timeLoad = 5;
    public enum TypeLoad { Loading, Time};
    public TypeLoad typeLoad;

    public Image progressBar;
    public Text progressText;
    public int progress = 0;
    private string text;

    public Canvas canvas;

    
    public void LoadScene(string scene)
    {
        StartCoroutine("SceneLoading", scene);
    }

    private void Start()
    {
        //canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        //if (GameManager.gameMode.Equals(GameMode.VR))
        //{
        //    canvas.renderMode = RenderMode.WorldSpace;
        //}

        switch (typeLoad)
        {
            case TypeLoad.Loading:
                StartCoroutine(SceneLoading(sceneLoad));
                break;
            case TypeLoad.Time:
                StartCoroutine(FixedTimeLoading(sceneLoad));               
                break;
        }

        if(progressText != null)
        {
            text = progressText.text;      
        }
        if(progressBar != null)
        {
            progressBar.type = Image.Type.Filled;
            progressBar.fillMethod = Image.FillMethod.Horizontal;
            progressBar.fillOrigin = (int)Image.OriginHorizontal.Left;
        }        
    }

    private void Update()
    {
        switch (typeLoad)
        {
            case TypeLoad.Loading:
                break;
            case TypeLoad.Time:
                progress = (int) (Mathf.Clamp((Time.time / timeLoad), 0.0f, 1.0f)* 100.0f);
                break;
            default:
                break;
        }

        if(progressText != null)
        {
            progressText.text = text + " " + progress + "%";
        }
        if(progressBar != null)
        {
            progressBar.fillAmount = (progress / 100.0f);
        }
    }

    IEnumerator SceneLoading(string scene)
    {
        Debug.Log(scene);
        AsyncOperation loading = SceneManager.LoadSceneAsync(scene);

        while (!loading.isDone)
        {
            progress = (int)(loading.progress * 100.0f);
            yield return null;
        }
    }

    IEnumerator FixedTimeLoading(string scene)
    {
        yield return new WaitForSeconds(timeLoad);
        SceneManager.LoadScene(scene);
    }
}
