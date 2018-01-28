using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CableTypes {Normal=1, Blocked, Gap};

public class LevelManager : MonoBehaviour
{
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

    private int _numberOfActivePlayers;
    private float _cableOffset = 6.15f;
    private int _sizeOfVisibleCable = 23;

    private GameObject[] _lastObjects;

    private int[] _cableTypesCounter;
    public float _speedMultiplier = 1.0f;

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
    //    PrepareGame(4);
    //}

    public void PrepareGame(int numberOfPlayer)
    {
        _numberOfActivePlayers = numberOfPlayer;

        _lastObjects = new GameObject[GetNumberOfActiveCables()];
        _cableTypesCounter = new int[GetNumberOfActiveCables()];

        PrepareCables();
        SoundManager.Instance.PlayRepeatedMusic("Mecha_Action");
    }

    public bool IsRunning()
    {
        return StartThingy;
    }

    public void ToggleRunning(bool value)
    {
        StartThingy = value;
    }
    public float getSpeedMultiplier()
    {
        return _speedMultiplier;
    }

    #region Cables methods

    public int GetNumberOfActiveCables()
    {
        return _numberOfActivePlayers + Mathf.Max(_numberOfActivePlayers - 1, 1);
    }

    public void AddCableToLine(int lineIndex)
    {
        if (lineIndex >= GetNumberOfActiveCables()) {
            return;
        }
        GameObject cable = GetRandomCable(lineIndex);
        Vector3 position = _lastObjects[lineIndex].transform.position;
        MovementComponent movementComponent = cable.GetComponent<MovementComponent>();
        movementComponent.Init(Velocity, new Vector3(position.x + _cableOffset, position.y, position.z), -_cableOffset, lineIndex);
        movementComponent.OnDisappear += AddCableToLine;
        _lastObjects[lineIndex] = cable;
    }

    public float GetMaxCableLength(){
        return _cableOffset * (_sizeOfVisibleCable - 2);
    }

    private void PrepareCables()
    {
        for (int i = 0; i < GetNumberOfActiveCables(); ++i)
        {
            CreateLine(i, CablePositions[i].position);
        }
    }

    private float LineXLimit()
    {
        return _cableOffset * GetNumberOfActiveCables();
    }

    private void CreateLine(int indexOfCable, Vector3 position)
    {
        Vector3 aux = new Vector3(position.x, position.y, position.z);
        for (int i = 0; i < _sizeOfVisibleCable; ++i)
        {
            GameObject cable = GetRandomCable(indexOfCable);
            MovementComponent movementComponent = cable.GetComponent<MovementComponent>();
            movementComponent.Init(Velocity, new Vector3(aux.x + _cableOffset, aux.y, aux.z), -_cableOffset, indexOfCable);
            movementComponent.OnDisappear += AddCableToLine;
            _lastObjects[indexOfCable] = cable;
            aux = _lastObjects[indexOfCable].transform.position;
        }
    }

    private GameObject GetRandomCable(int lineIndex)
    {
        GameObject randomCable = null;
        CableTypes cableType = CableTypes.Normal;
        if (_cableTypesCounter[lineIndex] <= 0)
        {
            cableType = this.getRandomCableType();
        }
        if (cableType == CableTypes.Normal)
        {
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

    private GameObject GetNormalCable()
    {
        return NormalCables.GetPooledObject();
    }

    private CableTypes getRandomCableType()
    {
        int rand = Random.Range(0, 100);
        if (rand <= normalCablesProbability)
        {
            return CableTypes.Normal;
        }
        else if (rand <= normalCablesProbability + blockedCablesProbability)
        {
            return CableTypes.Blocked;
        }
        else if (rand <= normalCablesProbability + blockedCablesProbability + gapCablesProbability)
        {
            return CableTypes.Gap;
        }
        else
        {
            //TODO: exception
            return 0;
        }
    }

    public void setCablesProbabilities(int[] probabilities)
    {
        normalCablesProbability = probabilities[0];
        blockedCablesProbability = probabilities[1];
        gapCablesProbability = probabilities[2];
    }
    #endregion

    #region Player methods

    public void setPlayersXPositions(float[] xPositions)
    {
        bool shouldGoFast = true;
        for (int i = 0; i < xPositions.Length; ++i)
        {
            shouldGoFast = shouldGoFast && (xPositions[i] > _sizeOfVisibleCable * _cableOffset / 2);
        }
        bool shouldGoSlow = true;
        for (int i = 0; i < xPositions.Length; ++i)
        {
            shouldGoSlow = shouldGoSlow && (xPositions[i] < _sizeOfVisibleCable * _cableOffset / 2);
        }
        if (shouldGoFast)
        {
            _speedMultiplier = 1.25f;
        }
        else if (shouldGoSlow)
        {
            _speedMultiplier = 0.75f;
        }
        else
        {
            _speedMultiplier = 1.0f;
        }
    }

    public void SetNumberOfActivePlayers(int numberOfActivePlayers)
    {
        _numberOfActivePlayers = numberOfActivePlayers;
    }

    #endregion
}
