using UnityEngine;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private RectTransform _startButton = null;
    [SerializeField] private RectTransform _settingsButton = null;
    [SerializeField] private RectTransform _settingsImage = null;
    [SerializeField] private Slider _difficultySlider = null;
    [SerializeField] private Slider _volumeSlider = null;
    [SerializeField] private float _buttonsSpeed = 1000.0f;          // скорость движения кнопок
    [SerializeField] private float _forwardImageSpeed = 3000.0f;     // скорость движения окна настроек к центру экрана
    [SerializeField] private float _backImageSpeed = 4000.0f;        // скорость движения окна настроек от центра экрана (вправо)
    [SerializeField] private DiscJockey _discJockey = null;

    private bool isStartMenu = true;
    public static bool IsUpdated { get; set; } = false;

    private const float StartButtonYMax = 660.0f;                 //  максимальные и минимальные координаты кнопок Играть и Настройки
    private const float StartButtonYMin = 40.0f;
    private const float SettingsButtonYMax = -180.0f;
    private const float SettingsButtonYMin = -800.0f;
    private const float SettingsImageXMax = 1500.0f;             // максимальная и минимальная координаты окна настроек
    private const float SettingsImageXMin = 0.0f;

    public void StartGame()
    {
        Application.LoadLevel(1);
    }

    public void ChangeDifficultyLevel(Slider s) {

        Debug.Log("INPUT");
        SceneController.SetDifficultyLevel((DifficultyLevel)s.value);
    } 

    private void SetDifficultyLevel()
    {
        _difficultySlider.value = (float) SceneController.GetDifficultyLevel();
        Debug.Log("SET LVL: " + _difficultySlider.value);
    }

    private void SetVolume() {

        _volumeSlider.value = _discJockey.Volume; 
    } 


    public void SetStartMenu(bool value)
    {
        isStartMenu = value;
    }

    private void Start()
    {
        SceneController.InitDifficultyLevel();
        
    }

    private void Update()
    {
        if (isStartMenu)
        { 
            _startButton.anchoredPosition = Vector2.MoveTowards(_startButton.anchoredPosition, new Vector2(0.0f, StartButtonYMin), _buttonsSpeed * Time.deltaTime);
            _settingsButton.anchoredPosition = Vector2.MoveTowards(_settingsButton.anchoredPosition, new Vector2(0.0f, SettingsButtonYMax), _buttonsSpeed * Time.deltaTime);
            _settingsImage.anchoredPosition = Vector2.MoveTowards(_settingsImage.anchoredPosition, new Vector2(SettingsImageXMax, 0.0f), _backImageSpeed * Time.deltaTime);
        }
        else {

            if (!IsUpdated)
            {
                SetDifficultyLevel();
                SetVolume();
                IsUpdated = true;
            }
            
            _startButton.anchoredPosition = Vector2.MoveTowards(_startButton.anchoredPosition, new Vector2(0.0f, StartButtonYMax), _buttonsSpeed * Time.deltaTime);
            _settingsButton.anchoredPosition = Vector2.MoveTowards(_settingsButton.anchoredPosition, new Vector2(0.0f, SettingsButtonYMin), _buttonsSpeed * Time.deltaTime);
            _settingsImage.anchoredPosition = Vector2.MoveTowards(_settingsImage.anchoredPosition, new Vector2(SettingsImageXMin, 0.0f), _forwardImageSpeed * Time.deltaTime);
        }
        
    }
}


public enum DifficultyLevel {

    EASY, MIDDLE, HARD
}