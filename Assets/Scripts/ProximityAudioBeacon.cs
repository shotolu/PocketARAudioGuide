using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProximityAudioBeacon : MonoBehaviour
{
    [Header("Detection")]
    public float triggerRadius = 1.5f;
    public float cooldownSeconds = 3f;

    [Header("Behavior")]
    public bool oneShotOnEnter = true;
    public bool loopWhileInside = false;

    private Transform _cameraTransform;
    private AudioSource _audioSource;
    private bool _wasInside = false;
    private float _lastPlayTime = -999f;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        var cam = Camera.main;
        if (cam != null)
        {
            _cameraTransform = cam.transform;
        }
    }

    void Update()
    {
        if (_cameraTransform == null)
        {
            var cam = Camera.main;
            if (cam != null)
            {
                _cameraTransform = cam.transform;
            }
            else
            {
                return;
            }
        }

        float distance = Vector3.Distance(_cameraTransform.position, transform.position);
        bool inside = distance <= triggerRadius;

        if (inside && !_wasInside)
        {
            // Just entered radius
            if (oneShotOnEnter && Time.time - _lastPlayTime > cooldownSeconds)
            {
                _audioSource.loop = false;
                _audioSource.Play();
                _lastPlayTime = Time.time;
            }

            if (loopWhileInside && !_audioSource.isPlaying)
            {
                _audioSource.loop = true;
                _audioSource.Play();
            }
        }
        else if (!inside && _wasInside)
        {
            // Just left radius
            if (loopWhileInside && _audioSource.isPlaying)
            {
                _audioSource.loop = false;
                _audioSource.Stop();
            }
        }

        _wasInside = inside;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}