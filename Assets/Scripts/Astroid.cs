using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth == null)
            return;
        playerHealth.Cresh();
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
