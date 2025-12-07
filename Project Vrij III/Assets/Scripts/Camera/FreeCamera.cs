using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lookSpeed = 2f;
    public float speedMultiplier = 3f;

    private float yaw;
    private float pitch;

    void Update()
    {
        // --- Mouse Look ---
        if (Input.GetMouseButton(1)) // Hold right mouse button
        {
            Cursor.lockState = CursorLockMode.Locked;

            yaw += Input.GetAxis("Mouse X") * lookSpeed;
            pitch -= Input.GetAxis("Mouse Y") * lookSpeed;
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        // --- Movement ---
        float speed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            speed *= speedMultiplier;

        Vector3 input = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            (Input.GetKey(KeyCode.E) ? 1 : 0) - (Input.GetKey(KeyCode.Q) ? 1 : 0),
            Input.GetAxisRaw("Vertical")
        );

        transform.position += speed * Time.deltaTime * transform.TransformDirection(input.normalized);
    }
}
