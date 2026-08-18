using UnityEngine;

public class WebSystemVisual : MonoBehaviour
{
    public LineRenderer lr;
    public MeshFilter meshFilter;
    private Mesh mesh;
    private bool hasBeenDestroyed = false;

    public void Awake()
    {
        //if (lr == null)
        //{
        //    lr = GetComponent<LineRenderer>();
        //}
        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
        }
    }

    // The LineRenderer aspect of this visual has been split to now be managed by the Node Connections instead, though it can be re-enabled


    public void SetVisualsByPosition(Transform A, Transform B, Transform C)
    {
        //Debug.Log("New Visual Made");
        hasBeenDestroyed = false;
        //lr.loop = true;
        //lr.positionCount = 3;
        //lr.SetPosition(0, A.position);
        //lr.SetPosition(1, B.position);
        //lr.SetPosition(2, C.position);

        // Convert into 2D positions
        Vector3 AA = new Vector3(A.position.x, A.position.y - 0.5f, A.position.z);
        Vector3 AB = new Vector3(B.position.x, B.position.y - 0.5f, B.position.z);
        Vector3 AC = new Vector3(C.position.x, C.position.y - 0.5f, C.position.z);

        // Transform into local space for the mesh generation
        Vector3[] newVertices = {
            meshFilter.transform.InverseTransformPoint(AA),
            meshFilter.transform.InverseTransformPoint(AB),
            meshFilter.transform.InverseTransformPoint(AC)};

        // Check the orientation of the face so that the correct face can be drawn
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
        
        // This is a triangle. I don't need a custom mesh, behold mesh generation
        mesh = new Mesh { vertices = newVertices, triangles = newTri };
        
        // Recalcuate new mesh properties and set to the Mesh Filter
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
