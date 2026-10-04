using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerController playerController;
    [SerializeField] private AfterimageRecorder afterimageRecorder;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject afterimagePrefab;
    [SerializeField] private DoorController doorController;
    [SerializeField] private PressurePlate pressurePlate;

    private AfterimagePlayback activeAfterimage;

    public PlayerController PlayerController => playerController;
    public AfterimageRecorder Recorder => afterimageRecorder;
    public UIManager UiManager => uiManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        ResolveReferences();

        if (afterimageRecorder != null)
        {
            afterimageRecorder.BeginRecording();
        }

        if (uiManager != null)
        {
            uiManager.UpdateStatus("Recording movement...");
        }
    }

    private void Update()
    {
        if (afterimageRecorder != null && afterimageRecorder.IsRecording && afterimageRecorder.ElapsedTime >= afterimageRecorder.RecordingDuration)
        {
            FinalizeRecording();
        }

        if (Input.GetKeyDown(KeyCode.R) && afterimageRecorder != null && !afterimageRecorder.IsRecording)
        {
            afterimageRecorder.BeginRecording();

            if (uiManager != null)
            {
                uiManager.UpdateStatus("Recording movement...");
            }
        }
    }

    public void TryInteract()
    {
        if (pressurePlate != null)
        {
            pressurePlate.OnPlayerInteraction();
        }

        if (uiManager != null)
        {
            uiManager.UpdateStatus("Interaction triggered");
        }
    }

    public void FinalizeRecording()
    {
        if (afterimageRecorder == null)
        {
            return;
        }

        afterimageRecorder.StopRecording();

        if (activeAfterimage != null)
        {
            Destroy(activeAfterimage.gameObject);
        }

        activeAfterimage = afterimageRecorder.CreateAfterimage(afterimagePrefab);

        if (uiManager != null)
        {
            uiManager.UpdateStatus("Afterimage ready");
        }
    }

    private void ResolveReferences()
    {
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        if (afterimageRecorder == null)
        {
            afterimageRecorder = FindObjectOfType<AfterimageRecorder>();
        }

        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        if (doorController == null)
        {
            doorController = FindObjectOfType<DoorController>();
        }

        if (pressurePlate == null)
        {
            pressurePlate = FindObjectOfType<PressurePlate>();
        }
    }
}
