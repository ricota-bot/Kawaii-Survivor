using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(WaveManagerUI))]
public class WaveManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Player _player;
    private WaveManagerUI _ui;

    [Header("Settings")]
    [SerializeField] private float _waveDuration;
    private float _timer;
    private bool _isTimerOn;
    private int _currentWaveIndex;

    [Header("Waves")]
    [SerializeField] private Wave[] _waves;

    private List<float> _localCounters = new List<float>();

    private void Awake()
    {
        _ui = GetComponent<WaveManagerUI>();
    }

    private void Update()
    {
        if (!_isTimerOn)
            return;

        if (_timer < _waveDuration)
        {
            string timerString = ((int)(_waveDuration - _timer)).ToString();
            _ui.UpdateWaveTimer(timerString);
            ManageCurrentWave();
        }
        else
            StartWaveTransition();
    }

    private void StartWave(int waveIndex)
    {
        Debug.Log("Start Wave " + waveIndex);

        _ui.UpdateWaveText("Wave " + (_currentWaveIndex + 1) + " / " + _waves.Length);
        _localCounters.Clear();
        foreach (var segment in _waves[waveIndex].segments)
        {
            _localCounters.Add(1);

        }
        _timer = 0;
        _isTimerOn = true;
    }

    private void ManageCurrentWave()
    {
        Wave currentWave = _waves[_currentWaveIndex];

        for (int i = 0; i < currentWave.segments.Count; i++) // Loop pro todos os segmentos dessa Wave
        {
            // Pegando o segmento interação por interação
            WaveSegments segment = currentWave.segments[i];
            // Calcular end time and start time
            float tStart = segment.timeStartToEnd.x / 100 * _waveDuration; // Example "0,00"  0/100 * 30 = 0s
            float tEnd = segment.timeStartToEnd.y / 100 * _waveDuration;   // Example "0,30"  30/100 * 30 = 9s

            // Checando se _timer esta dentro do timeStartEnd, caso esteja, vamos pegar a frequencia e spawnar os prefabs
            if (_timer < tStart || _timer > tEnd) // estamos fora do timeStart and timeEnd
                continue;

            float timeSinceSegmentStart = _timer - tStart;

            float spawnDelay = 1f / segment.spawnFrequency;

            if (timeSinceSegmentStart / spawnDelay > _localCounters[i])
            {
                Instantiate(segment.prefab, GetSpawnPosition(), Quaternion.identity, transform);
                _localCounters[i]++;
            }

        }
        _timer += Time.deltaTime;
    }

    private void StartWaveTransition()
    {
        _isTimerOn = false;
        DefeatAllEnemies();
        _currentWaveIndex++;

        if (_currentWaveIndex >= _waves.Length)
        {
            _ui.UpdateWaveTimer("");
            _ui.UpdateWaveText("- Stage Completed -");
            GameManager.instance.SetGameState(GameState.STAGECOMPLETE);
        }
        else
            GameManager.instance.WaveCompleteCallBack();
    }

    private void StartNextWave()
    {
        StartWave(_currentWaveIndex);
    }

    private void DefeatAllEnemies()
    {
        while (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 direction = Random.onUnitSphere;
        Vector2 offSet = direction.normalized * Random.Range(6, 10);
        Vector2 targetPosition = (Vector2)_player.transform.position + offSet;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -18, 18);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -8, 8);

        return targetPosition;
    }

    public void OnGameStateChangedCallBack(GameState state)
    {
        switch (state)
        {
            case GameState.GAME:
                StartNextWave();
                break;
            case GameState.GAMEOVER: // Limpa o mapa
                _isTimerOn = false;
                DefeatAllEnemies();
                break;
        }
    }
}

[System.Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegments> segments;
}

[System.Serializable]
public struct WaveSegments
{
    [MinMaxSlider(0, 100)] public Vector2 timeStartToEnd;
    public float spawnFrequency;
    public GameObject prefab;

}
