using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour {   //Será uma classe Singleton
    private static SoundController instance;

    [SerializeField] private float timeFadeInTrack, timeFadeOutTrack, timeFadeBetweenTracks, SFXVoume=0.7f;

    public Sound[] sounds;

    private Dictionary<string, bool> isPlayingOST = new Dictionary<string, bool>();
    private Dictionary<string, float> originalVolumesOSTs = new Dictionary<string, float>();
    private Dictionary<string, float> currentVolumesOSTs = new Dictionary<string, float>();
    private Dictionary<string, float> originalVolumesSFXs = new Dictionary<string, float>();
    private Dictionary<string, float> currentVolumesSFXs = new Dictionary<string, float>();

    public static SoundController GetInstance() {
        return instance;
    }

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void LoadSounds() {
        foreach (Sound s in sounds) {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.clip.name = s.name;
            //s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
            s.audioSource.playOnAwake = false;
            if (s.is3D) {
                s.audioSource.spatialBlend = 1f;
                s.audioSource.maxDistance = 100f;
                s.audioSource.minDistance = 10f;
                s.audioSource.rolloffMode = AudioRolloffMode.Linear;
            }

            if (!s.isOST) {
                s.audioSource.volume = SFXVoume;
                currentVolumesSFXs[s.name] = SFXVoume;
                originalVolumesSFXs[s.name] = SFXVoume;
            }
            else {
                s.audioSource.volume = s.volume;
                currentVolumesOSTs[s.name] = s.volume;
                originalVolumesOSTs[s.name] = s.volume;
            }
        }
    }

    public void PlaySound(string soundName) {
        Sound s = Array.Find(sounds, sound => sound.name.Equals(soundName));   //Procurando o som informado pelo seu nome
        if (s != null) {
            if (soundName.Contains("OST"))   //Se eu estiver tentando mudar a música de fundo
                SwapTrack(soundName);
            else {
                AudioSource[] audios = gameObject.GetComponents<AudioSource>();
                audios.FirstOrDefault(a => a.clip.name.Equals(soundName)).Play();
            }
        }
    }

    public void StopSound(string soundName, GameObject go) {
        Sound s = Array.Find(sounds, sound => sound.name.Equals(soundName));   //Procurando o som informado pelo seu nome
        if (s != null) {
            if (go != null) {
                AudioSource[] audios = go.GetComponents<AudioSource>();
                audios.FirstOrDefault(a => a.clip.name.Equals(soundName)).Stop();
            }
            else {
                AudioSource[] audios = gameObject.GetComponents<AudioSource>();
                audios.FirstOrDefault(a => a.clip.name.Equals(soundName)).Stop();
            }
        }
    }


    private void SwapTrack(string nameNewOST) {    //Método para trocar músicas de forma suave
        string nameOSTPlaying = "";
        foreach (KeyValuePair<string, bool> valuePair in isPlayingOST) {
            if (valuePair.Value == true) {
                nameOSTPlaying = valuePair.Key;   //Pegando o nome da trilha que está tocando
                break;
            }
        }
        if (nameOSTPlaying != nameNewOST) {
            AudioSource[] audios = gameObject.GetComponents<AudioSource>();
            AudioSource newOST = audios.FirstOrDefault(a => a.clip.name.Equals(nameNewOST));
            AudioSource oldOST = null;
            if (nameOSTPlaying != "")
                oldOST = audios.FirstOrDefault(a => a.clip.name.Equals(nameOSTPlaying));

            StopAllCoroutines();
            StartCoroutine(FadeBetweenTracks(oldOST, newOST));
            if(nameOSTPlaying != "")
                isPlayingOST[nameOSTPlaying] = false;
            isPlayingOST[nameNewOST] = true;
        }
    }


    private IEnumerator FadeBetweenTracks(AudioSource oldOST, AudioSource newOST) {    //Esta co-rotina será usada para transiocionar entre uma música e outra
        float timeElapsed = 0;
        float volumeNewOst = currentVolumesOSTs[newOST.clip.name], volumeOldOST = 1;
        if (oldOST != null)
            volumeOldOST = currentVolumesOSTs[oldOST.clip.name];

        newOST.volume = 0;
        newOST.Play();
        while (timeElapsed < timeFadeBetweenTracks) {
            if (oldOST != null)
                oldOST.volume = Mathf.Lerp(volumeOldOST, 0, timeElapsed / timeFadeBetweenTracks);
            newOST.volume = Mathf.Lerp(0, volumeNewOst, timeElapsed / timeFadeBetweenTracks);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        if (oldOST != null)
            oldOST.Stop();
    }

    private IEnumerator FadeInTrack(AudioSource track) {    //Esta co-rotina será usada para iniciar uma trilha de forma suave
        float timeElapsed = 0;
        float newVolume = currentVolumesOSTs[track.clip.name];

        track.volume = 0;
        track.Play();
        while (timeElapsed < timeFadeInTrack) {
            track.volume = Mathf.Lerp(0, newVolume, timeElapsed / timeFadeInTrack);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeOutTrack(AudioSource track) {    //Esta co-rotina será usada para parar uma trilha de forma suave
        float timeElapsed = 0;
        float trackVolume = currentVolumesOSTs[track.clip.name];
        //Debug.Log(track + "  " + trackVolume);

        while (timeElapsed < timeFadeOutTrack) {
            track.volume = Mathf.Lerp(trackVolume, 0, timeElapsed / timeFadeOutTrack);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        track.Pause();
    }


    public void PauseCurrentTrack() {
        string nameOSTPlaying = "";
        foreach (KeyValuePair<string, bool> valuePair in isPlayingOST) {
            if (valuePair.Value == true) {
                nameOSTPlaying = valuePair.Key;   //Pegando o nome da trilha que está tocando
                break;
            }
        }
        if (nameOSTPlaying != "") {
            AudioSource[] audios = gameObject.GetComponents<AudioSource>();
            AudioSource currentTrack = audios.FirstOrDefault(a => a.clip.name.Equals(nameOSTPlaying));
            StartCoroutine(FadeOutTrack(currentTrack));
        }
    }

    public void ResumeCurrentTrack() {
        string nameOSTPlaying = "";
        foreach (KeyValuePair<string, bool> valuePair in isPlayingOST) {
            if (valuePair.Value == true) {
                nameOSTPlaying = valuePair.Key;   //Pegando o nome da trilha que está tocando
                break;
            }
        }
        if (nameOSTPlaying != "") {
            AudioSource[] audios = gameObject.GetComponents<AudioSource>();
            AudioSource currentTrack = audios.FirstOrDefault(a => a.clip.name.Equals(nameOSTPlaying));
            StartCoroutine(FadeInTrack(currentTrack));
        }
    }
    public void ChangeVolumes(bool sceneStart) {    //o parâmetro sceneStart é necessário para evitar que uma música comece com volume errado quando uma cena é carregada
        List<AudioSource> allAudios = GetComponents<AudioSource>().ToList<AudioSource>();

        foreach (AudioSource audio in allAudios) {
            if (audio.clip.name.Contains("OST")) {
                currentVolumesOSTs[audio.clip.name] = originalVolumesOSTs[audio.clip.name] * Globals.volumeOST;
                if (!sceneStart)
                    audio.volume = originalVolumesOSTs[audio.clip.name] * Globals.volumeOST;
            }
            else {
                currentVolumesSFXs[audio.clip.name] = originalVolumesSFXs[audio.clip.name] * Globals.volumeSFX;
                audio.volume = originalVolumesSFXs[audio.clip.name] * Globals.volumeSFX;
            }
            if (audio.volume <= 0.015)
                audio.volume = 0;
        }
    }

    public void PlaySceneMusic() {
        if (SceneManager.GetActiveScene().name.ToLower().Contains("menu"))
            PlaySound("OST_menu");
        else if (SceneManager.GetActiveScene().name.ToLower().Contains("_1"))
            PlaySound("OST_fase1");
        else if (SceneManager.GetActiveScene().name.ToLower().Contains("_2"))
            PlaySound("OST_fase2");
        else if (SceneManager.GetActiveScene().name.ToLower().Contains("_3"))
            PlaySound("OST_fase3");
    }
}
