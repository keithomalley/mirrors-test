using UnityEngine;

public class Laser : MonoBehaviour {

    private LineRenderer m_lineRenderer;
    private int m_reflections = 0;

    void Awake() {
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        // set the reflections to zero so we can calculate them each frame recursively
        m_reflections = 0;
        SendRay(transform.position, transform.forward);
    }

    void SendRay(Vector3 position, Vector3 forward) {
        RaycastHit hit;
        
        if (Physics.Raycast(position, forward, out hit)) {
            
            // if we hit something, draw a line to the point
            m_lineRenderer.SetVertexCount(2 + m_reflections);
            m_lineRenderer.SetPosition(1 + m_reflections, transform.InverseTransformPoint(hit.point));
            
            // if we hit a mirror, add to reflections and call this function again
            if (hit.collider.gameObject.tag == "Mirror") {
                m_reflections++;

                // cast a ray from hit point in direction of reflection
                SendRay(hit.point, Vector3.Reflect(hit.point - position, hit.normal));
            }

        } else {
            // Hitting nothing 
            m_lineRenderer.SetVertexCount(2 + m_reflections);
            m_lineRenderer.SetPosition(1 + m_reflections, transform.TransformPoint( forward * 100f ) );
        }
    }
}
