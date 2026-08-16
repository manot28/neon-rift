using UnityEngine;

public class ShootTurret : MonoBehaviour
{
    private TurretVision vision;
    private bool isActive;
    [SerializeField] private GameObject proj;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Collider bodyCollider;
    float timer;

    private Animator anim;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        isActive = true;
        vision = GetComponentInChildren<TurretVision>();
    }

    void Update()
    {
        if (!vision.playerDetected || !isActive)
            return;

        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        Instantiate(proj, transform.position, transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!bodyCollider.bounds.Intersects(other.bounds))
            return;

        int layer = other.gameObject.layer;

        if (layer == LayerMask.NameToLayer("Ground") ||
            layer == LayerMask.NameToLayer("Wall")
            || layer == LayerMask.NameToLayer("Cube"))
        {
            isActive = false;
            anim.SetTrigger("Die");
            Debug.Log("Turret disabled");
        }
    }
}