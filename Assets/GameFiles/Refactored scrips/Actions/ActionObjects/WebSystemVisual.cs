using UnityEngine;

public class WebSystemVisual : MonoBehaviour
{
    public LineRenderer lr;
    public MeshFilter meshFilter;
    private Mesh mesh;
    private bool hasBeenDestroyed = false;

    public void Awake()
    {
        if (lr == null)
        {
            lr = GetComponent<LineRenderer>();
        }
        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
        }
    }

    public void SetVisualsByPosition(Transform A, Transform B, Transform C)
    {
        hasBeenDestroyed = false;
        lr.loop = true;
        lr.positionCount = 3;
        lr.SetPosition(0, A.position);
        lr.SetPosition(1, B.position);
        lr.SetPosition(2, C.position);
        Vector3 AA = new Vector3(A.position.x, A.position.y - 0.5f, A.position.z);
        Vector3 AB = new Vector3(B.position.x, B.position.y - 0.5f, B.position.z);
        Vector3 AC = new Vector3(C.position.x, C.position.y - 0.5f, C.position.z);
        Vector3[] newVertices = {
            meshFilter.transform.InverseTransformPoint(AA),
            meshFilter.transform.InverseTransformPoint(AB),
            meshFilter.transform.InverseTransformPoint(AC)};

        Vector3 normal = Vector3.Cross(AB - AA, AC - AA);
        int[] newTri;

        if (Vector3.Dot(normal, Vector3.up) >= 0)
        {
            newTri = new[] { 0, 1, 2 };
        }
        else
        {
            newTri = new[] { 0, 2, 1 };
        }
        if (mesh == null)
        {
            mesh = new Mesh { vertices = newVertices, triangles = newTri };
        }
        else
        {
            mesh.vertices = newVertices;
            mesh.triangles = newTri;
        }

            mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

    public void DestroyMe()
    {
        if (hasBeenDestroyed) { return; }
        hasBeenDestroyed = true;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
