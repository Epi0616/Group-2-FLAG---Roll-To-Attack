using UnityEngine;
using System.Collections.Generic;

public class GateManager : MonoBehaviour
{
    public List<GameObject> gates;
    float gatesDownY = -18;
    float gatesUpY = 1.8f;
    bool gateUp = true, gateDown = true;
    float gateUpTimer, gateDownTimer;
    float timer = 0;

    private void OnEnable()
    {
        EnemyDirector.WaveOver += GatesUp;
        DiceFaceSelectionUIManager.DiceFaceSelectionOver += GatesDown;
    }

    private void OnDisable()
    {
        EnemyDirector.WaveOver -= GatesUp;
        DiceFaceSelectionUIManager.DiceFaceSelectionOver -= GatesDown;
    }

    private void Start()
    {
        timer = 5;
        gateDown = false;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0) return;

        if (!gateUp)
        {
            for (int i = 0; i < gates.Count; i++)
            {
                RaiseGate(gates[i]);
            }
        }

        if (!gateDown)
        {
            for (int i = 0; i < gates.Count; i++)
            {
                LowerGate(gates[i]);
            }
        }
    }

    private void GatesUp(float timer)
    {
        gateUp = false;
    }

    private void GatesDown(float timer)
    {
        this.timer = timer;
        gateDown = false;
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
