using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {
    [SerializeField] Text timerText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timerText.text = Time.timeSinceLevelLoad.ToString("F1");
	}
}
