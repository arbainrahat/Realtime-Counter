using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterTimer : MonoBehaviour
{
    [SerializeField] Text counterTxt;
    [SerializeField] Button claimBtn;
    [SerializeField] int startHour = 5;
    [SerializeField] int startMinutes = 59;
    [SerializeField] float startSeconds = 60f;
                     private bool isTimer = true;
    [SerializeField] InputField hrsField;
    [SerializeField] InputField minsField;
    [SerializeField] InputField secField;

    float sec;
    int min;
    int hr;
    string second;
 
    private void OnEnable()
    {
        if(PlayerPrefs.GetInt("Timer") == 0)
        {
           sec = startSeconds;
           min = startMinutes;
           hr = startHour;
           counterTxt.text = hr.ToString() + ":" + min.ToString() + ":" + sec.ToString();
            isTimer = true;
            claimBtn.interactable = false;
        }
        else
        {
            sec = 0;
            min = 0;
            hr = 0;
            counterTxt.text = hr.ToString() + ":" + min.ToString() + ":" + sec.ToString();
            isTimer = false;
            claimBtn.interactable = true;
        }
        
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("prevDT") != null)
        {
            TimeDiff();
        }

        Input.backButtonLeavesApp = true;
    }

    private void Update()
    {
        if (isTimer)
        {
            sec -= Time.deltaTime;
            second = (sec % 60).ToString("f0");
            counterTxt.text = hr.ToString() + ":" + min.ToString() + ":" + second.ToString();
            if (sec <= 0)
            {
                if(hr != 0 || min != 0)
                {
                    sec = 60f;
                    if(hr > 1 && min != 0)
                    {
                        min--;
                    }
                    if(hr == 0 && min != 0)
                    {
                        min--;
                    }
                   
                    if (min == 0)
                    {
                        if (hr != 0)
                        {
                            min = 59;
                            hr--;
                        }
                    }
                }
               
            }
        }

        if (isTimer)
        {
            if (sec <= 0 && min <= 0 && hr <= 0)
            {
                isTimer = false;
                PlayerPrefs.SetInt("Timer", 1);
                claimBtn.interactable = true;
                print("Timer Stop");
            }
        }
             
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!Application.isEditor)
        {
            if (!focus)
            {
                DateTime quitTime = DateTime.Now;
                PlayerPrefs.SetString("prevDT", quitTime.ToString());
                PlayerPrefs.SetInt("H", hr);
                PlayerPrefs.SetInt("M", min);
                PlayerPrefs.SetFloat("S", sec);
            }
        }     
    }
    
    private void OnApplicationQuit()
    {
        if (Application.isEditor)
        {
            DateTime quitTime = DateTime.Now;
            PlayerPrefs.SetString("prevDT", quitTime.ToString());
            PlayerPrefs.SetInt("H", hr);
            PlayerPrefs.SetInt("M", min);
            PlayerPrefs.SetFloat("S", sec);
        }      
    }



    void TimeDiff()
    {
        try
        {
            DateTime date1 = DateTime.Now;
            String dateQuit = PlayerPrefs.GetString("prevDT");
            DateTime date2 = DateTime.Parse(dateQuit);
            TimeSpan dif = date1.Subtract(date2);

            hr = PlayerPrefs.GetInt("H");
            min = PlayerPrefs.GetInt("M");
            sec = PlayerPrefs.GetFloat("S");

            //print("Curnt Date = " + date1);
            //print("Previous Date = " + date2);

            //print("Dif = " + dif);
            //print("Hrs = " + dif.Hours);
            //print("Min = " + dif.Minutes);
            //print("Sec = " + dif.Seconds);

            if (hr >= dif.Hours)
            {
                hr -= dif.Hours;
            }
            else
            {
                hr = 0;
            }

            if (min >= dif.Minutes)
            {
                min -= dif.Minutes;
            }
            else
            {
                min = 0;
            }

            if ((int)sec >= dif.Seconds && min != 0 && hr != 0)
            {
                sec -= dif.Seconds;
            }
            else
            {
                sec = 0f;
            }
        }
        catch (Exception)
        {

           
        }
        

        counterTxt.text = hr.ToString() + ":" + min.ToString() + ":" + sec.ToString();

        if (sec <= 0 && min <= 0 && hr <= 0)
        {
            isTimer = false;
            PlayerPrefs.SetInt("Timer", 1);
            claimBtn.interactable = true;
            print("Timer Stop");
        }
    }

    public void ResetTimer()
    {
        try
        {
            if (!string.IsNullOrEmpty(secField.text))
            {
                sec = float.Parse(secField.text);
            }
            else
            {
                sec = startSeconds;
            }

            if (!string.IsNullOrEmpty(minsField.text))
            {
                min = int.Parse(minsField.text);
            }
            else
            {
                min = startMinutes;
            }

            if (!string.IsNullOrEmpty(hrsField.text))
            {
                hr = int.Parse(hrsField.text);
            }
            else
            {
                hr = startHour;
            }
        }
        catch (Exception)
        {    
        }
        
        counterTxt.text = hr.ToString() + ":" + min.ToString() + ":" + sec.ToString();
        PlayerPrefs.SetInt("Timer", 0);
        isTimer = true;
        claimBtn.interactable = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
