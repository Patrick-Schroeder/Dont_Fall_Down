using UnityEngine.SceneManagement;

public class TutorialManager : GameManager
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SkipTutorial()
    {
        LoadingData.isSceneInitialized = false;
        SceneManager.LoadScene(LevelNames.Level1);
    }

    public override void StartGame(int difficulty)
    {
        isGameActive = true;
        playerController.canPlayerMove = true;
    }
}
