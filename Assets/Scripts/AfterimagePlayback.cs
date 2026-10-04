using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AfterimagePlayback : MonoBehaviour
{
    [SerializeField] private float playbackSpeed = 1f;
    [SerializeField] private bool loopPlayback;
    [SerializeField] private Color ghostTint = new Color(0.4f, 0.9f, 1f, 0.45f);

    private readonly List<AfterimageSample> samples = new List<AfterimageSample>();
    private Renderer ghostRenderer;
    private float playbackTime;
    private bool isPlaying;

    public bool IsPlaying => isPlaying;

    private void Awake()
    {
        EnsureGhostVisual();
    }

    public void Play(IReadOnlyList<AfterimageSample> playbackSamples)
    {
        samples.Clear();

        for (int i = 0; i < playbackSamples.Count; i++)
        {
            samples.Add(playbackSamples[i]);
        }

        if (samples.Count == 0)
        {
            return;
        }

        playbackTime = 0f;
        isPlaying = true;
        transform.position = samples[0].Position;
        transform.rotation = samples[0].Rotation;
    }

    public void StopPlayback()
    {
        isPlaying = false;
    }

    private void Update()
    {
        if (!isPlaying || samples.Count < 2)
        {
            return;
        }

        playbackTime += Time.deltaTime * playbackSpeed;

        if (playbackTime >= samples[samples.Count - 1].Time)
        {
            if (loopPlayback)
            {
                playbackTime = 0f;
            }
            else
            {
                isPlaying = false;
                return;
            }
        }

        ApplySample(playbackTime);
    }

    private void ApplySample(float sampleTime)
    {
        if (samples.Count == 0)
        {
            return;
        }

        if (samples.Count == 1)
        {
            transform.position = samples[0].Position;
            transform.rotation = samples[0].Rotation;
            return;
        }

        AfterimageSample previous = samples[0];
        AfterimageSample next = samples[samples.Count - 1];

        for (int i = 1; i < samples.Count; i++)
        {
            if (samples[i].Time >= sampleTime)
            {
                next = samples[i];
                break;
            }

            previous = samples[i];
        }

        float range = Mathf.Max(next.Time - previous.Time, 0.0001f);
        float interpolation = (sampleTime - previous.Time) / range;

        transform.position = Vector3.Lerp(previous.Position, next.Position, interpolation);
        transform.rotation = Quaternion.Slerp(previous.Rotation, next.Rotation, interpolation);
    }

    private void EnsureGhostVisual()
    {
        Renderer existingRenderer = GetComponent<Renderer>();
        if (existingRenderer != null)
        {
            ghostRenderer = existingRenderer;
        }
        else
        {
            GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            visual.name = "Ghost Mesh";
            visual.transform.SetParent(transform, false);
            Destroy(visual.GetComponent<Collider>());
            ghostRenderer = visual.GetComponent<Renderer>();
        }

        if (ghostRenderer != null)
        {
            ghostRenderer.material = new Material(Shader.Find("Standard"));
            ghostRenderer.material.color = ghostTint;
            ghostRenderer.material.SetFloat("_Mode", 3f);
            ghostRenderer.material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            ghostRenderer.material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            ghostRenderer.material.SetInt("_ZWrite", 0);
            ghostRenderer.material.DisableKeyword("_ALPHATEST_ON");
            ghostRenderer.material.EnableKeyword("_ALPHABLEND_ON");
            ghostRenderer.material.renderQueue = 3000;
        }
    }
}
