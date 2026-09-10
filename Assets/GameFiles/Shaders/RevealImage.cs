using UnityEngine;
using UnityEngine.UI;
public class RevealImage : Image
{
    [SerializeField,Range(0f, 1f)]
    public float revealProgress = 1f;

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        base.OnPopulateMesh(toFill);

        UIVertex vertex = new UIVertex();

        for (int i = 0; i < toFill.currentVertCount; i++)
        {
            toFill.PopulateUIVertex(ref vertex, i);

            Vector4 uv1 = vertex.uv1;
            uv1.x = revealProgress;
            vertex.uv1 = uv1;

            toFill.SetUIVertex(vertex, i);
        }
    }
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        revealProgress = Mathf.Clamp01(revealProgress);
        SetVerticesDirty();
    }
#endif
}
