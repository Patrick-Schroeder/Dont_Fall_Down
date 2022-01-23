using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButtonManager : MonoBehaviour
{
    public int difficulty;

    private Button button;
    private GameManager gameManager;
    [SerializeField] private GameObject difficultyMenu;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
        gameManager = GameObject.Find(TagsAndNames.GameManager).GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetDifficulty()
    {
        ActivationMethods.DeactivateObject(difficultyMenu);
        ActivationMethods.DeactivateMouse();
        gameManager.ActivateScoreText();
        gameManager.StartGame(difficulty);
    }
}
