using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CableTypes {Normal=1, Blocked, Gap};  

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance;

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

    public int normalCablesAfterObstacle = 3;

    //Dummy
    public List<Transform> CablePositions;
    public bool StartThingy = false;

    private int _numberOfActiveCables;
	private float _cableOffset = 3f;
	private int _sizeOfVisibleCable = 20;

    private GameObject[] _lastObjects;

    private int[] _cableTypesCounter;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    // Commented for PlayMaker
    //private void Start()
    //{

    //    PrepareGame();
    //}

    public void PrepareGame(int numberOfPlayer){
        _numberOfActiveCables = numberOfPlayer + Mathf.Max(numberOfPlayer - 1, 1);

        _lastObjects = new GameObject[_numberOfActiveCables];
        _cableTypesCounter = new int[_numberOfActiveCables];

        PrepareCables();
    }

    public bool IsRunning() {
        return StartThingy;
    }

    public void ToggleRunning(bool value){
        StartThingy = value;
    }

    public void AddCableToLine(int lineIndex) {
        GameObject cable = GetRandomCable(lineIndex);
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
        Vector3 aux = new Vector3(position.x, position.y, position.z);
		for (int i = 0; i < _sizeOfVisibleCable; ++i) {
            GameObject cable = GetRandomCable (indexOfCable);
            MovementComponent movementComponent = cable.GetComponent<MovementComponent>();
            movementComponent.Init(Velocity, new Vector3(aux.x + _cableOffset, aux.y, aux.z), -_cableOffset, indexOfCable);
            movementComponent.OnDisappear += AddCableToLine;
            _lastObjects[indexOfCable] = cable;
            aux = _lastObjects[indexOfCable].transform.position;
        }
	}

    private GameObject GetRandomCable(int lineIndex) {
        GameObject randomCable = null;
        CableTypes cableType = CableTypes.Normal;
        if (_cableTypesCounter[lineIndex] <= 0) {
            cableType = this.getRandomCableType();
        }
		if (cableType == CableTypes.Normal) {
            randomCable = NormalCables.GetPooledObject();
            _cableTypesCounter[lineIndex]--;
        }
		else if (cableType == CableTypes.Blocked)
        {
            randomCable = BlockedCables.GetPooledObject();
            _cableTypesCounter[lineIndex] = normalCablesAfterObstacle;
        }
		else if (cableType == CableTypes.Gap)
        {
            randomCable = GapCables.GetPooledObject();
            _cableTypesCounter[lineIndex] = normalCablesAfterObstacle;
        }
        return randomCable;
	}

    private GameObject GetNormalCable() {
        return NormalCables.GetPooledObject();
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
