using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessagesToPatch : MonoBehaviour
{
    enum Scale
    {
        AllSemi, CMajorBlue, AMinorHarm
    }

    public static SendMessagesToPatch instance;
    // Start is called before the first frame update

    private Scale defaultScale = Scale.AllSemi;
    Scale currentScale;
    private int defaultOctave1 = 4;
    private int defaultOctave2 = 4;

    void Start()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        currentScale = defaultScale;

        // Initialize
        OSCHandler.Instance.Init();

        // Send initial values for scale and octave and start patch
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/scale", (int) defaultScale);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/octave1", defaultOctave1);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/octave2", defaultOctave2);

        OSCHandler.Instance.SendMessageToClient("pd", "/unity/start", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeOctave(int select, int octave)
    {
        string path = select == 1 ? "/unity/octave1" : "/unity/octave2";
        OSCHandler.Instance.SendMessageToClient("pd", path, octave);
    }

    void ChangeScale(Scale scale)
    {
        // Switch off current spigot
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/scale", (int) currentScale);
        // Switch on new spigot
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/scale", (int) scale);
    }
}
