using TMPro;
using UnityEngine;

public class WaveManagerUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] TextMeshProUGUI _waveText;
    [SerializeField] TextMeshProUGUI _timerText;


    public void UpdateWaveText(string waveString) => _waveText.text = waveString;
    public void UpdateWaveTimer(string timerString) => _timerText.text = timerString;


}
