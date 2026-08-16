using UnityEngine;

public class TurretVision : MonoBehaviour
{
    // the script attached to trigger sphere which is turret vision zone 
    public bool playerDetected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerDetected = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerDetected = false;
    }
}