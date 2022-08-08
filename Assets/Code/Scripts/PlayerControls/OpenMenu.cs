using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenMenu : MonoBehaviour
{
    public Button restartbutton;
    public Button exitGameButton;

    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OpenCloseMenu);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenCloseMenu();
        }
    }

    private void OpenCloseMenu()
    {
        GameObject player = GameObject.Find(TagsAndNames.Player);
        PlayerController playerController = player.GetComponent<PlayerController>();
        
        if (Cursor.lockState == CursorLockMode.None)
        {
            ActivationMethods.DeactivateMouse();
            restartbutton.gameObject.SetActive(false);
            exitGameButton.gameObject.SetActive(false);

            playerController.canPlayerMove = true;
        }
        else
        {
            ActivationMethods.ActivateMouse();
            restartbutton.gameObject.SetActive(true);
            exitGameButton.gameObject.SetActive(true);

            playerController.canPlayerMove = false;
        }
    }
}
