using UnityEngine;

public interface ISlimeSplit
{
    GameObject childObj { get; set; }
    float baseMoveSpeed { get; set; }
    int baseMaxHealth { get; set; }
    int baseIterationsLeft { get; set; }
    float baseScale { get; set; }
    int childrenSpawned { get; set; }
    int iterationsLeft { get; set; }
    float scale { get; set; }

    public void SplitReset(int maxHealth, int interationsLeft, float scale);
}
