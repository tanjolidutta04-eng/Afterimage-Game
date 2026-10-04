using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Vector3 openOffset = new Vector3(0f, 4f, 0f);
    [SerializeField] private float moveSpeed = 3f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen;

    public bool IsOpen => isOpen;

    private void Awake()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
    }

    private void Update()
    {
        Vector3 targetPosition = isOpen ? openPosition : closedPosition;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    public void Open()
    {
        isOpen = true;
    }

    public void Close()
    {
        isOpen = false;
    }
}
