using UnityEngine;
using UnityEngine.UI;
public class RevealImage : Image
{
    [SerializeField,Range(0f, 1f)]
    private float revealProgress = 0f;
    private float randomOffset;
    public float RevealProgress
    {
        get => revealProgress;
        set
        {
            value = Mathf.Clamp01(value);

            if (!Mathf.Approximately(revealProgress, value))
            {
                revealProgress = value;
                SetVerticesDirty();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        revealProgress = 0;
        randomOffset = Random.Range(-1f, 1f);
        if (randomOffset < 0.5f)
        {
            randomOffset = Random.Range(-1f, -0.5f);
        }
        else
        {
            randomOffset = Random.Range(0.5f, 1f);
        }
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        base.OnPopulateMesh(toFill);

        UIVertex vertex = new UIVertex();

        for (int i = 0; i < toFill.currentVertCount; i++)
        {
            toFill.PopulateUIVertex(ref vertex, i);

            Vector4 uv1 = vertex.uv1;
            uv1.x = revealProgress;
            uv1.y = randomOffset;
            vertex.uv1 = uv1;
           // Debug.Log("UV0: " + vertex.uv1.x);
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
