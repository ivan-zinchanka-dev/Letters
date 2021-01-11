using UnityEngine;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private RectTransform startButton;
    [SerializeField] private RectTransform settingsButton;
    [SerializeField] private RectTransform settingsImage;
    [SerializeField] private Slider difficultySlider;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private float buttonsSpeed = 1000.0f;          // скорость движения кнопок
    [SerializeField] private float forwardImageSpeed = 3000.0f;     // скорость движения окна настроек к центру экрана
    [SerializeField] private float backImageSpeed = 4000.0f;        // скорость движения окна настроек от центра экрана (вправо)

    private bool isStartMenu = true;
    public static bool isUpdated = true;

    private const float startButton_y_max = 660.0f;                 //  максимальные и минимальные координаты кнопок Играть и Настройки
    private const float startButton_y_min = 40.0f;
    private const float settingsButton_y_max = -180.0f;
    private const float settingsButton_y_min = -800.0f;
    private const float settingsImage_x_max = 1500.0f;             // максимальная и минимальная координаты окна настроек
    private const float settingsImage_x_min = 0.0f;

    public void StartGame()
    {
        Application.LoadLevel("SampleScene");
    }

    public void ChangeDifficultyLevel(Slider s) {

        Debug.Log("INPUT");
        SceneController.SetDifficultyLevel((DifficultyLevel)s.value);
    } 

    private void SetDifficultyLevel()
    {
        difficultySlider.value = (float) SceneController.GetDifficultyLevel();
        Debug.Log("SET LVL: " + SceneController.GetDifficultyLevel());
    }

    private void SetVolume() {

        volumeSlider.value = DiscJockey.Volume; 
    } 


    public void SetStartMenu(bool value)
    {
        isStartMenu = value;
    }

    private void Update()
    {
        if (isStartMenu)
        { 
            startButton.anchoredPosition = Vector2.MoveTowards(startButton.anchoredPosition, new Vector2(0.0f, startButton_y_min), buttonsSpeed * Time.deltaTime);
            settingsButton.anchoredPosition = Vector2.MoveTowards(settingsButton.anchoredPosition, new Vector2(0.0f, settingsButton_y_max), buttonsSpeed * Time.deltaTime);
            settingsImage.anchoredPosition = Vector2.MoveTowards(settingsImage.anchoredPosition, new Vector2(settingsImage_x_max, 0.0f), backImageSpeed * Time.deltaTime);
        }
        else {

            if (!isUpdated)
            {
                SetDifficultyLevel();
                SetVolume();
                isUpdated = true;
            }
            
            startButton.anchoredPosition = Vector2.MoveTowards(startButton.anchoredPosition, new Vector2(0.0f, startButton_y_max), buttonsSpeed * Time.deltaTime);
            settingsButton.anchoredPosition = Vector2.MoveTowards(settingsButton.anchoredPosition, new Vector2(0.0f, settingsButton_y_min), buttonsSpeed * Time.deltaTime);
            settingsImage.anchoredPosition = Vector2.MoveTowards(settingsImage.anchoredPosition, new Vector2(settingsImage_x_min, 0.0f), forwardImageSpeed * Time.deltaTime);
        }
        
    }
}


public enum DifficultyLevel {

    EASY, MIDDLE, HARD
}