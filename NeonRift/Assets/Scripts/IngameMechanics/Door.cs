using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Door : MonoBehaviour
{
    [SerializeField] private DoorFloorButtons[] buttons;
    [SerializeField] private GameObject triggerOpenDoor;
    private bool isOpen = false;

    [Header("Indicator")]
    [SerializeField] private Renderer indicator;
    [SerializeField] private Material[] mats;

    [Header("Audio")]
    [SerializeField] private AudioSource activateSound;
    [SerializeField] private AudioSource disactivateSound;

    private void OnMouseOver()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentIndex + 1);
        }
    }
    public void CheckButtons()
    {
        foreach (var button in buttons)
        {
            if (!button.IsPressed) // check for each button bein pressed
            {
                CloseDoor();
                return;
            }
        }

        OpenDoor();
    }

    void OpenDoor()
    {
        if (isOpen) return; // prevent spam
        isOpen = true;

        triggerOpenDoor.SetActive(!triggerOpenDoor.activeSelf);
        indicator.material = mats[0];
        activateSound.Play();
    }

    void CloseDoor()
    {
        if (!isOpen) return;
        isOpen = false;

        triggerOpenDoor.SetActive(!triggerOpenDoor.activeSelf);
        indicator.material = mats[1];
        disactivateSound.Play();
    }

}