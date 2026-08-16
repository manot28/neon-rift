using TMPro;
using UnityEngine;

public class Enter : MonoBehaviour
{
    public GameObject menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

       menu.SetActive(true);

    }

    private void OnTriggerExit(Collider other)
    {
        menu.SetActive(false);
    }
}
