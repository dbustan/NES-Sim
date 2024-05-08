using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendMessagesToPatch : MonoBehaviour
{
    public enum Scale
    {
        AllSemi, CMajorBlue, AMinorHarm
    }

    public static SendMessagesToPatch instance;

    private Scale defaultScale = Scale.AllSemi;
    Scale currentScale;

    // Get these values from the text next to the sliders
    [SerializeField] private int defaultOctave1 = 5;
    [SerializeField] private int defaultOctave2 = 5;

    [SerializeField] GameObject octave1TextObj;
    [SerializeField] GameObject octave2TextObj;
    [SerializeField] TMP_Text octave1Text;
    TMP_Text octave2Text;

    void Start()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        currentScale = defaultScale;

        octave1Text = octave1TextObj.GetComponent<TMP_Text>();
        octave2Text = octave2TextObj.GetComponent<TMP_Text>();

        // Initialize
        OSCHandler.Instance.Init();

        // Send initial values for scale and octave
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/scale", (int) defaultScale);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/octave1", defaultOctave1);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/octave2", defaultOctave2);
    }

    public void StartPatch()
    {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/start", 1);
    }

    public void ChangeOctave(int select, int octave)
    {
        string path = select == 1 ? "/unity/octave1" : "/unity/octave2";
        OSCHandler.Instance.SendMessageToClient("pd", path, octave);
    }

    // For UI to use
    public void ChangeOctave1(float octave)
    {
        ChangeOctave(1, (int)octave);
        octave1Text.text = octave.ToString();
    }

    public void ChangeOctave2(float octave)
    {
        ChangeOctave(2, (int)octave);
        octave2Text.text = octave.ToString();
    }

    // This takes an int so it works with the dropdown menu
    public void ChangeScale(int scale)
    {
        // Switch off current spigot
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/scale", (int) currentScale);
        // Switch on new spigot
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/scale", scale);

        currentScale = (Scale)scale;
    }

    // In case you'd like to call with Scale parameter
    public void ChangeScale(Scale scale)
    {
        ChangeScale((int)scale);
    }
}
