using UnityEngine;

public static class UiManager
{
    public static void SetChildrenActive(GameObject gameObject, bool isActive)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(isActive);
        }
    }
}
