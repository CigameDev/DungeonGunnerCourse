using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
public class Test : MonoBehaviour
{
    Transform myTransform;
    private void Awake()
    {
        myTransform = transform;
    }
    private void Start()
    {
        Stopwatch watch = new Stopwatch();
        int times = 1000000; // thực hiện 1000000 lần
        watch.Start();
        int x = 0;
        for (int i = 0; i < times; i++)
        {
            //x++;
            myTransform.position = Vector3.zero;
            transform.position = Vector3.zero;
        }
        UnityEngine.Debug.Log(x);
        watch.Stop();
        UnityEngine.Debug.Log("Non-cache costs : " + watch.ElapsedMilliseconds + "(ms)");
        watch.Reset();


    }
}
