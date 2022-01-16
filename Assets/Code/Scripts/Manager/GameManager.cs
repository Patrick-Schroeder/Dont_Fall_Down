using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerController playerController;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public GameObject menuScreen;
    public float obstacleSpeed = 1.0f;
    public bool isGameActive;
    public int score;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;
        playerController.canPlayerMove = false;
        ActivateMouse();
    }

    public void RestartScene()
    {
        LoadingData.isSceneInitialized = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public virtual void StartGame(int difficulty)
    {
        isGameActive = true;
        playerController.canPlayerMove = true;
        score = 0;

        obstacleSpeed *= difficulty;

        UpdateScore(0);

        menuScreen.gameObject.SetActive(false);
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
}
