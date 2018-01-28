using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownloadProgressManager : MonoBehaviour {

    public Image Progress;

    public Text TimeCode;

    public float UpdateRatio = .01f;

    public Action<float> OnProgressEnds;

    public float CurrentPercent {
        get {
            return Progress.fillAmount * 100;
        }
    }

    private float timer = 0;

	// Use this for initialization
	void Start () {
        Progress.fillAmount = .0f;
	}

	// Update is called once per frame
	void Update () {
        if (LevelManager.Instance.IsRunning()) {
            if (Progress.fillAmount < 1)
            {
                timer += Time.deltaTime;
                Progress.fillAmount += UpdateRatio * Time.deltaTime * LevelManager.Instance.getSpeedMultiplier();

                int minutes = Mathf.FloorToInt(timer / 60F);
                int seconds = Mathf.FloorToInt(timer - minutes * 60);

                TimeCode.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                if (OnProgressEnds != null) OnProgressEnds(timer);
            }
        }
	}
}
