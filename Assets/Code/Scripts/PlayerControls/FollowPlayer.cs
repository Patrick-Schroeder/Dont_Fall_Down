using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, 0);


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // 'LateUpdate' when you have a camera that follows the player.
    // All Calculations for the players movement are done. This is definetly where the camera should go.
    void LateUpdate()
    {
        // Offset the camera behind the player by adding to the player's position
        transform.position = player.transform.position + offset;
    }
}
