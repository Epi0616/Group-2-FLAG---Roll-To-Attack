using UnityEngine;
using System.Collections.Generic;

public class GateManager : MonoBehaviour
{
    public List<GameObject> gates;
    float gatesDownY = -13;
    float gatesUpY = 0.8f;
    bool waveOver = false, gateUp = false, gateDown = false;
    float timeToNextWave = 0;

    private void OnEnable()
    {
        EnemyDirector.WaveCountStart += WaitForNextWave;
    }

    private void OnDisable()
    {
        EnemyDirector.WaveCountStart -= WaitForNextWave;
    }

    private void Update()
    {
        timeToNextWave -= Time.deltaTime;
        if (timeToNextWave <= 0)
        {
            waveOver = false;
        }


        if (waveOver)
        {
            for (int i = 0; i < gates.Count; i++)
            {
                RaiseGate(gates[i]);
            }
            return;
        }

        for (int i = 0; i < gates.Count; i++)
        {
            LowerGate(gates[i]);
        }
    }

    private void WaitForNextWave(float timeToNextWave)
    {
        this.timeToNextWave = timeToNextWave;
        waveOver = true;
        gateDown = false;
        gateUp = false;
    }
    private void RaiseGate(GameObject gate)
    {
        if (gateUp) { return; }

        Vector3 tempPosition = gate.transform.position;
        float currentHeight = gate.transform.position.y;

        tempPosition.y += (gatesUpY - currentHeight) * Time.deltaTime * 2f;

        if (currentHeight >= gatesUpY - 0.2f)
        {
            tempPosition.y = gatesUpY;
            gateUp = true;
        }

        gate.transform.position = tempPosition;
    }

    private void LowerGate(GameObject gate)
    {
        if (gateDown) { return; }

        Vector3 tempPosition = gate.transform.position;
        float currentHeight = gate.transform.position.y;

        tempPosition.y += (gatesDownY - currentHeight) * Time.deltaTime * 2f;

        if (currentHeight <= gatesDownY + 0.2f)
        {
            tempPosition.y = gatesDownY;
            gateDown = true;
        }

        gate.transform.position = tempPosition;
    }
}
