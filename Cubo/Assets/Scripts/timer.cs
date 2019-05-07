using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    [SerializeField] private Text timerTxt;
    private bool finished = false;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (finished)
        {
            return;
        }
        float t = Time.time - startTime;
        
        int minutes = ((int) t / 60);
        string hours = ((int) minutes / 60).ToString();
        string seconds = (t % 60).ToString("f0");
        timerTxt.text = hours + ':' + minutes.ToString() + ':' + seconds;
    }

    public void Finish()
    {
        finished = true;
        timerTxt.color = Color.yellow;
        
    }
}
