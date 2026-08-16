using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRb;
    private bool isHolding;
    [SerializeField] private AudioSource pickUpSound;

    [SerializeField] private float range = 20f;
    [SerializeField] private float pickUpForce = 200f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && !isHolding)
        {
            if(heldObj == null)
            {
                isHolding = true;
                RaycastHit hit;

                if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range))
                    PickUpObject(hit.transform.gameObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.F) && isHolding)
        {
            if (Input.GetKeyDown(KeyCode.E))
                heldObjRb.AddForce(transform.forward * 1400f, ForceMode.Impulse);
            isHolding =false;
            DropObject();
        }

        if (Input.GetKeyDown(KeyCode.E) && heldObj != null)
        {
            heldObjRb.AddForce(transform.forward * 1400f, ForceMode.Impulse);
            isHolding = false;
            DropObject();
        }

        if (heldObj != null)
        {
            MoveObject();
        }
    }

    void MoveObject()
    {
        if(Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = holdArea.position - heldObj.transform.position;
            heldObjRb.AddForce(moveDirection * pickUpForce);
        }

    }

    void PickUpObject(GameObject pickObj)
    {
        if(pickObj.GetComponent<Rigidbody>())
        {
            AudioClip clip = pickUpSound.clip;
            pickUpSound.PlayOneShot(clip);

            heldObjRb = pickObj.GetComponent<Rigidbody>();
            heldObjRb.useGravity = false;
            heldObjRb.linearDamping = 10;
            heldObjRb.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjRb.transform.parent = holdArea;
            heldObj = pickObj;
        }
    }

    void DropObject()
    {
            heldObjRb.useGravity = true;
            heldObjRb.linearDamping = 1;
            heldObjRb.constraints = RigidbodyConstraints.None;

            heldObjRb.transform.parent = null;
            heldObj = null;
    }
}

