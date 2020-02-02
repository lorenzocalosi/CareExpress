using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Object = System.Object;

public class SoundManager : Singleton<SoundManager>
{
	public AudioMixerGroup mixer;
	public Dictionary<string, Sound> audioClips;
	public Dictionary<string, AudioSource> soundTracks;

	private void Start()
	{
		soundTracks["Walking"].Play();
		soundTracks["Talking"].Play();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			soundTracks["Walking"].Stop();
		}
	}

	public void PlayAudioClip(string key)
	{
		var audioSource = gameObject.AddComponent<AudioSource>();

		audioClips[key].mixer = mixer;
		audioClips[key].Play(audioSource);
	}

	public void PlayAudioClipOneShot(string key)
	{
		var audioSource = gameObject.AddComponent<AudioSource>();

		audioClips[key].mixer = mixer;
		audioClips[key].PlayOneShot(audioSource);
	}

	public void InterruptSoundTrackAndPlayOther(string toInterrupt, string toPlay)
	{
		soundTracks[toInterrupt].mute = true;
		soundTracks[toPlay].mute = false;
	}

	public void StartMapThemeAfterTime(string songOnAir, string themeSongToStop, string themeSongToStart)
	{
		if (themeSongToStop.Contains(",")) {
			string[] songsToStop = themeSongToStop.Split(',');
			for (int i = 0; i < songsToStop.Length; i++) {
				if (soundTracks.ContainsKey(songsToStop[i])) {
					soundTracks[songsToStop[i]].Stop();
				}
			}
		}
		else {
			soundTracks[themeSongToStop].Stop();
		}
		soundTracks[songOnAir].Play();
		//yield return new WaitForSeconds(soundTracks[songOnAir].clip.length);
		soundTracks[themeSongToStart].PlayDelayed(soundTracks[songOnAir].clip.length);
	}

	public void PlayButtonHighlightedSound()
	{
		PlayAudioClipOneShot("ButtonHighlight");
	}
}

[Serializable]
public class Sound
{
	public AudioClip audioClip;
	public float pitch = 1;
	public float volume = 1;
	public AudioMixerGroup mixer;

	public void Play(AudioSource audioSource)
	{
		audioSource.clip = audioClip;
		audioSource.pitch = pitch;
		audioSource.volume = volume;
		audioSource.outputAudioMixerGroup = mixer;

		audioSource.Play();
		UnityEngine.Object.Destroy(audioSource, audioSource.clip.length);
	}

	public void PlayOneShot(AudioSource audioSource)
	{
		audioSource.clip = audioClip;
		audioSource.pitch = pitch;
		audioSource.volume = volume;
		audioSource.outputAudioMixerGroup = mixer;

		audioSource.PlayOneShot(audioSource.clip);
		UnityEngine.Object.Destroy(audioSource, audioSource.clip.length);
	}
}