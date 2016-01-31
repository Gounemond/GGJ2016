using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    // Singleton Implementation
    protected static AudioManager _self;
    public static AudioManager Self
    {
        get
        {
            if (_self == null)
                _self = FindObjectOfType(typeof(AudioManager)) as AudioManager;
            return _self;
        }
    }

    public AudioSource generalAudioSource;
    public AudioSource generalAudioSource2;
    public AudioSource spider1TinderSource;
    public AudioSource spider2TinderSource;
    public AudioClip playerReady;
    public AudioClip swhooshSmartPhone;
    public AudioClip poseSelected;
    public AudioClip photoShoot;
    public AudioClip tinderScreenIn;
    public AudioClip tinderLike;
    public AudioClip tinderNope;
    public AudioClip tinderSwoosh;
    public AudioClip finishedGame;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayerReady()
    {
        generalAudioSource.PlayOneShot(playerReady);
    }

    public void SmartPhoneAppears()
    {
        generalAudioSource.PlayOneShot(swhooshSmartPhone);
    }

    public void PoseSelected()
    {
        generalAudioSource.PlayOneShot(poseSelected);
    }

    public void PhotoShoot()
    {
        generalAudioSource.PlayOneShot(photoShoot);
    }

    public void TinderScreenIn()
    {
        generalAudioSource.PlayOneShot(tinderScreenIn);
    }

    public void TinderPlayer1(bool isLike)
    {
        if (isLike)
        {
            spider1TinderSource.PlayOneShot(tinderLike);
        }
        else
        {
            spider1TinderSource.PlayOneShot(tinderNope);
        }
    }

    public void TinderPlayer2(bool isLike)
    {
        if (isLike)
        {
            spider2TinderSource.PlayOneShot(tinderLike);
        }
        else
        {
            spider2TinderSource.PlayOneShot(tinderNope);
        }
    }

    public void TinderSwoosh()
    {
        generalAudioSource2.PlayOneShot(tinderSwoosh);
    }

    public void FinishedGame()
    {
        generalAudioSource.PlayOneShot(finishedGame);
    }

}
