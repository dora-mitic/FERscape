using UnityEngine;

public class RobotSoundManager : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioClip movementSound;
    [SerializeField] private AudioClip bump1Sound;
    [SerializeField] private AudioClip bump2Sound;
    [SerializeField] private AudioClip bump3Sound;
    [SerializeField] private AudioClip robotYaySound;

    [Header("Settings")]
    [SerializeField] private float movementVolume = 0.5f;
    [SerializeField] private float bumpVolume = 0.7f;
    [SerializeField] private float yayVolume = 1f;

    private AudioSource movementAudioSource;
    private AudioSource effectAudioSource;
    private bool isMoving = false;

    void Start()
    {
        // Create two audio sources - one for looping movement, one for effects
        movementAudioSource = gameObject.AddComponent<AudioSource>();
        movementAudioSource.clip = movementSound;
        movementAudioSource.loop = true;
        movementAudioSource.volume = movementVolume;
        movementAudioSource.playOnAwake = false;

        effectAudioSource = gameObject.AddComponent<AudioSource>();
        effectAudioSource.playOnAwake = false;
    }

    void Update()
    {
        // Check if robot is moving
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        bool movingNow = (moveX != 0 || moveY != 0);

        // Start or stop movement sound based on movement
        if (movingNow && !isMoving)
        {
            PlayMovementSound();
        }
        else if (!movingNow && isMoving)
        {
            StopMovementSound();
        }

        isMoving = movingNow;
    }

    public void PlayMovementSound()
    {
        if (movementAudioSource != null && movementSound != null && !movementAudioSource.isPlaying)
        {
            movementAudioSource.Play();
        }
    }

    public void StopMovementSound()
    {
        if (movementAudioSource != null && movementAudioSource.isPlaying)
        {
            movementAudioSource.Stop();
        }
    }

    public void PlayBumpSound()
    {
        if (effectAudioSource == null)
        {
            Debug.LogError("Effect AudioSource is null!");
            return;
        }

        // Collect available bump sounds
        AudioClip[] bumpSounds = { bump1Sound, bump2Sound, bump3Sound };
        AudioClip[] availableBumps = System.Array.FindAll(bumpSounds, clip => clip != null);

        if (availableBumps.Length == 0)
        {
            Debug.LogWarning("No bump sounds assigned! Please assign bump1, bump2, and bump3 in the Inspector.");
            return;
        }

        // Randomly pick one of the available bump sounds
        AudioClip selectedBump = availableBumps[Random.Range(0, availableBumps.Length)];

        Debug.Log($"Playing bump sound: {selectedBump.name} at volume {bumpVolume}");
        effectAudioSource.PlayOneShot(selectedBump, bumpVolume);
    }

    public void PlayYaySound()
    {
        if (effectAudioSource != null && robotYaySound != null)
        {
            effectAudioSource.PlayOneShot(robotYaySound, yayVolume);
        }
    }
}