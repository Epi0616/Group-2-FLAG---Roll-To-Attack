using System.Collections;
using UnityEngine;

public interface IInitializeable
{
    void Initialize();
    IEnumerator InitializeAsync();
}
