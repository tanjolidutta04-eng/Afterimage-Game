using UnityEngine;

public class PressurePlate : AfterimageInteractable
{
    [SerializeField] private DoorController linkedDoor;
    [SerializeField] private Renderer plateRenderer;
    [SerializeField] private Color activeColor = new Color(0.15f, 1f, 0.7f, 1f);
    [SerializeField] private Color inactiveColor = new Color(0.4f, 0.4f, 0.4f, 1f);

    private bool isActive;

    public bool IsActive => isActive;

    private void Reset()
    {
        if (GetComponent<BoxCollider>() == null)
        {
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
        }

        if (plateRenderer == null)
        {
            plateRenderer = GetComponent<Renderer>();
        }
    }

    private void Awake()
    {
        if (plateRenderer == null)
        {
            plateRenderer = GetComponent<Renderer>();
        }

        if (plateRenderer != null)
        {
            plateRenderer.material.color = inactiveColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsValidTriggerSource(other))
        {
            Activate();
        }
    }

    public override void OnPlayerInteraction()
    {
        Activate();
    }

    public override void OnAfterimageInteraction()
    {
        Activate();
    }

    public void Activate()
    {
        if (isActive)
        {
            return;
        }

        isActive = true;

        if (plateRenderer != null)
        {
            plateRenderer.material.color = activeColor;
        }

        if (linkedDoor != null)
        {
            linkedDoor.Open();
        }
    }

    public void SetLinkedDoor(DoorController door)
    {
        linkedDoor = door;
    }

    private bool IsValidTriggerSource(Collider other)
    {
        if (other == null)
        {
            return false;
        }

        return other.CompareTag("Player") || other.GetComponent<AfterimagePlayback>() != null;
    }
}
