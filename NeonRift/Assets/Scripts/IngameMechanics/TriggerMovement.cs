using UnityEngine;

public class TriggerMovement : MonoBehaviour
{

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject portal;
    [SerializeField] private AudioSource sound;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        menu.SetActive(true);
        Invoke(nameof(DestroyMenu), 5f);
        if(gameObject.CompareTag("ProjectilePickUp") && other.CompareTag("Player"))
        {
            sound.Play();
            var portalGun = other.GetComponent<PortalGun>();
            if (portalGun != null)
                portalGun.enabled = true; // enable portal gun on trigger enter then tutorial ends and activates 
            portal.transform.position = new Vector3(-385, 61, 56); // so the player cant go back 
        }
    }

    private void DestroyMenu()
    {
        Destroy(menu);
        Destroy(gameObject);
    }
}
