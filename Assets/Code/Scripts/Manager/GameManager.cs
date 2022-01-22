using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    internal PlayerController playerController;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public GameObject finishScreen;
    public ParticleSystem finishParticle;
    public Animator playerAnim;
    public float obstacleSpeed = 1.0f;
    public bool isGameActive;
    public int score; // Total score
    public int temporaryScore; // If level gets resetted then reset temporaryScore to 'score'. If player wins then score = temporaryScore

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        playerAnim = player.GetComponent<Animator>();

        LoadingData.gameManager = gameObject.GetComponent<GameManager>();
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

        restartButton.gameObject.SetActive(true);
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

        restartButton.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);

        DeactivateMouse();
    }

    public virtual void StartGame(int difficulty)
    {
        isGameActive = true;
        playerController.canPlayerMove = true;
        score = 0;

        obstacleSpeed *= difficulty;

        UpdateScore(0);
    }

    public void LoadNextLevel()
    {
        player.transform.position = Vector3.zero;

        // TODO: Jedes Level möglich machen
        SceneManager.UnloadSceneAsync(LevelNames.Tutorial);
        SceneManager.LoadSceneAsync(LevelNames.Level1, LoadSceneMode.Additive);
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
