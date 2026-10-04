using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float boostMultiplier = 1.6f;
    [SerializeField] private float swimVerticalSpeed = 4f;
    [SerializeField] private float damping = 8f;
    [SerializeField] private float lookSensitivity = 2.2f;

    private CharacterController characterController;
    private Camera followCamera;
    private GameObject cameraPivot;
    private Vector3 movementVelocity;
    private float pitch;
    private float yaw;
    private Vector3 lastMoveDirection = Vector3.forward;
    private bool isPaused;

    public Vector3 Velocity => movementVelocity;
    public Vector3 CurrentMoveDirection => lastMoveDirection;
    public float CurrentSpeed => movementVelocity.magnitude;

    private void Awake()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        if (characterController == null)
        {
            characterController = gameObject.AddComponent<CharacterController>();
        }

        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<CapsuleCollider>();
        }

        tag = "Player";
    }

    private void Start()
    {
        SetUpCamera();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isPaused;
        }

        if (isPaused)
        {
            return;
        }

        RotateFromInput();
        MoveFromInput();

        if (Input.GetKeyDown(KeyCode.E) && GameManager.Instance != null)
        {
            GameManager.Instance.TryInteract();
        }
    }

    private void SetUpCamera()
    {
        if (Camera.main != null)
        {
            followCamera = Camera.main;
        }
        else
        {
            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            followCamera = cameraObject.AddComponent<Camera>();
        }

        if (followCamera != null)
        {
            cameraPivot = new GameObject("Camera Pivot");
            cameraPivot.transform.SetParent(transform);
            cameraPivot.transform.localPosition = new Vector3(0f, 1.6f, 0f);

            followCamera.transform.SetParent(cameraPivot.transform, false);
            followCamera.transform.localPosition = new Vector3(0f, 0.5f, -7f);
            followCamera.transform.localRotation = Quaternion.identity;
        }
    }

    private void RotateFromInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -70f, 70f);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        if (cameraPivot != null)
        {
            cameraPivot.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }

    private void MoveFromInput()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if (inputDirection.sqrMagnitude > 1f)
        {
            inputDirection.Normalize();
        }

        Vector3 desiredDirection = transform.right * inputDirection.x + transform.forward * inputDirection.z;

        if (desiredDirection.sqrMagnitude > 0.001f)
        {
            lastMoveDirection = desiredDirection.normalized;
        }

        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? boostMultiplier : 1f;
        Vector3 desiredVelocity = desiredDirection * (moveSpeed * speedMultiplier);

        if (Input.GetKey(KeyCode.Space))
        {
            desiredVelocity.y += swimVerticalSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            desiredVelocity.y -= swimVerticalSpeed;
        }

        if (desiredDirection.sqrMagnitude <= 0.001f)
        {
            desiredVelocity = Vector3.zero;
        }

        movementVelocity = Vector3.Lerp(movementVelocity, desiredVelocity, Time.deltaTime * damping);

        if (characterController != null)
        {
            Vector3 gravityMotion = Physics.gravity * 0.15f * Time.deltaTime;
            characterController.Move((movementVelocity * Time.deltaTime) + gravityMotion);
        }
    }
}
