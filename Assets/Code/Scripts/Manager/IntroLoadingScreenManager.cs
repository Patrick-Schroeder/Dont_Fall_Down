using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroLoadingScreenManager : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private GameObject loadingScreen;
    private Slider progressBar;
    private TextMeshProUGUI versionNumber;
    private TextMeshProUGUI percentageLoaded;
    private float fadeLoadingScreenInSecs = 1;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GameObject.Find("Canvas").GetComponent<CanvasGroup>();
        loadingScreen = GameObject.Find("IntroLoadingScreen");
        progressBar = GameObject.Find("ProgressBar").GetComponent<Slider>();
        versionNumber = GameObject.Find("Version").GetComponent<TextMeshProUGUI>();
        percentageLoaded = GameObject.Find("PercentageLoaded").GetComponent<TextMeshProUGUI>();

        versionNumber.text = LoadingData.versionNr;

        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(StartLoad());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StartLoad()
    {
        loadingScreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, fadeLoadingScreenInSecs));

        AsyncOperation operation = SceneManager.LoadSceneAsync(LevelNames.Tutorial, LoadSceneMode.Additive);
        while (!operation.isDone)
        {
            progressBar.value = Mathf.Clamp01(operation.progress / 0.9f);

            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            percentageLoaded.text = Mathf.Round(progressValue * 100) + "%";

            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0, 1));
        loadingScreen.SetActive(false);

        LoadingData.isSceneInitialized = false;
    }

    IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetValue;
    }
}
