using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    #region Variables
    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;

    private Rigidbody rb;
    private bool playerIsOnGround = true;
    #endregion

    #region Movement and Rotation
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Gets a movement vector
    public void Move (Vector3 _velocity)
    {
        velocity = _velocity;
    }

    // Perform movement based on velocity variable
    void PerformMovement ()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.deltaTime);
        }
    }

    // Gets a rotational vector
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    // Gets a rotational vector for the camera
    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }

    //Perform rotation
    void PerformRotation ()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler (rotation));
        if (cam != null)
        {
            cam.transform.Rotate(-cameraRotation);
        }
    }

    // Run every physics iteration
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }
    #endregion

    #region Jumping
    void Update()
    {
        if (Input.GetButtonDown("Jump") && playerIsOnGround)
        {
            rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
            playerIsOnGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            playerIsOnGround = true;
        }
    }
    #endregion
}
