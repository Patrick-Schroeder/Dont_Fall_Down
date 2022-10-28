using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    private PlayerController playerController;
    private float rotationSpeed = 750;
    private float minRotationDegree = 60;
    private float maxRotationDegree = 325;
    private float mouseXInput;
    private float mouseYInput;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find(TagsAndNames.Player).GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerController.CanPlayerMove)
        {
            return;
        }

        // Rotate Camera
        mouseXInput = Input.GetAxis(TagsAndNames.MouseX);
        mouseYInput = Input.GetAxis(TagsAndNames.MouseY);
        transform.Rotate(Vector3.up, mouseXInput * rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.right, -mouseYInput * rotationSpeed * Time.deltaTime);

        // Set z axis to 0
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);

        // Check rotation limits
        //Case 1:
        // -10 because instead the x value might be higher than maxRotationDegree, if moving the mouse quickly => case 2
        if (transform.eulerAngles.x > minRotationDegree && transform.eulerAngles.x < maxRotationDegree - 10)
        {
            transform.localRotation = Quaternion.Euler(minRotationDegree, transform.rotation.y, transform.rotation.z);
        }
        //Case 2:
        // +10 because the x value might be lower than minRotationDegree, if moving the mouse quickly => case 1
        if (transform.eulerAngles.x < maxRotationDegree && transform.eulerAngles.x > minRotationDegree + 10)
        {
            transform.localRotation = Quaternion.Euler(maxRotationDegree, transform.rotation.y, transform.rotation.z);
        }
    }
}
