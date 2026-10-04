using System.Collections.Generic;
using UnityEngine;

public struct AfterimageSample
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Direction;
    public float Speed;
    public float Time;
    public bool IsInteraction;
    public string InteractionName;

    public AfterimageSample(Vector3 position, Quaternion rotation, Vector3 direction, float speed, float time, bool isInteraction = false, string interactionName = "")
    {
        Position = position;
        Rotation = rotation;
        Direction = direction;
        Speed = speed;
        Time = time;
        IsInteraction = isInteraction;
        InteractionName = interactionName;
    }
}

public class AfterimageRecorder : MonoBehaviour
{
    [SerializeField] private float recordingDuration = 10f;
    [SerializeField] private float sampleInterval = 0.05f;
    [SerializeField] private bool recordOnStart = true;

    private readonly List<AfterimageSample> samples = new List<AfterimageSample>();
    private PlayerController playerController;
    private float elapsedTime;
    private float nextSampleClock;

    public bool IsRecording { get; private set; }
    public float RecordingDuration => recordingDuration;
    public float ElapsedTime => elapsedTime;
    public IReadOnlyList<AfterimageSample> Samples => samples;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
    }

    private void Start()
    {
        if (recordOnStart)
        {
            BeginRecording();
        }
    }

    private void Update()
    {
        if (!IsRecording)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        if (Time.time >= nextSampleClock)
        {
            CaptureSample();
            nextSampleClock = Time.time + sampleInterval;
        }

        if (elapsedTime >= recordingDuration)
        {
            StopRecording();
        }
    }

    public void BeginRecording()
    {
        samples.Clear();
        elapsedTime = 0f;
        nextSampleClock = Time.time;
        IsRecording = true;
        CaptureSample();
    }

    public void StopRecording()
    {
        IsRecording = false;

        if (samples.Count > 0)
        {
            CaptureSample();
        }
    }

    public void RecordInteraction(string interactionName)
    {
        if (!IsRecording || playerController == null)
        {
            return;
        }

        samples.Add(new AfterimageSample(
            transform.position,
            transform.rotation,
            playerController.CurrentMoveDirection,
            playerController.CurrentSpeed,
            elapsedTime,
            true,
            interactionName));
    }

    public AfterimagePlayback CreateAfterimage(GameObject prefab = null)
    {
        if (samples.Count == 0)
        {
            return null;
        }

        GameObject ghostObject = prefab != null ? Instantiate(prefab) : new GameObject("Afterimage");
        ghostObject.transform.position = samples[0].Position;
        ghostObject.transform.rotation = samples[0].Rotation;

        AfterimagePlayback afterimage = ghostObject.GetComponent<AfterimagePlayback>();
        if (afterimage == null)
        {
            afterimage = ghostObject.AddComponent<AfterimagePlayback>();
        }

        afterimage.Play(samples);
        return afterimage;
    }

    private void CaptureSample()
    {
        if (playerController == null)
        {
            return;
        }

        samples.Add(new AfterimageSample(
            transform.position,
            transform.rotation,
            playerController.CurrentMoveDirection,
            playerController.CurrentSpeed,
            elapsedTime,
            false,
            string.Empty));
    }
}
