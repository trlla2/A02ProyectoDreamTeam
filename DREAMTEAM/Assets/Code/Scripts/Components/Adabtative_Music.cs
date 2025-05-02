using UnityEngine;
public class Adabtative_Music : MonoBehaviour
{
    [Header("SETUP")]
    [SerializeField] private AudioSource base_Theme;
    [SerializeField] private AudioSource trumpets;
    [SerializeField] private AudioSource marimba;
    [SerializeField] private AudioSource tuba;
    [SerializeField] private AudioSource agut;
    [SerializeField] private AudioSource kick;


    private void Start()
    {
        GameManager.Instance.OnMusicLevelChanging += ChangeMusicLevel; // Subscribe to the event

        ChangeMusicLevel(0); //reset AS
    }

    private void ChangeMusicLevel(int musicLevel)
    {
        switch (musicLevel)
        {
            case 0:
                base_Theme.mute = false;
                trumpets.mute = true;
                marimba.mute = true;
                tuba.mute = true;
                agut.mute = true;
                kick.mute = true;
                break;
            case 1:
                base_Theme.mute = false;
                trumpets.mute = true;
                marimba.mute = true;
                tuba.mute = true;
                agut.mute = true;
                kick.mute = false;
                break;
            case 2:
                base_Theme.mute = false;
                trumpets.mute = true;
                marimba.mute = true;
                tuba.mute = true;
                agut.mute = false;
                kick.mute = false;
                break;
            case 3:
                base_Theme.mute = false;
                trumpets.mute = true;
                marimba.mute = false;
                tuba.mute = true;
                agut.mute = false;
                kick.mute = true;
                break;
            case 4:
                base_Theme.mute = false;
                trumpets.mute = true;
                marimba.mute = false;
                tuba.mute = true;
                agut.mute = false;
                kick.mute = false;
                break;
            case 5:
                base_Theme.mute = false;
                trumpets.mute = false;
                marimba.mute = false;
                tuba.mute = true;
                agut.mute = false;
                kick.mute = false;
                break;
            case 6:
                base_Theme.mute = false;
                trumpets.mute = false;
                marimba.mute = true;
                tuba.mute = false;
                agut.mute = true;
                kick.mute = false;
                break;
            default:
                base_Theme.mute = false;
                trumpets.mute = false;
                marimba.mute = false;
                tuba.mute = false;
                agut.mute = false;
                kick.mute = false;
                break;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnMusicLevelChanging -= ChangeMusicLevel; // Unsubscribe to the event
    }
}
