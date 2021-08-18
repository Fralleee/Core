using UnityEngine;

namespace Fralle.Core.CameraControls
{
  public class FlyCam : MonoBehaviour
  {
    public float CameraSensitivity = 90;
    public float ClimbSpeed = 4;
    public float NormalMoveSpeed = 10;
    public float SlowMoveFactor = 0.25f;
    public float FastMoveFactor = 3;

    float rotationX = 0.0f;
    float rotationY = 0.0f;

    void Start()
    {
      Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
      rotationX += Input.GetAxis("Mouse X") * CameraSensitivity * Time.deltaTime;
      rotationY += Input.GetAxis("Mouse Y") * CameraSensitivity * Time.deltaTime;
      rotationY = Mathf.Clamp(rotationY, -90, 90);

      transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
      transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

      if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
      {
        transform.position += transform.forward * (NormalMoveSpeed * FastMoveFactor * Input.GetAxis("Vertical") * Time.deltaTime);
        transform.position += transform.right * (NormalMoveSpeed * FastMoveFactor * Input.GetAxis("Horizontal") * Time.deltaTime);
      }
      else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
      {
        transform.position += transform.forward * (NormalMoveSpeed * SlowMoveFactor * Input.GetAxis("Vertical") * Time.deltaTime);
        transform.position += transform.right * (NormalMoveSpeed * SlowMoveFactor * Input.GetAxis("Horizontal") * Time.deltaTime);
      }
      else
      {
        transform.position += transform.forward * (NormalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
        transform.position += transform.right * (NormalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);
      }


      if (Input.GetKey(KeyCode.Q))
      {
        transform.position += transform.up * (ClimbSpeed * Time.deltaTime);
      }

      if (Input.GetKey(KeyCode.E))
      {
        transform.position -= transform.up * (ClimbSpeed * Time.deltaTime);
      }

      if (Input.GetKeyDown(KeyCode.End))
      {
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
      }
    }
  }
}