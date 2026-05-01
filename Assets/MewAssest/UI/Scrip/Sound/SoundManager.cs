using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private int collectSoundIndex = 0;
    private int punchSoundIndex = 1;
    private int dashSoundIndex = 2;
    private int healSoundIndex = 3;
    private int playerClankSoundIndex = 4;
    private int enemyClankSoundIndex = 5;
    private int jumpSoundIndex = 6;
    public List<AudioClip> clips = new List<AudioClip>();
    private AudioSource audioSource;

    private static SoundManager StaticInstance = null;
    public static SoundManager GetStatic()
    {
        return StaticInstance;
    }
    void Awake()
    {
        if(StaticInstance != null)
        {
            Destroy(this.gameObject);
        }
        StaticInstance = this;

        audioSource = GetComponent<AudioSource>();
    }
    public void OnCollectSound()
    {
        audioSource.PlayOneShot(clips[collectSoundIndex]);
    }
    public void OnPunchSound()
    {
        audioSource.PlayOneShot(clips[punchSoundIndex]);
    }

    public void OnDashSound()
    {
        audioSource.PlayOneShot(clips[dashSoundIndex]);
    }

    public void OnHealSound()
    {
        audioSource.PlayOneShot(clips[healSoundIndex]);
    }

    public void OnPlayerClankSound()
    {
        audioSource.PlayOneShot(clips[playerClankSoundIndex]);
    }
    public void OnEnemyClankSound()
    {
        audioSource.PlayOneShot(clips[enemyClankSoundIndex]);
    }
    public void OnJumpSound()
    {
        audioSource.PlayOneShot(clips[jumpSoundIndex]);
    }
}
