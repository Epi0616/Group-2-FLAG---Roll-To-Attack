using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class MoveableProp : MonoBehaviour
{
    protected Rigidbody rb;
    protected Vector3 startPosition;
    public bool canBeMoved = true;

    protected virtual void OnEnable()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        canBeMoved = true;
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    public void MoveToPosition(Vector3 targetPos)
    { 
        Vector3 direciton = targetPos - transform.position;

        rb.linearVelocity = direciton * 40f;
    }

    public virtual void ObjectDropped()
    { 
        StartCoroutine(ReturnToOriginalPosition(6f));
    }

    public virtual void ObjectSelected() { }
    public virtual void ObjectHovered() { }
    public virtual void ObjectUnHovered() { }

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

