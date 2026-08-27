using System.Collections.Generic;
using UnityEngine;

public interface IWavePool
{
    List<EntityBlockObj> entityBlockObjs { get; }
    List<EntityBlock> entityBlocks { get; set; }
    int waveRestriction { get; set; }
}
