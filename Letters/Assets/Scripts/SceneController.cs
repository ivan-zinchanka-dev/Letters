using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private RectTransform resumeButton;
    [SerializeField] private RectTransform exitButton;
    [SerializeField] private RectTransform errorImage;
    [SerializeField] private RectTransform restartButton;
    [SerializeField] private RectTransform gameEndButton;

    [SerializeField] private float buttonsSpeed = 1000.0f;       // скорость движения кнопок
    [SerializeField] private float imageSpeed = 2000.0f;         // скорость движение окна ошибок
    [SerializeField] private Results paper;

    private const float resumeButton_y_max = 660.0f;             // максимальные и минимальные координаты кнопок Продолжить и Выход
    private const float resumeButton_y_min = 40.0f;
    private const float exitButton_y_max = -180.0f;
    private const float exitButton_y_min = -800.0f;

    private const float restartButton_x_max = -600.0f;           // максимальные и минимальные координаты кнопок Играть снова и Выйти
    private const float restartButton_x_min = -1300.0f;
    private const float gameEndButton_x_max = 1300.0f;
    private const float gameEndButton_x_min = 600.0f;


    public Stamps currentStamp;
    [SerializeField] private StampController[] stamps = null;
    [SerializeField] private LetterController letter = null;

    [SerializeField] private TextMesh output;
    [SerializeField] private float leftTime = 10;                          // время в секундах
    private float gameTime;
    private DataBases db = null;

    private static DifficultyLevel level = DifficultyLevel.EASY;
    private bool pause;
    public bool endOfGame { get; private set; }
    public static bool error = false;

    public static void SetDifficultyLevel(DifficultyLevel newlevel)
    {
        level = newlevel;
    }

    public static DifficultyLevel GetDifficultyLevel()
    {
        return level;
    }

    public void GamePause()
    {
        for (short i = 0; i < stamps.Length; i++)
        {
            stamps[i].StopGame();
        }

        letter.StopGame();
        pause = true;
    }

    public void GameContinue()
    {
        for (short i = 0; i < stamps.Length; i++)
        {
            stamps[i].StartGame();
        }

        letter.StartGame();
        pause = false;
    }

    public void GameEnd() {

        paper.SetMoving(true);
        paper.SetData();

        for (short i = 0; i < stamps.Length; i++)
        {
            stamps[i].StopGame();
        }

        letter.StopGame();
        endOfGame = true;
    }

    public void GameError()
    {
        for (short i = 0; i < stamps.Length; i++)
        {
            stamps[i].StopGame();
        }

        letter.StopGame();
    }

    public void Fail()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Stats.Clear();
        Application.LoadLevel("SampleScene");
    }

    public void Exit()
    {
        Stats.Clear();
        StartSceneManager.isUpdated = false;
        Application.LoadLevel("StartScene");
    }

    public void SetGameParams()
    {
        if (level == DifficultyLevel.EASY)                  // уровень сложности
        {
            leftTime = 120.0f;                              // время сессии
            DataBases.SetLettersCount(10);                  // кол-во писем за сессию
        }
        else if (level == DifficultyLevel.MIDDLE)
        {
            leftTime = 100.0f;
            DataBases.SetLettersCount(10);
        }
        else
        {
            leftTime = 30.0f;
            DataBases.SetLettersCount(10);
        }
    }

    public void SetStamp(Stamps newStamp) {

        currentStamp = newStamp;

        for (int i = 0; i < stamps.Length; i++) {

            if (stamps[i].isVisible == false && (Stamps)stamps[i].stamp_number != currentStamp)
            {
                stamps[i].SetVisible(true);
            }
        }
    }

    public void CloseStamps() {

        currentStamp = Stamps.EMPTY;
    }

    public string ViewTime(float time)
    {
        float minutes = 0.0f, seconds = time;

        while (seconds > 59)
        {     
            seconds -= 60;
            minutes++;
        }

        return string.Format("{0:D2}:{1:D2}", (int)minutes, (int)seconds);
    }


    private void Start()
    {
        SetGameParams();
        db = GetComponent<DataBases>();
        endOfGame = false;
        pause = false;
        gameTime = 0;      
    }

    private void OnMouseUp()
    {
        Debug.Log("PAUSE");
        GamePause();
    }

    private void Update()
    {
        if (endOfGame)
        {
            restartButton.anchoredPosition = Vector2.MoveTowards(restartButton.anchoredPosition, new Vector2(restartButton_x_max, 0.0f), buttonsSpeed * Time.deltaTime);
            gameEndButton.anchoredPosition = Vector2.MoveTowards(gameEndButton.anchoredPosition, new Vector2(gameEndButton_x_min, 0.0f), buttonsSpeed * Time.deltaTime);
            return;
        }

        if (error)
        {
            GameError();
            errorImage.anchoredPosition = Vector2.MoveTowards(errorImage.anchoredPosition, new Vector2(0.0f, 0.0f), imageSpeed * Time.deltaTime);
            return;
        }

        if (pause) {

            resumeButton.anchoredPosition = Vector2.MoveTowards(resumeButton.anchoredPosition, new Vector2(0.0f, resumeButton_y_min), buttonsSpeed * Time.deltaTime);
            exitButton.anchoredPosition = Vector2.MoveTowards(exitButton.anchoredPosition, new Vector2(0.0f, exitButton_y_max), buttonsSpeed * Time.deltaTime);
            return;
        }

        resumeButton.anchoredPosition = Vector2.MoveTowards(resumeButton.anchoredPosition, new Vector2(0.0f, resumeButton_y_max), buttonsSpeed * Time.deltaTime);
        exitButton.anchoredPosition = Vector2.MoveTowards(exitButton.anchoredPosition, new Vector2(0.0f, exitButton_y_min), buttonsSpeed * Time.deltaTime);

        if (leftTime < 0) {
                                                                                        // время вышло
            db.RemoveLastLetter();
            db.Summarizing();                   
            GameEnd();                                                         
            return;
        }

        output.text = ViewTime(leftTime);
        gameTime += Time.deltaTime;

        if (gameTime >= 1)
        {
            leftTime--;
            gameTime = 0;
        }

    }

}
