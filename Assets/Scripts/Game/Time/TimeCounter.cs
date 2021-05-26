using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Time counter class only when player starts a new or loaded game.
/// </summary>
public class TimeCounter : MonoBehaviour
{
    [SerializeField]TMPro.TextMeshProUGUI textMesh;
    public static int miliseconds,seconds,minutes,hours;
    private static string currentTime;
    /// <summary>
    /// Current game session time.
    /// </summary>
    public static string CurrentTime{get{
        return currentTime=TimeValuesToString();
    }}
    public static string currentTimePause;
    private void OnEnable() {
        GameEvents.timeCounter+=StartCounter;
        GameEvents.pauseTimeCounter+=StartPauseCounter;
    }
    private void OnDisable() {
        GameEvents.timeCounter -= StartCounter;
        GameEvents.pauseTimeCounter -= StartPauseCounter;
    }
    public static void SetTimeAfterLoad(int[] time){
        hours=time[0];
        minutes= time[1];
        seconds= time[2];
        miliseconds= time[3];
    }
    private void StartCounter(bool start){
        StopAllCoroutines();
        if(start){textMesh.gameObject.SetActive(false); StartCoroutine(Counter());}
        else {textMesh.gameObject.SetActive(true);textMesh.text=TimeValuesToString();}
    }
    private void StartPauseCounter(bool start)
    {
        StopAllCoroutines();
        if (start){textMesh.gameObject.SetActive(true); ; StartCoroutine(OnPauseCounter());}
        else{textMesh.gameObject.SetActive(false);StartCoroutine(Counter());}
    }
    IEnumerator Counter(){
        while(true){
            miliseconds += 1;
            if(miliseconds>99){seconds++;miliseconds=0;}
            if(seconds>59){minutes++;seconds=0;}
            if(minutes>59){hours++;minutes=0;}
            yield return new WaitForSeconds(.01f);
        }
    }
    /// <summary>
    /// Coroutine for player pause.
    /// </summary>
    /// <returns></returns>
    IEnumerator OnPauseCounter(){
        textMesh.gameObject.SetActive(true);
        while (true)
        {
            miliseconds += 1;
            if (miliseconds > 99) { seconds++; miliseconds = 0; }
            if (seconds > 59) { minutes++; seconds = 0; }
            if (minutes > 59) { hours++; minutes = 0; }
            textMesh.text=TimeValuesToString();
            yield return new WaitForSecondsRealtime(.01f);
        }
    }
    private static string AddCharacterFront(int value,string newChar){
        if(value<=9)return newChar+value.ToString();
        else return value.ToString();
    }
    private static string TimeValuesToString(){
        return AddCharacterFront(hours,"0") + ":" + AddCharacterFront(minutes, "0") + ":" + AddCharacterFront(seconds, "0") + "." + AddCharacterFront(miliseconds, "0");
    }
    public static string TimeArrayIntToString(int[] time){
        string timeString;
        timeString=time[0].ToString()+":"+time[1].ToString() + ":" +time[2].ToString() + "." +time[3].ToString();
        return timeString;
    }
}
