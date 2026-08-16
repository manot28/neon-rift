using System.Collections.Generic;
using UnityEngine;
public class ButtonSpawnCube : MonoBehaviour
{
    [Header ("Stats")]
    public Transform startPos;
    public GameObject spawn;

    public Renderer rendererButton;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    public Material pressedMat;
    private Material origMat;
    private bool isPressed;

    public AudioSource pressSound;
    public AudioSource dropSound;
    public GameObject menubutton;


    private void Start()
    {
        isPressed = false;
       rendererButton = GetComponent<Renderer>();
       origMat = rendererButton.material;
    }
    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isPressed)
        {
            AudioClip clip = pressSound.clip;
            pressSound.PlayOneShot(clip);
            isPressed = true;
            rendererButton.material = pressedMat;
            Invoke(nameof(Spawn), 1.5f);
        }
    }

    void Spawn()
    {
        GameObject obj = Instantiate(spawn, startPos.position, startPos.rotation);
        spawnedObjects.Add(obj);
        AudioClip clip = dropSound.clip;
        dropSound.PlayOneShot(clip);

        if (spawnedObjects.Count > 1)
        {
            Destroy(spawnedObjects[0]); // destroy oldest
            spawnedObjects.RemoveAt(0);
        }

        rendererButton.material = origMat;
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        menubutton?.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        menubutton?.SetActive(false);
    }
}