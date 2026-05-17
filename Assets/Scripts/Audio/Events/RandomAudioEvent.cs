using UnityEngine;

namespace Audio.Events
{
    [CreateAssetMenu(menuName = "Audio/Random Audio Event")]
    public class RandomAudioEvent : ScriptableObject
    {
        public AudioClip[] clips;
        public float volume = 1f;
        public float pitchRange = 0.1f;

        public void Play(AudioSource source)
        {
            if (clips.Length == 0) return;
            source.clip = clips[Random.Range(0, clips.Length)];
            source.volume = volume;
            source.pitch = 1f + Random.Range(-pitchRange, pitchRange);
            source.Play();
        }
    }
}
