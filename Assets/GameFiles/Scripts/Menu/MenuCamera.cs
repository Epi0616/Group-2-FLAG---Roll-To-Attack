using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    Vector3 targetPoint = new (-50,10,70);

    public float radius = 100f;
    public float speed = 360f;
    public float yHeight = 25;

    private float angle;
    private Quaternion rotation;
    private Vector3 offset;

    void Update()
    {
        angle += speed * Time.deltaTime;

        rotation = Quaternion.Euler(0, angle, 0);
        offset = rotation * Vector3.forward * radius;

        offset.y = yHeight;

        transform.position = targetPoint + offset;
        transform.LookAt(targetPoint);
    }
}
