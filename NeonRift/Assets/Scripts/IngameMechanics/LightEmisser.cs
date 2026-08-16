using UnityEngine;
using System.Collections.Generic;

public class LightEmisser : MonoBehaviour
{
    [Header("Bridge Settings")]
    [SerializeField] private float maxDist = 100f;
    [SerializeField] private float bridgeWidth = 3.0f;
    [SerializeField] private float bridgeThickness = 0.2f;
    [SerializeField] private Material bridgeMaterial;

    [Header("References")]
    [SerializeField] private Teleport portal;

    private List<Vector3> beamPoints = new List<Vector3>();
    private List<GameObject> activeSegments = new List<GameObject>();
    private RaycastHit hit;

    void Start()
    {
        // build bridge
        RebuildBridge();
    }

    // build bridge method
    public void RebuildBridge()
    {
        CalculateBeamPoints();
        GenerateBridgeSegments();
    }

    void CalculateBeamPoints()
    {
        beamPoints.Clear();
        beamPoints.Add(transform.position);

        // RayThroughPortals adds point of enter, of exit from portal and final point 
        if (portal != null)
            portal.RayThroughPortals(transform.position, transform.forward, maxDist, beamPoints, out hit);
        else
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxDist))
                beamPoints.Add(hit.point);
            else
                beamPoints.Add(transform.position + transform.forward * maxDist);
        }
    }

    void GenerateBridgeSegments()
    {
        foreach (GameObject segment in activeSegments)
        {
            if (segment != null) Destroy(segment);
        }

        activeSegments.Clear();

        for (int i = 0; i < beamPoints.Count - 1; i += 2)
        {
            Vector3 start = beamPoints[i];
            Vector3 end = beamPoints[i + 1];

            CreateSegment(start, end);
        }
    }

    void CreateSegment(Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        if (distance < 0.001f)
            return;

        GameObject segment = new GameObject("Bridge_Segment");
        segment.transform.position = start;
        segment.transform.forward = direction.normalized;
        segment.transform.parent = this.transform;

        LineRenderer line = segment.AddComponent<LineRenderer>();
        line.material = bridgeMaterial;
        line.useWorldSpace = true;
        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        line.startWidth = bridgeWidth;
        line.endWidth = bridgeWidth;
        line.alignment = LineAlignment.View;

        BoxCollider box = segment.AddComponent<BoxCollider>();
        box.center = new Vector3(0f, 0f, distance * 0.5f);
        box.size = new Vector3(bridgeWidth, bridgeThickness, distance);

        activeSegments.Add(segment);
    }
}