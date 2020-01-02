using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float timer = 3600;
    bool timeIsUp = false;


    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.gameIsOn)
        {
            if (timer > 0 && !timeIsUp)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0;
                TimeIsUp();
            }


            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = (timer % 60).ToString("00");

            UIManager.Instance.time.text = string.Format("Time Left: {0}min: {1}s", minutes, seconds);
        }
       
        //print(string.Format("{0}:{1}", minutes, seconds));
    }

    public void TimeIsUp()
    {
        if(!timeIsUp)
        {
            GameManager.Instance.TimeIsUp();
            timeIsUp = true;
        }
        
    }
}
