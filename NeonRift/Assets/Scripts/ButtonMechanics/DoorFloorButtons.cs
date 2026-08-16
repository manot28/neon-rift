using UnityEngine;

public class DoorFloorButtons : MonoBehaviour
{
    [SerializeField] private AudioSource buttonAudio;
    [SerializeField] private Door door;

        public bool IsPressed { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("In");
        if (other.attachedRigidbody)
        {
         AudioClip clip = buttonAudio.clip;
            buttonAudio.PlayOneShot(clip);
            IsPressed = true;
            door.CheckButtons();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Out");
        if (other.attachedRigidbody)
        {
            IsPressed = false;
            door.CheckButtons();
        }
    }
}