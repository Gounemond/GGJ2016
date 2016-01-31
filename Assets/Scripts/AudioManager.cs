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
    public AudioClip playerReady;
    public AudioClip swhooshSmartPhone;
    public AudioClip poseSelected;

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
}
