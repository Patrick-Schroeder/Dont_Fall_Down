using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find(TagsAndNames.GameManager).GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SkipTutorial()
    {
        LoadingData.isSceneInitialized = false;
        SceneManager.LoadSceneAsync(LevelNames.Level1, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(LevelNames.Tutorial);
    }

    public void StartGame()
    {
        SceneManager.UnloadSceneAsync(LevelNames.Intro);
        gameManager.isGameActive = true;
        gameManager.playerController.canPlayerMove = true;
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
        gameManager.playerController.canPlayerMove = true;
    }
}
