using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemExtension : MonoBehaviour
{
    public int pointValue;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagsAndNames.Player))
        {
            Destroy(gameObject);
            gameManager.UpdateScore(pointValue);
        }
    }
}
