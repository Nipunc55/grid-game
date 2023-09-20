using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource clickSound;

    public void ClickSound()
    {
        clickSound.Play();
    }

}
