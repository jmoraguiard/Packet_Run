using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownloadProgressManager : MonoBehaviour {

    public Image Progress;

    public float UpdateRatio = .01f;

    public Action OnProgressEnds;

	// Use this for initialization
	void Start () {
        Progress.fillAmount = .0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (LevelManager.Instance.IsRunning()) {
            if (Progress.fillAmount < 1)
            {
                Progress.fillAmount += UpdateRatio * Time.deltaTime; // TODO: Add velocity multiplier
            }
            else
            {
                if (OnProgressEnds != null) OnProgressEnds();
            }
        }
	}
}
