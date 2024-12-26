using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public OSC osc;
    public string oscHelloWorld = "/test";
    void Start()
    {
        osc.SetAddressHandler(oscHelloWorld, OnRecieveHelloWorld);
        
    
    }

    void OnRecieveHelloWorld(OscMessage message)
    {
        //Console.WriteLine("hi!");
        Debug.Log("recieve ");
        int oscNum = message.GetInt(0);
        Debug.Log("number " + oscNum);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
