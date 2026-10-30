using UnityEngine;

// Plays the game's background music and sound effects
// Other scripts call this when something happens that should make a sound
public class SoundManager : MonoBehaviour
{
    // The background music and how loud it plays
    [SerializeField] AudioClip music;
    [SerializeField, Range(0f, 1f)] float musicVolume = 0.4f;

    // The sound effects and how loud they play
    [SerializeField] AudioClip customerDing;
    [SerializeField] AudioClip trashSound;
    [SerializeField, Range(0f, 1f)] float effectsVolume = 0.8f;

    AudioSource musicSource;
    AudioSource effectsSource;

    // Creates one speaker for the music and one for sound effects
    void Awake()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;

        effectsSource = gameObject.AddComponent<AudioSource>();
        effectsSource.playOnAwake = false;
    }

    // Starts the music when the game begins
    void Start()
    {
        if (music != null) musicSource.Play();
    }

    // Plays the bell when a new customer steps up
    public void PlayCustomerDing()
    {
        PlayEffect(customerDing);
    }

    // Plays the sound for throwing a burger in the trash
    public void PlayTrash()
    {
        PlayEffect(trashSound);
    }

    // Plays any sound effect once, without cutting off sounds that are already playing
    public void PlayEffect(AudioClip clip)
    {
        if (clip != null) effectsSource.PlayOneShot(clip, effectsVolume);
    }
}