using UnityEngine;

public interface ISlimeSplit
{
    GameObject childObj { get; set; }
    int childrenSpawned { get; set; }
    int iterationsLeft { get; set; }
    float scale { get; set; }
}
