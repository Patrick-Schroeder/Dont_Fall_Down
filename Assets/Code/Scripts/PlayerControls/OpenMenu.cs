using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenMenu : MonoBehaviour
{
    public GameObject subMenu;

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
            subMenu.gameObject.SetActive(false);
            UiManager.SetChildrenActive(subMenu, false);

            playerController.canPlayerMove = true;
        }
        else
        {
            ActivationMethods.ActivateMouse();
            subMenu.gameObject.SetActive(true);
            UiManager.SetChildrenActive(subMenu, true);

            playerController.canPlayerMove = false;
        }
    }
}
