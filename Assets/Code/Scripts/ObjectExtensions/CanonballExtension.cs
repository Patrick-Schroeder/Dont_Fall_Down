using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonballExtension : MonoBehaviour
{
    private Rigidbody cannonballRb;
    private int power = 2000;

    // Start is called before the first frame update
    void Start()
    {
        cannonballRb = GetComponent<Rigidbody>();
        cannonballRb.AddRelativeForce(Vector3.up * power, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
