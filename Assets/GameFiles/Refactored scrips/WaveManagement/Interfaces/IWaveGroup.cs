using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public interface IWaveGroup
{
    List<EntityBlockObj> entityBlockObjs { get; }
    List<EntityBlock> entityBlocks { get; set; }
    List<BaseWaveCondition> conditions { get; }
}
