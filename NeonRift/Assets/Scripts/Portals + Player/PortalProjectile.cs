using UnityEngine;

public class PortalProjectile : MonoBehaviour
{
    private Vector3 targetPoint;
    private PortalGun portalGun;
    private RaycastHit hitInfo;

    public float speed = 60f;

    public void Init(Vector3 target, RaycastHit hit, PortalGun gun)
    {
        targetPoint = target;
        hitInfo = hit;
        portalGun = gun;
        transform.LookAt(target);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            portalGun.PlacePortal(hitInfo);
            Destroy(gameObject);
        }
    }
}