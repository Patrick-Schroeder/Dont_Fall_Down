using UnityEngine;

public static class ActivationMethods
{
    public static void DeactivateObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public static void ActivateObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    public static void ActivateMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void DeactivateMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
