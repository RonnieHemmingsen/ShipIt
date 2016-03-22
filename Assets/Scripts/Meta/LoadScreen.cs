using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadScreen : MonoBehaviour {

    [SerializeField]
    private GameObject _progressBar;
    [SerializeField]
    private GameObject _loadPercentage;

    private int _loadProgress;

	// Use this for initialization
	void Start () {
        _loadProgress = 0;
        _progressBar.SetActive(false);
        _loadPercentage.SetActive(false);

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            //StartCoroutine(DisplayLoadingScreen(GameSettings.LOAD_LEVEL_MENU, true));    
        }


	}
	
    public void StartLoadSequence(string sceneName)
    {
        StartCoroutine(DisplayLoadingScreen(sceneName, false));   
    }

    private IEnumerator DisplayLoadingScreen(string sceneName, bool isSplash)
    {
        if(isSplash)
        {
            yield return new WaitForSeconds(7);
        }

        _progressBar.SetActive(true);
        _loadPercentage.SetActive(true);
        Text txt = _loadPercentage.GetComponent<Text>();

        txt.text = GameSettings.LOAD_PROGRESS_TEXT + _loadProgress + "%";
        _progressBar.transform.localScale = new Vector3(_loadProgress, 1, 1);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);


        while(!async.isDone)
        {
            print("Load progress: " + async.progress);
            _loadProgress = (int) (async.progress * 100);
            _progressBar.transform.localScale = new Vector3(async.progress, 1, 1);
            yield return null;    
        }

        yield return new WaitForSeconds(2f);


    }
}
