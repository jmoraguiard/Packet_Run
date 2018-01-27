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

    public List<Queue<GameObject>> ActiveCables;

    private int _numberOfActiveCables;
	private float _cableOffset = 3f;
	private int _sizeOfVisibleCable = 20;

    private GameObject[] _lastObjects;


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

        _lastObjects = new GameObject[_numberOfActiveCables];

        ActiveCables = new List<Queue<GameObject>>();//new GameObject[_numberOfActiveCables][];

        for (int i = 0; i < _numberOfActiveCables; ++i){
            ActiveCables.Add(new Queue<GameObject>()); //new GameObject[_sizeOfVisibleCable];
        }

		PrepareCables ();
	}

    public bool IsRunning() {
        return StartThingy;
    }

    public void AddCableToLine(int lineIndex) {
        GameObject cable = GetRandomCable();
        Vector3 position = _lastObjects[lineIndex].transform.position;
        MovementComponent movementComponent = cable.GetComponent<MovementComponent>();
        movementComponent.Init(Velocity, new Vector3(position.x + _cableOffset, position.y, position.z), -_cableOffset, lineIndex);
        movementComponent.OnDisappear += AddCableToLine;
        _lastObjects[lineIndex] = cable;
    }

	private void PrepareCables() {
		for (int i = 0; i < _numberOfActiveCables; ++i) {
            CreateLine (i, CablePositions[i].position);
		}
	}

    private float LineXLimit() {
        return _cableOffset * _numberOfActiveCables;
    }

	private void CreateLine(int indexOfCable, Vector3 position) {
        float initialX = position.x;
        Vector3 aux = new Vector3(position.x, position.y, position.z);
		for (int i = 0; i < _sizeOfVisibleCable; ++i) {
			GameObject cable = GetRandomCable ();
            MovementComponent movementComponent = cable.GetComponent<MovementComponent>();
            movementComponent.Init(Velocity, new Vector3(aux.x + _cableOffset, aux.y, aux.z), -_cableOffset, indexOfCable);
            movementComponent.OnDisappear += AddCableToLine;
            _lastObjects[indexOfCable] = cable;
            aux = _lastObjects[indexOfCable].transform.position;
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
