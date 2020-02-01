﻿using System;
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
	
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.H))
		{
			PlayAudioClip("Test");
		}

		if (Input.GetKeyDown(KeyCode.O))
		{
			PlayAudioClipOneShot("Test");
		}
	}

	private void PlayAudioClip(string key)
	{
		var audioSource = gameObject.AddComponent<AudioSource>();

		audioClips[key].mixer = mixer;
		audioClips[key].Play(audioSource);
	}

	private void PlayAudioClipOneShot(string key)
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
		soundTracks[themeSongToStop].Stop();
		soundTracks[songOnAir].Play();
		//yield return new WaitForSeconds(soundTracks[songOnAir].clip.length);
		soundTracks[themeSongToStart].PlayDelayed(soundTracks[songOnAir].clip.length);
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