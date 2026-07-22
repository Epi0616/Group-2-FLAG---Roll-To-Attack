using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public interface ITarget
{

    int perimeterPointsCount { get; set; }
    float perimeterRadius { get; set; }
    List<Vector3> perimeterPoints { get; set; }
    void InitializePerimeterPoints();
    void GeneratePerimeterPoints();
}
