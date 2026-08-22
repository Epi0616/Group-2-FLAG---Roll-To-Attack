using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public interface IDecalShadowCast
{
    public ShadowDecal currentShadowDecal { get; set; }
    public GameObject shadowDecalPrefab { get; set; }
    
}
