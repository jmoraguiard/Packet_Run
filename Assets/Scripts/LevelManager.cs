using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CableTypes {Normal=1, Blocked, Gap};  

public class LevelManager : MonoBehaviour {

	public int NumberOfPlayers;

	public PoolManager NormalCables;
	public PoolManager BlockedCables;
	public PoolManager GapCables;

	[Range(0, 100)]
	public int normalCablesProbability;
	[Range(0, 100)]
	public int blockedCablesProbability;
	[Range(0, 100)]
	public int gapCablesProbability;

    //Dummy
    public List<Transform> CablePositions;

	private int _activeCables;
	private float _cableOffset = 4f;
	private int _sizeOfVisibleCable = 20;

	// Use this for initialization
	void Start () {
        _activeCables = NumberOfPlayers + Mathf.Max(NumberOfPlayers - 1, 1);

		PrepareCables ();
	}
	
	// Update is called once per frame
	void Update () {
		//Here we do the endless cable...
	}

	private void PrepareCables() {
		for (int i = 0; i < _activeCables; ++i) {
            CreateLine (CablePositions[i].position);
		}
	}

	// TODO: give the transform to this.
	private void CreateLine(Vector3 position) {
		for (int i = 0; i < _sizeOfVisibleCable; ++i) {
			GameObject cable = GetRandomCable ();
            cable.transform.position = position;
            cable.SetActive(true);

            position = new Vector3(position.x, position.y + _cableOffset, position.z);
		}
	}

    private GameObject GetRandomCable() {
        GameObject randomCable = null;
		CableTypes cableType = this.getRandomCableType ();
		Debug.Log ("Cable type: " + cableType);
		if (cableType == CableTypes.Normal) {
            randomCable = NormalCables.GetPooledObject();
        }
		else if (cableType == CableTypes.Blocked)
        {
            randomCable = BlockedCables.GetPooledObject();
        }
		else if (cableType == CableTypes.Gap)
        {
            randomCable = GapCables.GetPooledObject();
        }
        return randomCable;
	}

	private CableTypes getRandomCableType(){
		int rand = Random.Range(0, 100);
		if (rand <= normalCablesProbability) {
			return CableTypes.Normal;
		} else if (rand <= normalCablesProbability + blockedCablesProbability) {
			return CableTypes.Blocked;
		} else if (rand <= normalCablesProbability + blockedCablesProbability + gapCablesProbability) {
			return CableTypes.Gap;
		} else {
			//TODO: exception
			return 0;
		}
	}
}
