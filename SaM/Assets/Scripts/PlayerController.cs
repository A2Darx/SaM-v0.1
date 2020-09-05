using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    #region Variables
    public float speed = 5.5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    private PlayerMotor motor;
    #endregion

    #region Motor
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }
    #endregion

    #region Movement and Look Controls
    void Update()
    {
        if (PauseMenu.IsOn)
        {
            if (Cursor.lockState != CursorLockMode.None)
                Cursor.lockState = CursorLockMode.None;

            motor.Move(Vector3.zero);
            motor.Rotate(Vector3.zero); // Vector3.zero means that the player will not move/rotate
            motor.RotateCamera(Vector3.zero);

            return;
        }

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        //Calculate movement velocity as a 3d vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        //Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

        //Apply movement
        motor.Move(_velocity);

        //Calculate rotation as a 3d vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        //Apply rotation
        motor.Rotate(_rotation);

        //Calculate camera rotation as a 3d vector (turning around)
        float _xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * lookSensitivity;

        //Apply camera rotation
        motor.RotateCamera(_cameraRotation);
    }
    #endregion
}
