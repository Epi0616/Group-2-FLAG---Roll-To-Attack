using UnityEngine;
using UnityEngine.UI;
public class RevealImage : Image
{
    [SerializeField,Range(0f, 1f)]
    private float revealProgress = 0f;
    private Color sigilColour = Color.red;
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

    public Color SigilColour
    {
        get => sigilColour;
        set
        {
            sigilColour = value;
            SetVerticesDirty();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        revealProgress = 0;
        sigilColour = Color.red;
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

            Vector4 uv3 = vertex.uv3;
            uv3.x = sigilColour.r;
            uv3.y = sigilColour.g;
            uv3.z = sigilColour.b;
            vertex.uv3 = uv3;
            //Debug.Log("sigilColour: " + sigilColour);
           // Debug.Log("UV Colours: " + uv3.x + " , " + uv3.y + " , " + uv3.z);
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
