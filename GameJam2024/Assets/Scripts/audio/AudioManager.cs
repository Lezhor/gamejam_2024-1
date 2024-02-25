using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace audio
{
    public class AudioManager : MonoBehaviour
    {

        private static AudioManager _instance;

        public AudioManager Instance => _instance;

        [SerializeField] private AudioMixer audioMixer;

        public float MasterVolume
        {
            get => audioMixer.GetFloat("masterVolume", out var value) ? Mathf.Pow(10f, value / 20f) : 1f;
            set => audioMixer.SetFloat("masterVolume", Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f));
        }
        
        public float MusicVolume
        {
            get => audioMixer.GetFloat("musicVolume", out var value) ? Mathf.Pow(10f, value / 20f) : 1f;
            set => audioMixer.SetFloat("musicVolume", Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f));
        }
        
        public float SoundFXVolume
        {
            get => audioMixer.GetFloat("sfxVolume", out var value) ? Mathf.Pow(10f, value / 20f) : 1f;
            set => audioMixer.SetFloat("sfxVolume", Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f));
        }

        public SoundGroup[] soundGroups;

        private Sound[] _sounds;

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
            
            List<Sound> soundList = new List<Sound>();
            
            foreach (SoundGroup soundGroup in soundGroups)
            foreach (Sound sound in soundGroup.sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.outputAudioMixerGroup = soundGroup.audioMixerGroup;
                sound.source.clip = sound.clip;

                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;

                if (sound.playOnAwake)
                {
                    sound.source.Play();
                }
                
                soundList.Add(sound);
            }

            _sounds = soundList.ToArray();
        }

        public void Play(String sound, float delay)
        {
            Sound s = Array.Find(_sounds, s => s.name == sound);
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

        [Serializable]
        public class SoundGroup
        {
            public AudioMixerGroup audioMixerGroup;
            public Sound[] sounds;
        }
    }
}
