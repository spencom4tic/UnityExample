using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OSCBee : MonoBehaviour

{
    // Start is called before the first frame update
    
    public OSC osc;
    public float x1, y1, xDiff, yDiff;
    public int count;
    public bool beeInitialize = false;
    public float noise, noiseX, noiseY  = 0;
  
    public float divisor = 100;
    public float divisorCount = 0;
    public float scaleDivisor = 20;
    public float scaleCount = 0;
    public GameObject beeShape;
    string whichBee;
    public GameObject beeLocation;
    public GameObject[] beeFriends;
    public float[] noiseFriendsX;
    public float[] noiseFriendsY;
    public string oscBee = "/bee";
    public Vector3 scaleChange, scaleDiff, originalScale;
    void Start()
    {   
        noiseFriendsX = new float[15];
        noiseFriendsY = new float[15];
        beeLocation.GetComponent<Renderer>().enabled = false;
        originalScale = beeShape.transform.localScale;
        divisor = 100;
        beeShape.GetComponent<Renderer>().enabled = false;
        count = 0;
        x1=0;
        y1=0;
        osc.SetAddressHandler(oscBee, OnRecieveBee);
        for (int z = 0; z<15; z++)
        {
            beeFriends[z].GetComponent<Renderer>().enabled = false;
            
        }

    }

    void OnRecieveBee(OscMessage message)
    {

        
        whichBee = message.GetString(0);
        if(whichBee != "disperse" && whichBee != "last")
        {
            x1 = message.GetInt(1);
            y1 = message.GetInt(2);
            scaleChange = beeShape.transform.localScale;
            beeShape.transform.localScale = 2*scaleChange;
            scaleChange = scaleChange/scaleDivisor;
        }
 
        noise = (float)message.GetInt(3)/(30f);
        xDiff = (x1-(int)beeShape.transform.position[0])/divisor;
        yDiff = (y1-(int)beeShape.transform.position[1])/divisor;
        scaleCount = 0;
        divisorCount = 0;
        if(!beeInitialize)
        {
            beeInitialize = true;
            beeShape.GetComponent<Renderer>().enabled = true;
        }
        if (whichBee == "friend" && count < 15)
        {
            beeFriends[count].GetComponent<Renderer>().enabled = true;
            float friendX = beeLocation.transform.position.x;
            float friendY = beeLocation.transform.position.y;
            beeFriends[count].transform.position = new Vector3(0, 0, 0); 
            count++;

        }
        else if (whichBee == "disperse")
        {
            StartCoroutine(DeadBee(0.02f));
        }
        else if (whichBee == "last")
        {
            StartCoroutine(LastBee(0.02f));
        }
        

    }


    // Update is called once per frame
    void Update()
    {
        if(whichBee == "first" || whichBee == "friend")
        {
            if(UnityEngine.Time.frameCount%6 == 0)
            {
                noiseX = noiseX + UnityEngine.Random.Range(-1f*noise, noise)/20f;
                noiseY = noiseY + UnityEngine.Random.Range(-1f*noise, noise)/20f;
                if(noiseX > noise)
                {
                    noiseX = noise;
                }
                else if (noiseX < -noise)
                {
                    noiseX = -noise;
                }
                if(noiseY > noise)
                {
                    noiseY = noise;
                }
                else if (noiseY < -noise)
                {
                    noiseY = -noise;
                }
                beeShape.transform.position =  beeLocation.transform.position + new Vector3(noiseX, noiseY, 0);
                for(int a = 0; a<count; a++)
                {
                    noiseFriendsX[a] = noiseFriendsX[a] + UnityEngine.Random.Range(-1f*noise, noise)/10f;
                    noiseFriendsY[a] = noiseFriendsY[a] + UnityEngine.Random.Range(-1f*noise, noise)/10f;
                    if(noiseFriendsX[a] > 17f)
                    {
                        noiseFriendsX[a] = 17f;
                    }
                    else if (noiseFriendsX[a] < -17f)
                    {
                        noiseFriendsX[a] = -17f;
                    }
                    if(noiseY > 6f)
                    {
                        noiseFriendsY[a] = 6f;
                    }
                    else if (noiseFriendsY[a] < -6f)
                    {
                        noiseFriendsY[a] = -6f;
                    }
                    beeFriends[a].transform.position =  new Vector3(noiseFriendsX[a], noiseFriendsY[a],0); 
                }
            }
        
            if(divisorCount < divisor)
            {
                beeLocation.transform.position = beeLocation.transform.position + new Vector3(xDiff, yDiff, 0);
                divisorCount++;
                if(beeShape.transform.position.x > beeLocation.transform.position.x + noise)
                {
                    beeShape.transform.position = new Vector3(beeLocation.transform.position.x+noise, beeShape.transform.position.y, 0);
                }
                else if(beeShape.transform.position.x < beeLocation.transform.position.x - noise)
                {
                    beeShape.transform.position = new Vector3(beeLocation.transform.position.x-noise, beeShape.transform.position.y, 0);
                }
                if(beeShape.transform.position.y > beeLocation.transform.position.y + noise)
                {
                    beeShape.transform.position = new Vector3(beeShape.transform.position.x, beeLocation.transform.position.y+noise, 0);
                }
                else if(beeShape.transform.position.y < beeLocation.transform.position.y - noise)
                {
                    beeShape.transform.position = new Vector3(beeShape.transform.position.x, beeLocation.transform.position.y-noise, 0);
                }
                //Debug.Log(divisor + "" + beeShape.transform.position[0]);

                for (int z = 0; z<5; z++)
                {
                    //beeFriends[z].transform.position = beeShape.transform.position + new Vector3(0,3,3);
                }
            }
        
            
            if(scaleCount < scaleDivisor)
            {
                beeShape.transform.localScale -= scaleChange;
                scaleCount++;
            }
            else
            {
                beeShape.transform.localScale = originalScale;
            }
        }
        
    }

    IEnumerator DeadBee(float time)
    {
        float[] xDisperse = new float[16];
        float[] yDispers = new float[16];
        int[] randDisperse = new int[16];
        for(int a = 0; a < 16; a++)
        {
            randDisperse[a] = UnityEngine.Random.Range(0,7);
            switch(randDisperse[a])
            {
                case 0:
                    (xDisperse[a], yDispers[a]) = (0, 1);
                    break;
                case 1:
                    (xDisperse[a], yDispers[a]) = (0.7f, 0.7f);
                    break;
                case 2:
                    (xDisperse[a], yDispers[a]) = (1, 0);
                    break;
                case 3:
                    (xDisperse[a], yDispers[a]) = (0.7f, -0.7f);
                    break;
                case 4:
                    (xDisperse[a], yDispers[a]) = (0, -1);
                    break;
                case 5:
                    (xDisperse[a], yDispers[a]) = (-0.7f, -0.7f);
                    break;
                case 6:
                    (xDisperse[a], yDispers[a]) = (-1, 0);
                    break;
                case 7:
                    (xDisperse[a], yDispers[a]) = (-0.7f, 0.7f);
                    break;
            }

        }
    
        
        for(int a = 0; a < 5; a++)
        {
            beeLocation.transform.position = beeLocation.transform.position + new Vector3(xDisperse[0]*8, yDispers[0]*4, 0);
            for(int b = 1; b < 16; b++)
            {
                beeFriends[b-1].transform.position = beeFriends[b-1].transform.position + new Vector3(xDisperse[b]*8, yDispers[b]*4, 0);
            }
            yield return new WaitForSeconds(time);  

        }
        //beeShape.GetComponent<Renderer>().enabled = false;
        beeLocation.transform.position = new Vector3(-19f, -8f, 0);
        beeShape.transform.position = new Vector3(-19f, -8f, 0);
        x1 = -19f;
        y1 = -8f;
        for(int z = 0; z < 15; z++)
        {
            beeFriends[z].GetComponent<Renderer>().enabled = false;
        }

        //beeShape.transform.position = beeShape.transform.position +  new Vector3
        
    }
    
    IEnumerator LastBee(float time)
    {
        beeShape.transform.localScale = originalScale;
        beeShape.GetComponent<Renderer>().enabled = true;
        for (float z = -200f; z < 200f; z++)
        {
            x1 = MathF.Cos(z*20f/200f)+z*(20f/200f);
            y1 = -MathF.Cos(z*8f/200f)+z*(8f/200f);
            beeLocation.transform.position = new Vector3(x1, y1, 0);
            yield return new WaitForSeconds(time); 
            Debug.Log( z + " x " + x1 + " y " + y1);

        }
        yield return new WaitForSeconds(time);

        //Print the time of when the function is first called.
        //After we have waited 5 seconds print the time again.
    }

}

