using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI Username;
    public TMP_InputField UsernameInputField;

    public GameObject player;
    internal PlayerController playerController;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public GameObject subMenu;
    public GameObject finishScreen;
    public ParticleSystem finishParticle;
    public Animator playerAnim;
    public bool isGameActive;
    public int score; // Total score
    public int temporaryScore; // If level gets resetted then reset temporaryScore to 'score'. If player wins then score = temporaryScore

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        playerAnim = player.GetComponent<Animator>();

        LoadUserData();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int scoreToAdd)
    {
        temporaryScore += scoreToAdd;
        scoreText.text = "Score: " + temporaryScore;
    }

    public void GameOver()
    {
        playerAnim.SetBool("Death_b", true);
        playerAnim.SetInteger("DeathType_int", 1);

        subMenu.SetActive(true);
        UiManager.SetChildrenActive(subMenu, true);

        gameOverText.gameObject.SetActive(true);
        isGameActive = false;
        playerController.canPlayerMove = false;
        ActivateMouse();
    }

    public void GameFinished()
    {
        score = temporaryScore;
        isGameActive = false;
        finishParticle.Play();
        finishScreen.gameObject.SetActive(true);
        ActivateMouse();
    }

    public void RestartScene()
    {
        player.transform.position = Vector3.zero;
        temporaryScore = score;

        isGameActive = true;
        playerController.canPlayerMove = true;
        playerAnim.SetBool("Death_b", false);

        subMenu.SetActive(false);
        UiManager.SetChildrenActive(subMenu, false);

        gameOverText.gameObject.SetActive(false);

        DeactivateMouse();
    }

    public virtual void StartGame(int difficulty)
    {
        isGameActive = true;
        playerController.canPlayerMove = true;
        score = 0;

        LoadingData.difficulty = difficulty;

        UpdateScore(0);
    }

    public void ActivateScoreText()
    {
        scoreText.gameObject.SetActive(true);
    }

    public void LoadNextLevel()
    {
        player.transform.position = Vector3.zero;

        var sceneName = SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name;
        var nextSceneName = LevelNames.GetNextLevelName(sceneName);
        SceneManager.UnloadSceneAsync(sceneName);
        SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
    }

    public void ActivateObject(GameObject obj)
    {
        ActivationMethods.ActivateObject(obj);
    }

    public void DeactivateObject(GameObject obj)
    {
        ActivationMethods.DeactivateObject(obj);
    }
    
    public void ActivateMouse()
    {
        ActivationMethods.ActivateMouse();
    }
    
    public void DeactivateMouse()
    {
        ActivationMethods.DeactivateMouse();
    }

    public void ActivatePlayerMovement()
    {
        playerController.canPlayerMove = true;
    }

    // Most of the time you won’t save everything inside your classes. It’s good practice and more efficient to use a small class that only contains the specific data that you want to save.
    [System.Serializable]
    class SaveData
    {
        public string Username;
    }

    public void SaveUserData()
    {
        if (string.IsNullOrEmpty(UsernameInputField.text))
            return;

        playerController.PlayerName = UsernameInputField.text;
        Username.text = playerController.PlayerName;

        SaveData data = new SaveData();
        data.Username = playerController.PlayerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadUserData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerController.PlayerName = data.Username;
        }
        else
        {
            playerController.PlayerName = "UsernameTest";
        }

        Username.text = playerController.PlayerName;
    }
}
