using UnityEngine;
using System.Collections;

public class Sounds : SceneSingleton<Sounds>
{
	public AudioClip playerMove;
	public AudioClip playerMoveStart;
	public AudioClip playerAttack;
	public AudioClip playerAttackHit;
	public AudioClip playerExplosion;
	public AudioClip enemyMove;
	public AudioClip enemyAttackHit;
	public AudioClip enemyExplosion;
	public AudioClip motherKeenMove;
	public AudioClip motherActivation;
	public AudioClip motherDesactivation;
	public AudioClip bonusGrabbed;
	public AudioClip gameTheme;

	public enum soundMode{
		Standard,
		Delay,
		Scheduled
	};

	public void Start() {
		this.PlaySound (gameTheme, soundMode.Standard, true);
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

	public void PlaySound(AudioClip clip, Sounds.soundMode soundMode, bool loop = false, float volume = 1.0f, float delay = 0f) {
		AudioSource asource = getAudioSource (clip);

		if (asource.isPlaying) {
			asource.Stop();
			Debug.Log ("Sound is already playing");
		} 
			
		// loops or not
		if(loop) {
			asource.loop = true;
		}

		// change volume
		asource.volume = volume;

		switch (soundMode) {
		case Sounds.soundMode.Standard :
			asource.Play();
			break;
		case Sounds.soundMode.Delay :
			asource.loop = false;
			asource.PlayDelayed(delay);
			break;
		case Sounds.soundMode.Scheduled :
			asource.loop = false;
			asource.PlayScheduled(delay);
			break;
		}
	}

	/**
	 * Creates an audio source and assign choosen clip
	 */
	private AudioSource createAudioClip(AudioClip p_clip) {
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
		return createAudioClip(p_clip);
	}

}
