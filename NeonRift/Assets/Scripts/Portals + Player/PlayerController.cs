using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Player Stats")]
    private Vector3 movement;
    public float speed = 5f;
    public float jumpForce = 15f;
    private bool isOnTheGround;
    public int hp = 15;
    public float mouseSensitivity = 100f;

    [Header("Refs")]
    public Transform cameraPivot;
    private Rigidbody rb;
    float xRotation = 0f;
    [SerializeField] private GameObject deathMenu;
    void Start()
    {
        Physics.gravity = new Vector3(0f, -19.62f, 0f); // x2 gravity 
        deathMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isOnTheGround = true;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    void Update()
    {
        // Mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate player left/right
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        movement = (transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical")).normalized;

        if(Input.GetKeyDown(KeyCode.Space) && isOnTheGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnTheGround =false;
        }
    }

    private void LateUpdate()
    {
        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void FixedUpdate()
    {
        Vector3 velocity = movement * speed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;

        Vector3 rot = rb.rotation.eulerAngles;
        rb.rotation = Quaternion.Euler(0f, rot.y, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
            isOnTheGround = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Danger"))
            TakeDamage(hp); // Instant death
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"player HP: {hp}");

        if (hp <= 0)
        {
            hp = 0;
            Die();
        }
    }

    private void Die()
    {
        Animation deathAnim = GetComponent<Animation>();
        if (deathAnim != null)
            deathAnim.Play();

        deathMenu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        enabled = false; // disable player controls
    }
}
