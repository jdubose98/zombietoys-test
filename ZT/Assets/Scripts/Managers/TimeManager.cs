using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {
    [SerializeField] Text timerText;
    [SerializeField] PlayerHealth playerHP;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (playerHP.currentHealth > 0)
        timerText.text = Time.timeSinceLevelLoad.ToString("F1");
	}
}
