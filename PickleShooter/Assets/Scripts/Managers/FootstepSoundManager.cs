using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSoundManager : MonoBehaviour
{
    [System.Serializable]
    public class SurfaceSounds
    {
        public PhysicMaterial surfaceMaterial;
        public AudioClip[] footstepSounds;
    }

    public SurfaceSounds[] surfaceSounds;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstepSound(PhysicMaterial surfaceMaterial)
    {
        foreach (var surfaceSound in surfaceSounds)
        {
            if (surfaceSound.footstepSounds.Length > 0)
            {
                AudioClip sound = surfaceSound.footstepSounds[Random.Range(0, surfaceSound.footstepSounds.Length)];
                audioSource.PlayOneShot(sound);
            }
            else
            {
                Debug.LogWarning("No footstep sounds assigned for the surface material: " + surfaceSound.surfaceMaterial.name);
            }
        }
    }
}
