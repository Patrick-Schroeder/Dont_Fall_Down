using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenMenu : MonoBehaviour
{
    public Button restartbutton;

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
        if (Cursor.lockState == CursorLockMode.None)
        {
            ActivationMethods.DeactivateMouse();
            restartbutton.gameObject.SetActive(false);
        }
        else
        {
            ActivationMethods.ActivateMouse();
            restartbutton.gameObject.SetActive(true);
        }
    }
}
