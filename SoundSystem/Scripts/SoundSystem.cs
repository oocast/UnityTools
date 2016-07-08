using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class SoundSystem : MonoBehaviour
{
    [System.Serializable]
    public class ClipEntry
    {
        // TODO: specify related clip names in external object XMLs
        public string name;
        public AudioClip clip;
        public string path;
        // TODO: load/unload clip file in specific times
        public bool loaded;
    }

    public ClipEntry[] clips;
    Dictionary<string, int> lookupTable;
    string currentBGM;

    // Use this for initialization
    void Start()
    {
        lookupTable = new Dictionary<string, int>();
        for (int i = 0; i < clips.Length; i++)
        {
            if (!lookupTable.ContainsKey(clips[i].name))
            {
                lookupTable.Add(clips[i].name, i);
            }
        }
        currentBGM = "";
    }

    /// <summary>
    /// Load audio clip to memory
    /// </summary>
    /// <param name="clipName">Name of audio clip</param>
    public void LoadSound(string clipName)
    {
        if (lookupTable.ContainsKey(clipName))
        {
            ClipEntry entry = clips[lookupTable[clipName]];
            if (!entry.loaded)
            {
                entry.clip = Resources.Load<AudioClip>(entry.path);
                if (entry.clip != null)
                {
                    entry.loaded = true;
                }
            }
        }
    }

    /// <summary>
    /// Load audio clip to memory
    /// </summary>
    /// <param name="clipEntry">ClipEntry object containing audio clip</param>
    public void LoadSound(ClipEntry clipEntry)
    {
        if (!clipEntry.loaded)
        {
            clipEntry.loaded = true;
            clipEntry.clip = Resources.Load<AudioClip>(clipEntry.path);
        }
    }

    /// <summary>
    /// Unload audio clip from memory
    /// </summary>
    /// <param name="clipEntry">ClipEntry object containing audio clip</param>
    public void UnloadSound(ClipEntry clipEntry)
    {
        if (clipEntry.loaded)
        {
            clipEntry.loaded = false;
            clipEntry.clip = null;
        }
    }

    /// <summary>
    /// Unload audio clip from memory
    /// </summary>
    /// <param name="clipName">Name of audio clip</param>
    public void UnloadSound(string clipName)
    {
        if (lookupTable.ContainsKey(clipName))
        {
            ClipEntry entry = clips[lookupTable[clipName]];
            if (entry.loaded)
            {
                entry.loaded = false;
                entry.clip = null;
            }
        }
    }

    /// <summary>
    /// Play audio clip once on certain position
    /// </summary>
    /// <param name="position">Position in world coordinates to play the audio clip</param>
    /// <param name="clipName">Name of audio clip</param>
    /// <param name="volume">Volume to play the audio</param>
    public void PlaySound(Vector3 position, string clipName, float volume = 1f)
    {
        if (lookupTable.ContainsKey(clipName))
        {
            GameObject soundSource = new GameObject("Sound Source", typeof(AudioSource));
            soundSource.transform.position = position;
            AudioSource source = soundSource.GetComponent<AudioSource>();
            ClipEntry entry = clips[lookupTable[clipName]];
            LoadSound(entry);
            source.clip = entry.clip;
            source.volume = volume;
            source.Play();
            Destroy(soundSource, source.clip.length + 1f);
        }
    }

    /// <summary>
    /// Play audio clip repeatedly on certain position, until explicitly delete the audio source object
    /// </summary>
    /// <param name="position">Position in world coordinates to play the audio clip</param>
    /// <param name="clipName">Name of audio clip</param>
    /// <param name="volume">Volume to play the audio</param>
    /// <returns>Audio source object</returns>
    public GameObject PlayLoopingSound(Vector3 position, string clipName, float volume = 1f)
    {
        if (lookupTable.ContainsKey(clipName))
        {
            GameObject soundSource = new GameObject("Sound Source", typeof(AudioSource));
            soundSource.transform.position = position;
            AudioSource source = soundSource.GetComponent<AudioSource>();
            source.clip = clips[lookupTable[clipName]].clip;
            source.volume = volume;
            source.loop = true;
            source.Play();
            return soundSource;
        }
        else
        {
            return null;
        }
    }

    public AudioClip GetSoundClip(string clipName)
    {
        if (lookupTable.ContainsKey(clipName))
        {
            return clips[lookupTable[clipName]].clip;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Change BGM to clipName
    /// </summary>
    /// <param name="clipName">Name of new BGM clip</param>
    public void ChangeBGM(string clipName)
    {
        if (lookupTable.ContainsKey(clipName) && !currentBGM.Equals(clipName))
        {
            StartCoroutine(ChangeBGMCoroutine(clipName));
            currentBGM = clipName;
        }
    }

    /// <summary>
    /// Coroutine used to change BGM
    /// </summary>
    /// <param name="clipName">Name of new BGM clip</param>
    /// <returns></returns>
    IEnumerator ChangeBGMCoroutine(string clipName)
    {
        GameObject audioObject = GameObject.Find("Main Camera/BGM");
        if (audioObject != null)
        {
            AudioSource source = GameObject.Find("Main Camera/BGM").GetComponent<AudioSource>();
            source.DOFade(0f, 1f);
            yield return new WaitForSeconds(1f);
            source.Stop();
            source.clip = clips[lookupTable[clipName]].clip;
            source.Play();
            source.DOFade(1f, 1f);
        }
        else
        {
            yield return null;
        }
    }
}
