using UnityEngine;
using System.Collections.Generic;

public class Teleport : MonoBehaviour
{
    public Teleport Other;

    private HashSet<Transform> justTeleported = new HashSet<Transform>();
    [SerializeField] private AudioSource teleportAudio;

    //raycast
   public bool RayThroughPortals(Vector3 origin, Vector3 direction, float maxDist, List<Vector3> points, out RaycastHit finalHit)
   {
    finalHit = default;
    RaycastHit hit;

    if (!Physics.Raycast(origin, direction, out hit, maxDist))
    {
        points.Add(origin + direction * maxDist);
        return false;
    }

    // no hit portal
    if (!hit.collider.CompareTag("Portal"))
    {
        points.Add(hit.point);
        finalHit = hit;
        return true;
    }

    // hit portal
    points.Add(hit.point);

    Teleport portal = hit.collider.GetComponent<Teleport>();

    if (portal == null || portal.Other == null)
    {
        finalHit = hit;
        return true;
    }

    // ray out of portal
    Vector3 exitOrigin = portal.Other.transform.position + portal.Other.transform.forward * 0.2f;
    Vector3 exitDirection = -portal.Other.transform.up; // 270 degrees

    points.Add(exitOrigin);

    float remainingDistance = maxDist - hit.distance;

    RaycastHit exitHit;

    if (Physics.Raycast(exitOrigin, exitDirection, out exitHit, remainingDistance))
    {
        points.Add(exitHit.point);
        finalHit = exitHit;
        return true;
    }

    points.Add(exitOrigin + exitDirection * remainingDistance);
    return false;
}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PickUp"))
            Destroy(other.gameObject);
        if (other.CompareTag("ViewZone") || justTeleported.Contains(other.transform))
            return;

        TeleportObj(other.transform);
        Other.justTeleported.Add(other.transform);
    }

    private void TeleportObj(Transform obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        AudioClip clip = teleportAudio.clip;
        teleportAudio.PlayOneShot(clip);

        Vector3 localPos = transform.InverseTransformPoint(obj.position);
        localPos = new Vector3(-localPos.x, localPos.y, -localPos.z);

        Vector3 newWorldPos = Other.transform.TransformPoint(localPos);
        newWorldPos += Other.transform.forward * 0.2f;

        Quaternion relativeRot = Quaternion.Inverse(transform.rotation) * obj.rotation;
        Quaternion newWorldRot = Other.transform.rotation;
        
        // tp rb preserve
        if (rb)
        {
            Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);
            localVel.x *= -1;
            localVel.z *= -1;

            rb.position = newWorldPos;
            rb.rotation = newWorldRot;
            rb.linearVelocity = Other.transform.TransformDirection(localVel);
        }
        else
        {
            obj.position = newWorldPos;
            obj.rotation = newWorldRot;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        justTeleported.Remove(other.transform);
    }
}