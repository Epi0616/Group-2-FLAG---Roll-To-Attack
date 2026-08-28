using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Rendering;

public class GateManager : MonoBehaviour
{
    public List<GameObject> gates;
    private float gateUpY = 0;
    private float gateDownY = -16;
    private Coroutine moveGatesRoutine;

    private void OnEnable()
    {
        WaveSpawner.finishedSpawning += GatesUp;
        WaveManager.WaveCountStart += GatesDown;
    }

    private void OnDisable()
    {
        WaveSpawner.finishedSpawning -= GatesUp;
        WaveManager.WaveCountStart -= GatesDown;
    }

    private void GatesUp()
    {
        if (moveGatesRoutine != null)
        {
            StopCoroutine(moveGatesRoutine);
        }
        float currentY = gates[0].transform.position.y;
        moveGatesRoutine = StartCoroutine(MoveGates(2, currentY, gateUpY));
    }

    private void GatesDown(float timer)
    {
        if (moveGatesRoutine != null)
        {
            StopCoroutine(moveGatesRoutine);
        }
        float currentY = gates[0].transform.position.y;
        moveGatesRoutine = StartCoroutine(MoveGates(timer, currentY, gateDownY));
    }

    private IEnumerator MoveGates(float duration, float from, float to)
    {
        float timer = 0;
        float t = 0;

        if (duration <= 0)
        {
            duration = 0.5f;
        }

        while (t < 1)
        {
            timer += Time.deltaTime;
            t = timer / duration;

            foreach (GameObject gate in gates)
            {
                Vector3 gatePos = gate.transform.position;
                gatePos.y = Mathf.Lerp(from, to, t);
                gate.transform.position = gatePos;
            }
            yield return null;
        }
        
        foreach (GameObject gate in gates)
        {
            Vector3 gatePos = gate.transform.position;
            gatePos.y = to;
            gate.transform.position = gatePos;
        }

        moveGatesRoutine = null;
    }
}
