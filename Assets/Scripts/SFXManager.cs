using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    
    private AudioSource audioSource;
    
    public AudioClip buttonClick;      // Ovo će biti "book-closing-48184"
    public AudioClip doorOpen;         // Ovo će biti "door-opening-350028"
    public AudioClip drawerOpen;       // Ovo će biti "drawer-open-98801"
    public AudioClip electricDamage;   // Ovo će biti "qubodupElectricityDamage01"
    public AudioClip soundEffect;      // Ovo će biti "sound-effect-4971"

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
            
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(buttonClick);
    }

    public void PlayDoorOpen()
    {
        audioSource.PlayOneShot(doorOpen);
    }

    public void PlayDrawerOpen()
    {
        audioSource.PlayOneShot(drawerOpen);
    }

    public void PlayElectricDamage()
    {
        audioSource.PlayOneShot(electricDamage);
    }

    public void PlayEffect()
    {
        audioSource.PlayOneShot(soundEffect);
    }
}
