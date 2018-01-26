using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public int NumberOfPlayers;

	public PoolManager NormalCables;
	public PoolManager BlockedCables;
	public PoolManager GapCables;

    //Dummy
    public List<Transform> CablePositions;

	private int _activeCables;
	private float _cableOffset = 4f;
	private int _sizeOfVisibleCable = 20;

	// Use this for initialization
	void Start () {
        _activeCables = NumberOfPlayers + (NumberOfPlayers - 1);

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

        // TODO: make probabilities to get wires... like 70% normal 15% gaps and 15% blocked

        //Get just normal random for now (MU CUTRE)

        int rand = Random.Range(0, 3);
        if (rand == 0) {
            randomCable = NormalCables.GetPooledObject();
        }
        else if (rand == 1)
        {
            randomCable = BlockedCables.GetPooledObject();
        }
        else if (rand == 2)
        {
            randomCable = GapCables.GetPooledObject();
        }

        return randomCable;
	}
}
