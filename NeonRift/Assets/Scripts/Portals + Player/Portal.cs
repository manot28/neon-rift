using UnityEngine;

public class Portal : MonoBehaviour
{
    // visual 
    public Portal other;
    public Camera PortalView;

    private void Start()
    {
        PortalView.nearClipPlane = 0.01f;
        other.PortalView.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        GetComponentInChildren<MeshRenderer>().sharedMaterial.mainTexture = other.PortalView.targetTexture;
    }
    private void Update()
    {
            // position
            Vector3 lookerPos = other.transform.InverseTransformPoint(Camera.main.transform.position);
            lookerPos = new Vector3(-lookerPos.x, lookerPos.y, -lookerPos.z);
            PortalView.transform.position = lookerPos;

            // rotation
            Quaternion relativeRot = Quaternion.Inverse(other.transform.rotation) * Camera.main.transform.rotation;
            PortalView.transform.rotation = transform.rotation * relativeRot;

            //  flip
            PortalView.transform.rotation *= Quaternion.Euler(0, 180f, 0);
            PortalView.transform.position += PortalView.transform.forward * 0.05f;
    }


}
