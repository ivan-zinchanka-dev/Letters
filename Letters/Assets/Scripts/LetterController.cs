using UnityEngine;

public class LetterController : MonoBehaviour
{ 
    [SerializeField] private Label _label = null; 
    [SerializeField] private TextMesh _rightText = null;
    [SerializeField] private TextMesh _rightTextIndex = null;
    [SerializeField] private TextMesh _leftText = null;

    [SerializeField] private SceneController _sceneController = null;
    [SerializeField] private SpriteRenderer _wasteBasket = null;
    [SerializeField] private SpriteRenderer _bag = null;
    [SerializeField] private float _speed = 25.0f;                                      // скорость перемещения письма

    [SerializeField] private Transform _letter = null;
    [SerializeField] private BoxCollider2D _collider = null;
    
    private DataBases _dataBase = null;    
    private Vector3 _fingerPos = default, _startPos = default, _deskPos = default;
    private float _dx = 0.0f, _dy = 0.0f;
    private bool _isTrash = false, _isExamined = false, _isResumed = false, _isReceived = false, _isWrited = false;
    private bool _noLetters = false;
                                     
    private const float UpperBound = 1.0f, LowerBound = -2.5f;                            // границы поля, в рамках которого можно перемещать письмо
    private const float LeftBound = -5.0f, RightBound = 5.0f;
    private const float TrashCan = -14.0f, BackPack = 14.0f;                              // не трогать
    private const float LeftX = -3.0f, RightX = 3.0f, UpperY = 0.0f, LowerY = -3.0f;    // границы поля, в котором можно ставить печать
    private const float MaxAngle = 5.5f;                                                   // максимальный угол поворота письма
         
    private short _generalLettersCount = default; 
    private short _processedLettersCount = default;

    public void StopGame() {
      
        _collider.enabled = false;
    }

    public void StartGame() {

        _collider.enabled = true;
    }

    private void UpdateColors() {

        // обновление цветов мусорной урны(красный) и портфеля(зелёный) в зависимости от позиции письма 

        const float ColorFactor = 0.25f;
        const float MaxAlpha = 1f;
        
        var tmpColor = _wasteBasket.color;
        tmpColor.a = Mathf.Clamp(-1 * ColorFactor * _letter.position.x, 0, MaxAlpha);
        _wasteBasket.color = tmpColor;

        tmpColor = _bag.color;
        tmpColor.a = Mathf.Clamp(ColorFactor * _letter.position.x, 0, MaxAlpha);
        _bag.color = tmpColor;
    }

    private void Start() {
  
        _sceneController.TryGetComponent<DataBases>(out _dataBase);
        _deskPos = new Vector2(0.0f, -1.45f);
        _startPos = _letter.position;

        _generalLettersCount = DataBases.LettersCount;
        _processedLettersCount = 0;
    }

    private void OnMouseDown(){

        if (_sceneController.EndOfGame) {

            return;       
        }

        _isResumed = false;
        _fingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _dx = _letter.position.x - _fingerPos.x;
        _dy = _letter.position.y - _fingerPos.y;

        if (_fingerPos.x > LeftX && _fingerPos.x < RightX && _fingerPos.y > LowerY && _fingerPos.y < UpperY)
        {
            _label.DrawLabel(_sceneController.CurrentStamp, _fingerPos);
        }     
    }

    private void OnMouseUp(){

        if (!_isTrash && !_isExamined) {

            _isResumed = true;  
        }   
    }

    private void OnMouseDrag(){

        if (!_isReceived || _sceneController.EndOfGame) {

            return;
        }

        float tmp_y;
        _fingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_letter.position.x > RightBound)
        {
            _isExamined = true;
        }
        else if (_letter.position.x < LeftBound)
        {
            _isTrash = true;
        }
        else {

            tmp_y = _fingerPos.y + _dy;

            if (tmp_y > UpperBound || tmp_y < LowerBound) {

                return;
            }
            else {

                _letter.position = new Vector2(_fingerPos.x + _dx, tmp_y);
            } 
        }        
    }

    private void Update(){

        if (SceneController.Error)
        {
            return;
        }

        UpdateColors();

        if ((_processedLettersCount == _generalLettersCount))
        {
            if (!_noLetters)
            {
                _dataBase.Summarizing();                             // все письма обработаны
                _sceneController.GameEnd();
                _noLetters = true;
            }

            return;
        }

        if (_letter.position.x <= TrashCan || _letter.position.x >= BackPack) {         
                                                                                                            //очистка письма
            _letter.position = new Vector2(_startPos.x, _startPos.y);
            _letter.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(-1 * MaxAngle, MaxAngle));

            _processedLettersCount++;                                                                        // зачёт письма

            if (_isTrash)
            {
                _dataBase.SetUserData(_processedLettersCount, false, Stamps.EMPTY);
            }
            else if (_isExamined) {

                _dataBase.SetUserData(_processedLettersCount, true, _label.GetCurrentLabel());      
            }

            _isReceived = false;
            _isTrash = false;
            _isExamined = false;
            _isResumed = false;
            _isWrited = false;

            _label.ClearLabel();
        }
        else if (!_isReceived) {

            if (!_isWrited) {

                int addresIndex = 0;

                _rightText.text = _dataBase.GetRightRandomAddress(ref addresIndex);                  
                _rightTextIndex.text = _dataBase.AdaptIndex(addresIndex);

                _leftText.text = _dataBase.GetLeftRandomAddress();

                _rightText.font = _rightTextIndex.font = _leftText.font = _dataBase.GetRandomFont();

                _isWrited = true;
            }
        
            _letter.position = Vector2.MoveTowards(_letter.position, _deskPos, _speed * Time.deltaTime);

            if (_letter.position.Equals(_deskPos)) {

                _isReceived = true;
            }
        }
        else if (_isTrash)
        {
            _letter.position = Vector2.MoveTowards(_letter.position, new Vector2(TrashCan, _letter.position.y), _speed * Time.deltaTime);           
        }
        else if (_isExamined)
        {
            _letter.position = Vector2.MoveTowards(_letter.position, new Vector2(BackPack, _letter.position.y), _speed * Time.deltaTime);        
        }
        else if (_isResumed) {

            _letter.position = Vector2.MoveTowards(_letter.position, new Vector2(_deskPos.x, _deskPos.y), _speed * Time.deltaTime);
        }
        
    }


}
