using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public interface IWave
{
    List<WaveGroupObj> waveGroupObjs { get; }
    List<WaveGroup> waveGroups { get; set; }
}
