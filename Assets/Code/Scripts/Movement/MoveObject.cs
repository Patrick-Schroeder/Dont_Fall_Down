using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    // If 'MoveObject' and 'RotateObject' get added to the same object the result is a circular motion

    [SerializeField] private float distance;
    [SerializeField] private float movementSpeed;
    private Vector3 startPosition;
    private string direction;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        direction = "forward";
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == "forward")
        {
            if (transform.position.z < startPosition.z + distance)
            {
                Debug.Log(transform.position.z + " --- " + (startPosition.z - distance));
                transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
                //transform.position = new Vector3(transform.position.x - movementSpeed, transform.position.y, transform.position.z);
            }
            else if (transform.position.z >= startPosition.z + distance)
                direction = "backward";
        }

        else if (direction == "backward")
        {
            if (transform.position.z > startPosition.z)
            {
                transform.Translate(Vector3.back * Time.deltaTime * movementSpeed);
                //transform.position = new Vector3(transform.position.x + movementSpeed, transform.position.y, transform.position.z);
            }
            else if (transform.position.z <= startPosition.z)
                direction = "forward";
        }
    }
}
