using UnityEngine;
using System.Collections.Generic;

public interface IGhostTrail 
{
    Queue<Vector3> ghostQueue { get; set; }

    float timer { get; set; }

    float interval { get; set; }

    float maxDelay { get; set; }

    void GenerateGhostTrail();

    Vector3 GetGhostTrail();
}
