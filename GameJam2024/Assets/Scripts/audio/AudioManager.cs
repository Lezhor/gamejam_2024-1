using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace audio
{
    public class AudioManager : MonoBehaviour
    {

        private static AudioManager _instance;

        public AudioManager Instance => _instance;

        public Sound[] sounds;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;

                if (s.playOnAwake)
                {
                    s.source.Play();
                }
            }
        }

        public void Play(String sound, float delay)
        {
            Sound s = Array.Find(sounds, s => s.name == sound);
            if (s == null)
            {
                Debug.LogWarning("Tried to play non-existent sound: '" + sound + "'");
            }
            else
            {
                if (delay == 0f)
                {
                    s.source.Play();
                }
                else
                {
                    s.source.PlayDelayed(delay);
                }
            }
        }

        public void Play(String sound)
        {
            Play(sound, 0f);
        }

        [Serializable]
        public class Sound
        {
            public string name;
            public AudioClip clip;
            [Range(0f, 1f)]
            public float volume = 1f;
            [Range(.1f, 3f)]
            public float pitch = 1f;

            public bool playOnAwake = false;
            public bool loop;

            [HideInInspector]
            public AudioSource source;
        }
    }
}
