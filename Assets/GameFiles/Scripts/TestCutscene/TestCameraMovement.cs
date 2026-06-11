using System.Collections;
using System.Timers;
using UnityEngine;

public class TestCameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float targetX;

    [SerializeField] private float seconds;

    private Vector3 startPos;
    private Quaternion startRot;
    private Quaternion targetRot;

    private float timePassed = 0;
    private float move = 0;

    private void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;

        targetRot = Quaternion.Euler(new Vector3(targetX, 0, 0));

    }

    private void Update()
    {
        if (transform.position != targetPos)
        {
            move += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startPos, targetPos, move);
        }

        if (transform.rotation.x != targetX)
        {
            timePassed += Time.deltaTime;

            transform.rotation = Quaternion.Slerp(startRot, targetRot, Mathf.Clamp01(timePassed / seconds));
        }
    }
}
