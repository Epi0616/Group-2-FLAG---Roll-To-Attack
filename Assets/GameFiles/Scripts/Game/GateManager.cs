using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
        moveGatesRoutine = StartCoroutine(MoveGates(2, currentY, gateUpY, 1.5f));
    }

    private void GatesDown(float timer)
    {
        if (moveGatesRoutine != null)
        {
            StopCoroutine(moveGatesRoutine);
        }
        float currentY = gates[0].transform.position.y;
        moveGatesRoutine = StartCoroutine(MoveGates(2, currentY, gateDownY, timer));
    }

    private IEnumerator MoveGates(float duration, float from, float to, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        float timer = duration;
        float t = 0;
        while (t < 1)
        { 
            timer -= Time.deltaTime;
            t = (duration - timer) / duration;

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
