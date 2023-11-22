using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EM
{
    // The world audio.
    public class WorldAudio : MonoBehaviour
    {
        // Manager
        public WorldManager manager;

        // The BGM audio source.
        public AudioSource bgmSource;

        // The SFX audio source.
        public AudioSource sfxSource;

        // Start is called before the first frame update
        void Start()
        {
            if (manager == null)
                manager = WorldManager.Instance;
        }


    }
}