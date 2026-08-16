using UnityEngine;

public class PortalGun : MonoBehaviour
{
    [SerializeField] AudioSource warpSound;

    [SerializeField] private Portal Red;
    [SerializeField] private Portal Blue;

    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private LayerMask hitMask;

    private Portal currentPortal;

    // light bridge emitter
    [Header("Bridge Link")]
    [SerializeField] private LightEmisser bridgeEmitter;

    void Start()
    {
        rotationOffset = new Vector3(270, 0, 0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) { Shoot(Red); }
        if (Input.GetMouseButtonDown(1)) { Shoot(Blue); }
    }

    public void PlacePortal(RaycastHit hit)
    {
        int layer = hit.collider.gameObject.layer;
        ParticleSystem ps = Instantiate(explosion, hit.point, Quaternion.identity);
        Destroy(ps.gameObject, 1f);
        Quaternion baseRot;

        AudioClip clip = warpSound.clip;
        warpSound.PlayOneShot(clip);

        if (layer == 9)
        {
            Vector3 toPlayer = Camera.main.transform.position - hit.point;
            Vector3 projected = Vector3.ProjectOnPlane(toPlayer, hit.normal);

            if (projected.sqrMagnitude < 0.001f)
                projected = transform.forward;

            baseRot = Quaternion.LookRotation(hit.normal, projected);
        }
        else baseRot = Quaternion.LookRotation(hit.normal);
       
        float offset = 1.5f;

        currentPortal.transform.rotation = baseRot * Quaternion.Euler(rotationOffset);
        currentPortal.transform.position = hit.point + hit.normal * offset;

        // emitter rebuildes bridge when portal is placed
        if (bridgeEmitter != null)
        {
            StartCoroutine(RebuildBridgeDelayed());
        }
    }

    // can be abnormal behaviour if bridge is rebuilt at the same time with portal bein placed
    private System.Collections.IEnumerator RebuildBridgeDelayed()
    {
        yield return new WaitForEndOfFrame(); 
        if (bridgeEmitter != null)
            bridgeEmitter.RebuildBridge();
    }


    void Shoot(Portal portal)
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f)); //center of the screen (0-1 coordinates)
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitMask))
        {
            int layer = hit.collider.gameObject.layer;

            if (layer != 8 && layer != 9) return;

            GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

            PortalProjectile p = proj.GetComponent<PortalProjectile>();
            p.Init(hit.point, hit, this);

            currentPortal = portal;
        }
    }
}