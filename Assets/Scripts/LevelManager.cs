using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CableTypes {Normal=1, Blocked, Gap};  

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance;

	public int NumberOfPlayers;

	public PoolManager NormalCables;
	public PoolManager BlockedCables;
	public PoolManager GapCables;

    public float Velocity = 5f;

	[Range(0, 100)]
	public int normalCablesProbability = 80;
	[Range(0, 100)]
	public int blockedCablesProbability = 10;
	[Range(0, 100)]
	public int gapCablesProbability = 10;

    //Dummy
    public List<Transform> CablePositions;
    public bool StartThingy = false;

    public GameObject[][] ActiveCables;

    private int _numberOfActiveCables;
	private float _cableOffset = 4f;
	private int _sizeOfVisibleCable = 20;

	// Use this for initialization
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    // Use this for initialization
    void Start () {
        _numberOfActiveCables = NumberOfPlayers + Mathf.Max(NumberOfPlayers - 1, 1);

        ActiveCables = new GameObject[_numberOfActiveCables][];

        for (int i = 0; i < _numberOfActiveCables; ++i){
            ActiveCables[i] = new GameObject[_sizeOfVisibleCable];
        }

		PrepareCables ();
	}

    public bool IsRunning() {
        return StartThingy;
    }
	
	// Update is called once per frame
	void Update () {
		//Here we do the endless cable...

        // TODO: if for starting. Must wait until all cables are loaded...
        if (StartThingy){
            for (int i = 0; i < _numberOfActiveCables; ++i){
                for (int j = 0; j < _sizeOfVisibleCable; ++j){
                    Vector3 movement = new Vector3(-Velocity, 0, 0) * Time.deltaTime;
                    ActiveCables[i][j].transform.Translate(movement, Space.World);
                }
            }
        }
	}

	private void PrepareCables() {
		for (int i = 0; i < _numberOfActiveCables; ++i) {
            CreateLine (i, CablePositions[i].position);
		}
	}

	private void CreateLine(int indexOfCable, Vector3 position) {

        GameObject[] line = new GameObject[_sizeOfVisibleCable];

		for (int i = 0; i < _sizeOfVisibleCable; ++i) {
			GameObject cable = GetRandomCable ();
            cable.transform.position = position;
            cable.SetActive(true);

            line[i] = cable;

            position = new Vector3(position.x + _cableOffset, position.y, position.z);
		}

        ActiveCables[indexOfCable] = line;
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
