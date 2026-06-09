
using UnityEngine;
using System.Collections.Generic;

public interface IOrbitSpikeSpawner
{
    public int numSpikesPerSpawn {  get; set; }
    public float spikeLifeSpan { get; set; }
    public float orbitRadius { get; set; }
    public float initialOrbitSpeed { get; set; }

    public List<BaseOrbitObject> orbitObjects { get; set; }

    public int spikeDamage { get; set; }

    public GameObject spikePrefab { get; set; }

    public void RemoveObjectFromOrbit(BaseOrbitObject obj);

    public void UpdateOrbitObjectAngles();

}
