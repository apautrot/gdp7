using UnityEngine;
using System.Collections;

public class Sounds : SceneSingleton<Sounds>
{
	public AudioClip playerMove;
	public AudioClip playerMoveStart;
	public AudioClip playerAttack;
	public AudioClip playerAttackHit;
	public AudioClip playerExplosion;
	public AudioClip enemyMove1;
	public AudioClip enemyMove2;
	public AudioClip enemyMove3;
	public AudioClip enemyMove4;
	public AudioClip enemyAttackHit;
	public AudioClip enemyExplosion;
	public AudioClip motherKeenMove;
	public AudioClip motherActivation;
	public AudioClip motherDesactivation;
	public AudioClip bonusGrabbed;
	public AudioClip bonusGlow;
	public AudioClip gameTheme;
	
	public enum soundMode{
		Standard,
		Delay,
		Scheduled
	};
	
	public void Start() {
		this.PlaySound (gameTheme, soundMode.Standard, true, 1f);
	}
	
	public void FixedUpdate() {
		//Check if sound is over
		foreach (AudioSource source in this.gameObject.GetComponents<AudioSource>()) {
			if(null != source.clip && source.time >= source.clip.length) {
				this.removeAudioSource(source);
				Debug.Log("Clip is ended and removed" + source);
			}
		}
	}
	
	public AudioSource PlaySound(AudioClip clip, Sounds.soundMode soundMode, bool loop = false, float volume = 1.0f, float delay = 0f, bool allowMultiple = false) {
		AudioSource aSource;
		if (allowMultiple) {
			aSource = createAudioSource(clip);
		} 
		else {
			aSource = getAudioSource (clip);
			if (aSource.isPlaying) {
				aSource.Stop ();
				Debug.Log ("Sound is already playing");
			} 
		}
		
		handleSource(aSource,soundMode,loop,volume,delay);
		
		return aSource;
	}
	
	public void PlaySoundAt(AudioClip clip, Sounds.soundMode soundMode, Vector3 atPosition, bool loop = false, float volume = 1.0f, float delay = 0f, bool allowMultiple = false) {
		PlaySound (clip, soundMode, loop, volume, delay, allowMultiple).transform.position = atPosition;
	}
	
	/**
	 * Creates an audio source and assign choosen clip
	 */
	private AudioSource createAudioSource(AudioClip p_clip) {
		AudioSource asource = this.gameObject.AddComponent<AudioSource> ();
		asource.clip = p_clip;
		return asource;
	}
	
	/**
	 * Removes an audio clip
	 */
	private void removeAudioSource(AudioSource p_asource) {
		Destroy (p_asource);
	}
	
	/**
	 * Returns the audioSource depending the clip name
	 * Creates one if none exists.
	 */
	private AudioSource getAudioSource (AudioClip p_clip) {
		foreach (AudioSource audioSource in this.gameObject.GetComponents<AudioSource>()) {
			if(p_clip == audioSource.clip) {
				return audioSource;
			}
		}
		return createAudioSource(p_clip);
	}
	
	private void handleSource(AudioSource asource, Sounds.soundMode soundMode, bool loop = false, float volume = 1.0f, float delay = 0f){
		// loops or not
		asource.loop = loop;
		
		
		// change volume
		asource.volume = volume;
		
		switch (soundMode) {
		case Sounds.soundMode.Standard:
			asource.Play ();
			break;
		case Sounds.soundMode.Delay:
			asource.loop = false;
			asource.PlayDelayed (delay);
			break;
		case Sounds.soundMode.Scheduled:
			asource.loop = false;
			asource.PlayScheduled (delay);
			break;
		}
	}
	
}
