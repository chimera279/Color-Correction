using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class octaveChange : MonoBehaviour
{

    public float transpose = 0;
    float note = -1, transposeTimer = 16f, modeTimer = 8f;
    [Range (0f,2f)]
    public float minTimer = 0.25f;
    float[] countdowns = { 0.25f,0.5f,1.0f}, notes = { 0, 2, 4, 5, 7, 9, 11, 12 };
    new AudioSource[] audio;

    public enum Mode
    {
        Ionian,
        Dorian,
        Phrygian,
        Lydian,
        Mixolydian,
        Aeolian

    };

    public Mode mode;
    // Start is called before the first frame update
    void Start()
    {

        audio = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < countdowns.Length; i++)
        {
            if (countdowns[i] <= 0f)
            {
                DoChange();
                countdowns[i] = minTimer * Mathf.Pow(2, i);
            }
            countdowns[i] -= Time.unscaledDeltaTime;
        }
        note = -1;
        transposeTimer -= Time.unscaledDeltaTime;
        if (transposeTimer <= 0f)
        {
           transpose = notes[Random.Range(0, 5)];
            transposeTimer = 16f;
        }
        modeTimer -= Time.unscaledDeltaTime;
        if (modeTimer <= 0f)
        {
            SetMode();
            modeTimer = 8f;
        }
    }

    
        void DoChange()
        {
          note = -1;
      /* if (Input.GetKeyDown(KeyCode.A)) note = 0;  // C
         if (Input.GetKeyDown(KeyCode.S)) note = 2;  // D
         if (Input.GetKeyDown(KeyCode.D)) note = 4;  // E
         if (Input.GetKeyDown(KeyCode.F)) note = 5;  // F
         if (Input.GetKeyDown(KeyCode.G)) note = 7;  // G
         if (Input.GetKeyDown(KeyCode.H)) note = 9;  // A
         if (Input.GetKeyDown(KeyCode.J)) note = 11; // B
         if (Input.GetKeyDown(KeyCode.K)) note = 12; // C
         if (Input.GetKeyDown(KeyCode.L)) note = 14; // D    */

        note = notes[Random.Range(0, 8)];

            if (note >= 0)
            {
                AudioSource a = LatestAudioSource();
                if (a)
                {
                a.pitch = Mathf.Pow(2, (note + transpose) / 12.0f);
                a.Play();
                }
            }
        }

    void SetMode()
    {
        notes = new float[] { 0, 2, 4, 5, 7, 9, 11, 12 };
        mode = (Mode)Random.Range(0, 6);
        switch(mode)
        {
            case Mode.Ionian:
                break;
            case Mode.Dorian:
                notes[2] -= 1;
                notes[6] -= 1;
                break;
            case Mode.Phrygian:
                notes[1] -= 1;
                notes[2] -= 1;
                notes[5] -= 1;
                notes[6] -= 1;
                break;
            case Mode.Lydian:
                notes[3] += 1;
                break;
            case Mode.Mixolydian:
                notes[6] -= 1;
                break;
            case Mode.Aeolian:
                notes[2] -= 1;
                notes[5] -= 1;
                notes[6] -= 1;
                break;

        }
    }

    AudioSource LatestAudioSource()
    {
        foreach (AudioSource a in audio)
        {
            if (!a.isPlaying)
            {
                
                return a;
                
            }
        }
        return null;
    }
}
