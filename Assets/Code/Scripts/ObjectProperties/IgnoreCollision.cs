using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    [SerializeField] private GameObject hintToActivate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject player = GameObject.Find(TagsAndNames.Player);
        Physics.IgnoreCollision(player.GetComponent<BoxCollider>(), GetComponent<BoxCollider>());
        ActivationMethods.ActivateMouse();
        ActivationMethods.ActivateObject(hintToActivate);

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.canPlayerMove = false;
    }
}
