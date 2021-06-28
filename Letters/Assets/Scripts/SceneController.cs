using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private RectTransform _resumeButton = null;
    [SerializeField] private RectTransform _exitButton = null;
    [SerializeField] private RectTransform _errorImage = null;
    [SerializeField] private RectTransform _restartButton = null;
    [SerializeField] private RectTransform _gameEndButton = null;

    [SerializeField] private float _buttonsSpeed = 1000.0f;       // скорость движения кнопок
    [SerializeField] private float _imageSpeed = 2000.0f;         // скорость движение окна ошибок
    [SerializeField] private ResultPaper _paper = null;
       
    [SerializeField] private StampController[] _stamps = null;
    [SerializeField] private LetterController _letter = null;
    [SerializeField] private TextMesh _timeOutput = null;
    [SerializeField] private float _leftTime = 10;                          // время в секундах    
    [SerializeField] private DataBases _dataBase = null;

    private static DifficultyLevel _level = DifficultyLevel.EASY;
    private bool _pause = default;
    private float _gameTime = default;

    public Stamps CurrentStamp { get; set; } = default;
    public bool EndOfGame { get; private set; } = default;
    public static bool Error { get; set; } = false;

    private const float ResumeButtonYMax = 660.0f;             // максимальные и минимальные координаты кнопок Продолжить и Выход
    private const float ResumeButtonYMin = 40.0f;
    private const float ExitButtonYMax = -180.0f;
    private const float ExitButtonYMin = -800.0f;
    private const float RestartButtonXMax = -600.0f;           // максимальные и минимальные координаты кнопок Играть снова и Выйти
    private const float GameEndButtonXMin = 600.0f;

    public static void SetDifficultyLevel(DifficultyLevel newlevel)
    {
        _level = newlevel;
    }

    public static DifficultyLevel GetDifficultyLevel()
    {
        return _level;
    }

    public void GamePause()
    {
        for (int i = 0; i < _stamps.Length; i++)
        {
            _stamps[i].StopGame();
        }

        _letter.StopGame();
        _pause = true;
    }

    public void GameContinue()
    {
        for (int i = 0; i < _stamps.Length; i++)
        {
            _stamps[i].StartGame();
        }

        _letter.StartGame();
        _pause = false;
    }

    public void GameEnd() {

        _paper.SetMoving(true);
        _paper.SetData();

        for (int i = 0; i < _stamps.Length; i++)
        {
            _stamps[i].StopGame();
        }

        _letter.StopGame();
        EndOfGame = true;
    }

    public void GameError()
    {
        for (int i = 0; i < _stamps.Length; i++)
        {
            _stamps[i].StopGame();
        }

        _letter.StopGame();
    }

    public void Fail()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Stats.Clear();
        Application.LoadLevel(1);
    }

    public void Exit()
    {
        Stats.Clear();
        StartSceneManager.IsUpdated = false;
        Application.LoadLevel(0);
    }

    public void SetGameParams()
    {
        if (_level == DifficultyLevel.EASY)                  // уровень сложности
        {
            _leftTime = 120.0f;                              // время сессии
            DataBases.LettersCount = 10;                  // кол-во писем за сессию
        }
        else if (_level == DifficultyLevel.MIDDLE)
        {
            _leftTime = 100.0f;
            DataBases.LettersCount = 10;
        }
        else
        {
            _leftTime = 30.0f;
            DataBases.LettersCount = 10;
        }
    }

    public void SetStamp(Stamps newStamp) {

        CurrentStamp = newStamp;

        for (int i = 0; i < _stamps.Length; i++) {

            if (_stamps[i].IsVisible == false && (Stamps)_stamps[i].number != CurrentStamp)
            {
                _stamps[i].IsVisible = true;
            }
        }
    }

    public void CloseStamps() {

        CurrentStamp = Stamps.EMPTY;
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
        EndOfGame = false;
        _pause = false;
        _gameTime = 0;      
    }

    private void OnMouseUp()
    {
        Debug.Log("PAUSE");
        GamePause();
    }

    private void Update()
    {
        if (EndOfGame)
        {
            _restartButton.anchoredPosition = Vector2.MoveTowards(_restartButton.anchoredPosition, new Vector2(RestartButtonXMax, 0.0f), _buttonsSpeed * Time.deltaTime);
            _gameEndButton.anchoredPosition = Vector2.MoveTowards(_gameEndButton.anchoredPosition, new Vector2(GameEndButtonXMin, 0.0f), _buttonsSpeed * Time.deltaTime);
            return;
        }

        if (Error)
        {
            GameError();
            _errorImage.anchoredPosition = Vector2.MoveTowards(_errorImage.anchoredPosition, new Vector2(0.0f, 0.0f), _imageSpeed * Time.deltaTime);
            return;
        }

        if (_pause) {

            _resumeButton.anchoredPosition = Vector2.MoveTowards(_resumeButton.anchoredPosition, new Vector2(0.0f, ResumeButtonYMin), _buttonsSpeed * Time.deltaTime);
            _exitButton.anchoredPosition = Vector2.MoveTowards(_exitButton.anchoredPosition, new Vector2(0.0f, ExitButtonYMax), _buttonsSpeed * Time.deltaTime);
            return;
        }

        _resumeButton.anchoredPosition = Vector2.MoveTowards(_resumeButton.anchoredPosition, new Vector2(0.0f, ResumeButtonYMax), _buttonsSpeed * Time.deltaTime);
        _exitButton.anchoredPosition = Vector2.MoveTowards(_exitButton.anchoredPosition, new Vector2(0.0f, ExitButtonYMin), _buttonsSpeed * Time.deltaTime);

        if (_leftTime < 0) {
                                                                                        // время вышло
            _dataBase.RemoveLastLetter();
            _dataBase.Summarizing();                   
            GameEnd();                                                         
            return;
        }

        _timeOutput.text = ViewTime(_leftTime);
        _gameTime += Time.deltaTime;

        if (_gameTime >= 1)
        {
            _leftTime--;
            _gameTime = 0;
        }

    }

}
