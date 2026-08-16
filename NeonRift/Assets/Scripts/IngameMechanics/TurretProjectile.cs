using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    private Transform player;

    [SerializeField] float speed = 10f;
    [SerializeField] float rotateSpeed = 10f;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        Invoke(nameof(Destroy), 1.5f);
        if (p != null)
            player = p.transform;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
    void Update()
    {
        if (player == null)
            return;

        // direction to player
        Vector3 dir = (player.position - transform.position).normalized;

        // rotate towards player
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation,targetRot,rotateSpeed * Time.deltaTime);

        // move towards player
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("ViewZone") || collision.CompareTag("Turret"))
            return;

        collision.GetComponent<PlayerController>()?.TakeDamage(1);
        Destroy(gameObject);
    }
}