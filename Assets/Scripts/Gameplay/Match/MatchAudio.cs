using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EM
{
    public class MatchAudio : MonoBehaviour
    {
        // The manager
        public MatchManager manager;

        // The BGM source.
        public AudioSource bgmSource;

        // The BGM source control.
        public AudioSourceLooper bgmSourceLooper;

        // The SFX source.
        public AudioSource sfxSource;

        [Header("Audio Clips")]

        // The normal BGM
        public AudioClip normalMatchBgm;

        // Normal BGM Loop
        public Vector2 normalMatchLoopPoints;

        // The final match BGM
        public AudioClip bossMatchBgm;

        // Boss match loop
        public Vector2 bossMatchLoopPoints;

        // The results BGM.
        public AudioClip resultsBgm;

        // Match Results loop
        public Vector2 resultsLoopPoints;

        // Start is called before the first frame update
        void Start()
        {
            if(manager == null)
                manager = MatchManager.Instance;
        }

        // Normal Match BGM
        public void PlayNormalMatchBgm()
        {
            bgmSource.Stop();

            // Loop Points
            bgmSourceLooper.clipStart = normalMatchLoopPoints.x;
            bgmSourceLooper.clipEnd = normalMatchLoopPoints.y;

            // Replace and play clip.
            bgmSource.clip = normalMatchBgm;
            bgmSource.Play();
        }

        // Final Match BGM
        public void PlayBossMatchBgm()
        {
            // Stops Audio
            bgmSource.Stop();

            // Loop Points
            bgmSourceLooper.clipStart = bossMatchLoopPoints.x;
            bgmSourceLooper.clipEnd = bossMatchLoopPoints.y;

            // Replace and play clip.
            bgmSource.clip = bossMatchBgm;
            bgmSource.Play();
        }

        // Results BGM
        public void PlayResultsBgm()
        {
            // Stops Audio
            bgmSource.Stop();

            // Loop Points
            bgmSourceLooper.clipStart = resultsLoopPoints.x;
            bgmSourceLooper.clipEnd = resultsLoopPoints.y;


            // Replace and play clips.
            bgmSource.clip = resultsBgm;
            bgmSource.Play();
        }
    }
}