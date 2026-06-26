using System.Collections;
using UnityEngine;

public class ThrowableBoulder : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        rb.isKinematic = true;
    }

    public void HandlePathToTarget(Vector3 target, float durationOfTravel)
    {
        StartCoroutine(PathToTarget(target, transform.position, durationOfTravel));
    }

    private IEnumerator PathToTarget(Vector3 target, Vector3 initialPosition, float durationOfTravel)
    {
        float timer = durationOfTravel;
        float t = 0;

        Vector3 position; 

        float peakInArc = GetPeakInArc(target);
        Debug.Log(peakInArc);

        while (t < 1)
        { 
            timer -= Time.deltaTime;
            t = (durationOfTravel - timer) / durationOfTravel;

            position = Vector3.Lerp(initialPosition, target, t);
            position.y += ArcY(target, initialPosition, peakInArc, t);

            transform.position = position;

            yield return null;
        }
        transform.position = target;
    }

    private float ArcY(Vector3 target, Vector3 initialPosition, float peakInArc, float t)
    {
        return peakInArc * 6 * t * (1 - t);
    }

    private float GetPeakInArc(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        float distance = direction.magnitude;
        float peakInArc;

        Vector3 midPoint = transform.position + (direction / 2);

        if (distance != 0)
        {
            peakInArc = midPoint.y + (500 * (1 / distance));
        }
        else
        {
            peakInArc = midPoint.y + (500 * (1 / distance));
        }

        return peakInArc;
    }

    //private IEnumerator FollowPathToTarget(Vector3 direction, float durationOfTravel, float peakInArc)
    //{ 
    //    float timer = durationOfTravel;
    //    float progress = ((durationOfTravel - timer) / durationOfTravel);

    //    direction.y = peakInArc;
    //    rb.linearVelocity = direction;
    //    //while (progress <= 0.5f)
    //    //{ 
    //    //    timer -= Time.deltaTime;


    //    //}
    //    yield return null;
    //}

    //public void ArcToTarget(Vector3 target, float durationOfTravel)
    //{
    //    Vector3 direction = target - transform.position;
    //    float distance = direction.magnitude;
    //    float peakInArc;

    //    if (distance != 0)
    //    {
    //        peakInArc = direction.y * (2 / distance);
    //    }
    //    else
    //    {
    //        peakInArc = direction.y * (2 / 0.0001f);
    //    }

    //    StartCoroutine(FollowPathToTarget(direction, durationOfTravel, peakInArc));
    //}

}
