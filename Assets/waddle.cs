using UnityEngine;

public class waddle : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject target;

    Quaternion tempRotation;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.MovePosition(target.transform.position);
        transform.LookAt(target.transform, new Vector3(0, 0, 1));
    }
}
