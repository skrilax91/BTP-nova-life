using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerEngine : MonoBehaviour
{
    private Vector3 velocity;
    private Vector3 rotation;
    private float camRotation;
    private float currentCamRotation = 0;
    private Rigidbody rb;

    [SerializeField]
    private Camera cam;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetVelocity(Vector3 vel)
    {
        velocity = vel;
    }

    public void SetRotation(Vector3 rot, float camRot)
    {
        rotation = rot;
        camRotation = camRot;
    }

    private void Move()
    {   
        if (velocity != Vector3.zero)
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        // set X camera axis
        currentCamRotation -= camRotation;
        currentCamRotation = Mathf.Clamp(currentCamRotation, -90, 90);
        cam.transform.localEulerAngles = new Vector3(currentCamRotation, 0f, 0f);
    }

    private void FixedUpdate()
    {
        Move();
    }
}
