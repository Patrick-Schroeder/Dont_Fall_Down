using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonExtension : MonoBehaviour
{
    public GameObject cannonballPrefab;

    private int reloadTime = 5;
    private float startDelay = 1.0f;
    private bool isInstantiated = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isInstantiated && LoadingData.difficulty > 0)
        {
            isInstantiated = true;

            // 'Update' und nicht 'Start', da LoadingData.difficulty manuell vom User gesetzt wird ('Start' dann bereits durchgelaufen)
            InvokeRepeating("SpawnCannonball", startDelay, reloadTime / LoadingData.difficulty);
        }
    }

    private void SpawnCannonball()
    {
        Vector3 spawnPos = gameObject.transform.position + gameObject.transform.up.normalized;
        Instantiate(cannonballPrefab, spawnPos, gameObject.transform.rotation);
    }
}
