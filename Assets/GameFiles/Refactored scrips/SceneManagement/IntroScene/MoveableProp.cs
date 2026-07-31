using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class MoveableProp : MonoBehaviour
{
    protected Rigidbody rb;
    protected Vector3 startPosition;

    protected virtual void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    public void MoveToPosition(Vector3 targetPos)
    { 
        Vector3 direciton = targetPos - transform.position;

        rb.linearVelocity = direciton * 40f;
    }

    public void ObjectDropped()
    { 
        StartCoroutine(ReturnToOriginalPosition(6f));
    }

    protected virtual IEnumerator ReturnToOriginalPosition(float waitTime)
    {
        while (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPosition;
    }
}

