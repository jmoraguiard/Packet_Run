using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour {

    public Action<int> OnDisappear;
    private float _velocity = 5f;
    private float _limit;
    private int _lineIndex;

    public void Init(float velocity, Vector3 startingPosition, float limit, int lineIndex) {
        this._velocity = velocity;
        transform.position = startingPosition;
        _limit = limit;
        _lineIndex = lineIndex;
        gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        if(LevelManager.Instance.IsRunning()) {
            float multiplier = LevelManager.Instance.getSpeedMultiplier();
            Vector3 movement = new Vector3(-_velocity * multiplier, 0, 0) * Time.deltaTime;
            transform.Translate(movement, Space.World);
            if (transform.position.x <= _limit) {
                OnDisappear.Invoke(_lineIndex);
                OnDisappear = null;
                gameObject.SetActive(false);
            }
        }
	}
}
