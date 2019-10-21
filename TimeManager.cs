using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
// Unity package for good looking text
using TMPro;

public class TimeManager : MonoBehaviour {

	public static TimeManager sharedInstance = null;
	private string _urlWithDate = "http://localhost/SetTime/getDateAndTime.php";
	private string _urlTime = "http://localhost/SetTime/getTime.php";

	[Header("Get date and time")]
	[SerializeField] private string _currentDate;
	[SerializeField] private string _currentTime;
	
    [Header("Set date and time")]
	[SerializeField] private TextMeshProUGUI clock;
    [SerializeField] private TextMeshProUGUI secTimer;

    // For showing info if user doesnt have internet connection
	[Header("Connection alert")]
	[SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private GameObject alert;

	private string _currentHoursMinutes;
	private string _timeData;
	private string seconds;
	private string _serverSeconds;
	private string[] hoursMinutesSplit;

	//make sure there is only one instance of this always.
	void Awake() 
	{
		if (sharedInstance == null) 
		{
			sharedInstance = this;
		} 
		else if (sharedInstance != this) 
		{
			Destroy (gameObject);  
		}
		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
        // get date/time on start
		StartCoroutine (GetDateAndTime());
		// get time each second
		StartCoroutine (GetTime());
	}

	public IEnumerator GetDateAndTime()
	{
		UnityWebRequest www = UnityWebRequest.Get(_urlWithDate);
		yield return www.SendWebRequest();

		if (www.isNetworkError)
			{
				Debug.Log("Please, Check your internet connection" + "\n" + "Can't get the date");
			} 
			else 
			{
				_timeData = www.downloadHandler.text;
				string[] words = _timeData.Split('/');

				Debug.Log ("The date is : "+words[0]);
				Debug.Log ("The time is : "+words[1]);

				//setting current time
				_currentDate = words[0];
				_currentTime = words[1];	
		}
	}	

	// getting only time, without date and seconds. I use this for getting server second because my game depends on them.
    // You can just remove this whole method
	public IEnumerator GetTime()
	{
		while (true)
		{
			UnityWebRequest www = UnityWebRequest.Get(_urlTime);
			yield return www.SendWebRequest();

			if (www.isNetworkError)
			{
				Debug.Log("Please, Check your internet connection");
			} 
			else 
			{
				_currentHoursMinutes = www.downloadHandler.text;
				hoursMinutesSplit = _currentHoursMinutes.Split('|');
				_currentHoursMinutes = hoursMinutesSplit[0];
				_serverSeconds = hoursMinutesSplit[1];

				clock.text = _currentHoursMinutes;

				int colorizer = Convert.ToInt32(_serverSeconds);

				if (colorizer == 0)
				{
					clock.text = _currentHoursMinutes;
					secTimer.color = new Color (0.4941177f, 0.4509804f, 0.3490196f, 1f);
				}

				if (colorizer >= 40)
				{
					secTimer.color = new Color (0.9056604f, 0.4458134f, 0.1324315f, 1f);
				}

				if (colorizer >= 50)
				{
					secTimer.color = Color.red;
				}

				secTimer.text =_serverSeconds;
			}		
				yield return new WaitForSecondsRealtime(1f);
		}
	}

	//get the current date
	public string getCurrentDateNow()
	{
		return _currentDate;
	}

	//get the current time
	public string getCurrentTimeNow()
	{
		return _currentTime;
	}
}