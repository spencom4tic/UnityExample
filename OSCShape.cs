using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class OSCShape : MonoBehaviour
{
    // Start is called before the first frame update
    private static System.Random rand = new System.Random();
    public OSC osc;
    public int count = 0;
    public int countDown = 30;
    public int x = 0;

    public int y = 0;
    public string oscCreateShape = "/create";
    public Material[] hexInt;
    public Material[] hexExt;
    public bool[] hexActive = new bool[31];
    public int[,] hexToArray = new int[11, 3];
    public List<int> listOfHexesLeft, destroyList = new List<int>();
    public float countDissolve = 0.01f;
    string msg;
    public bool failsafe = false;
    void Start()
    {   
        count=0;
        Debug.Log("THE START OF TEH CODE");
        for (int z = 0; z<31; z++)
        {
            listOfHexesLeft.Add(z);
            destroyList.Add(z);
            Debug.Log("hexes z " + listOfHexesLeft[z]);
            //Debug.Log(z);
            if(z <= 9)
            {
                //Debug.Log("x:" + z + "y: " + 0);
                hexToArray[z, 0] = z;
                
            }
            else if(10 <= z && z <= 20)
            {
                //Debug.Log("x:" + (z-10) + "y: " + 1);
                hexToArray[z-10, 1] = z;
                
            }
            else
            {
                //Debug.Log("x:" + (z-21) + "y: " + 2);
                hexToArray[z-21, 2] = z;
            }

            hexInt[z].SetFloat("_DissolveInt", 1);
            hexExt[z].SetFloat("_DissolveExt", 1);
        }
        Debug.Log("start hi!");
        osc.SetAddressHandler(oscCreateShape, OnRecieveCreateShape);
        listOfHexesLeft = listOfHexesLeft.OrderBy(c => rand.Next()).ToList();

        destroyList = destroyList.OrderBy(c => rand.Next()).ToList();
    
    }

    void OnRecieveCreateShape(OscMessage message)
    {
        countDissolve = 0;
        msg = message.GetString(0);
        x = message.GetInt(1);
        y = message.GetInt(2);
        if(msg == "hex")
        {

            //int num = hexToArray[x, y];
            //Debug.Log("x" + x + "y" + y + " element" + num);

            //if (hexActive[num] == false)
            //{
            //    hex[num].SetActive(true);
            //    hexActive[num] = true;
            //}
            //if(listOfHexesLeft.Contains(num))
            //{
            //Debug.Log(count + " " + listOfHexesLeft[count]);
            if(count < 31)
            {
                StartCoroutine(CreateHex(0.01f));

                //hexActive[listOfHexesLeft[count]] = true;
                count++;
            }
 
  
 
        }
        else if(msg == "failsafe")
        {
            failsafe = true;
            Debug.Log("new list!");
            StartCoroutine(CreateAndWait(1f));
        } 
        else if(msg == "destroyFailsafe")
        {
            StartCoroutine(DestroyAndWait(1f));
        }
        else if(msg == "destroy")
        {
            /*int num = hexToArray[x, y];
            Debug.Log("x" + x + "y" + y + " element" + num);

            if (hexActive[num] == true)
            {
                hex[num].SetActive(false);
                hexActive[num] = false;
            }
            if(destroyList.Contains(num))
            {
                destroyList.Remove(destroyList.Single( s => s == num ) );
            }*/
            //hexInt[destroyList[countDown]].SetActive(false);
            //hexExt[destroyList[countDown]].SetActive(false);
            StartCoroutine(DestroyHex(0.01f));
        }
    }
        
        


    // Update is called once per frame
    void Update()
    {
        /*if(countDissolve < 1)
        {
            Material hexMat = hex[hexToArray[x,y]].GetComponent<Renderer>().material;
            hexMat.SetFloat("_DissolveInt", countDissolve);
            countDissolve = countDissolve + 0.02f;
            //Debug.Log(countDissolve + "" + hex[hexToArray[x,y]].GetComponent<Renderer>().material);
        }*/
        
    }
    IEnumerator CreateAndWait(float time)
    {
         
        for(int z = 0; z < listOfHexesLeft.Count; z++)
        {
            //hexInt[listOfHexesLeft[z]].SetActive(true);
            //hexExt[listOfHexesLeft[z]].SetActive(true);
            for(float a = 1; a > 0; a = a - 0.05f)
            {
                hexInt[listOfHexesLeft[z]].SetFloat("_DissolveInt", a);
                hexExt[listOfHexesLeft[z]].SetFloat("_DissolveExt", a);
                yield return new WaitForSeconds(time);
            }

        }
        //Print the time of when the function is first called.
        //After we have waited 5 seconds print the time again.
    }
    IEnumerator DestroyAndWait(float time)
    {
         
        for(int z = 0; z < destroyList.Count; z++)
        {
            for(float a = 0; a < 1; a = a + 0.05f)
            {
                hexInt[destroyList[z]].SetFloat("_DissolveInt", a);
                hexExt[destroyList[z]].SetFloat("_DissolveExt", a);
                yield return new WaitForSeconds(time);
            }
        }
        //Print the time of when the function is first called.
        //After we have waited 5 seconds print the time again.
    }

    IEnumerator CreateHex(float time)
    {
        for(float a = 1; a > 0; a = a - 0.05f)
        {
            //ObjectName.SetFloat("_FloatName", a)
            yield return new WaitForSeconds(time);
        }
    }

    IEnumerator DestroyHex(float time)
    {
        for(float a = 0; a < 1; a = a + 0.05f)
        {
            hexInt[destroyList[countDown]].SetFloat("_DissolveInt", a);
            hexExt[destroyList[countDown]].SetFloat("_DissolveExt", a);
            yield return new WaitForSeconds(time);
        }
        destroyList.RemoveAt(countDown);
        countDown--;
        //Print the time of when the function is first called.
        //After we have waited 5 seconds print the time again.
    }

    void OnClose()
    {
        for(int z =0; z< 31;z++)
        {
            hexInt[z].SetFloat("_DissolveInt", 0);
            hexExt[z].SetFloat("_DissolveExt", 0);
        }
    }

}
