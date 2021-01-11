using UnityEngine;

public class LetterController : MonoBehaviour
{
    private Transform letter;
    private BoxCollider2D collider;
    [SerializeField] private Label label;
    [SerializeField] private SceneController sceneController;
    [SerializeField] private TextMesh RightText;
    [SerializeField] private TextMesh RightTextIndex;
    [SerializeField] private TextMesh LeftText;

    [SerializeField] private SpriteRenderer WasteBasket;
    [SerializeField] private SpriteRenderer Bag;

    private DataBases db;    
    Vector3 fingerPos, startPos, deskPos;
    float dx = 0.0f, dy = 0.0f;
    bool isTrash = false, isExamined = false, isRestart = false, isReceived = false, isWrited = false;

    bool NoLetters = false;

    [SerializeField] private float speed = 25.0f;                                   // скорость перемещения письма
    const float upper_bound = 1.0f, lower_bound = -2.5f;                            // границы поля, в рамках которого можно перемещать письмо
    const float left_bound = -5.0f, right_bound = 5.0f;
    const float trash_can = -14.0f, back_pack = 14.0f;                              // не трогать
    const float left_x = -3.0f, right_x = 3.0f, upper_y = 0.0f, lower_y = -3.0f;    // границы поля, в котором можно ставить печать
    const float max_angle = 5.5f;                                                   // максимальный угол поворота письма

    const float coef = 0.25f;
    const float max_alpha = 1f;

    short generalLettersCount; 
    short processedLettersCount;

    public void StopGame() {
      
        collider.enabled = false;
    }

    public void StartGame() {

        collider.enabled = true;
    }

    private void UpdateColors() {                                               
                                                                                   // обновление цветов мусорной урны(красный) и портфеля(зелёный) в зависимости от позиции письма 
        var tmpColor = WasteBasket.color;
        tmpColor.a = Mathf.Clamp(-1 * coef * letter.position.x, 0, max_alpha);
        WasteBasket.color = tmpColor;

        tmpColor = Bag.color;
        tmpColor.a = Mathf.Clamp(coef * letter.position.x, 0, max_alpha);
        Bag.color = tmpColor;
    }

    private void Start() {
  
        letter = GetComponent<Transform>();
        collider = GetComponent<BoxCollider2D>();
        db = sceneController.GetComponent<DataBases>();
        deskPos = new Vector2(0.0f, -1.45f);
        startPos = letter.position;

        generalLettersCount = DataBases.GetLettersCount();
        processedLettersCount = 0;

    }

    private void OnMouseDown(){

        if (sceneController.endOfGame) {

            return;       
        }

        isRestart = false;
        fingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dx = letter.position.x - fingerPos.x;
        dy = letter.position.y - fingerPos.y;

        if (fingerPos.x > left_x && fingerPos.x < right_x && fingerPos.y > lower_y && fingerPos.y < upper_y)
        {
            label.DrawLabel(sceneController.currentStamp, fingerPos);
        }     
    }

    private void OnMouseUp(){

        if (!isTrash && !isExamined) {

            isRestart = true;  
        }   
    }

    private void OnMouseDrag(){

        if (!isReceived || sceneController.endOfGame) {

            return;
        }

        float tmp_y;
        fingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (letter.position.x > right_bound)
        {
            isExamined = true;
        }
        else if (letter.position.x < left_bound)
        {
            isTrash = true;
        }
        else {

            tmp_y = fingerPos.y + dy;

            if (tmp_y > upper_bound || tmp_y < lower_bound) {

                return;
            }
            else {

                letter.position = new Vector2(fingerPos.x + dx, tmp_y);
            } 
        }        
    }

    private void Update(){

        if (SceneController.error)
        {
            return;
        }

        UpdateColors();

        if ((processedLettersCount == generalLettersCount))
        {
            if (!NoLetters)
            {
                db.Summarizing();                             // все письма обработаны
                sceneController.GameEnd();
                NoLetters = true;
            }

            return;
        }

        if (letter.position.x <= trash_can || letter.position.x >= back_pack) {         
                                                                                                            //очистка письма
            letter.position = new Vector2(startPos.x, startPos.y);
            letter.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(-1 * max_angle, max_angle));

            processedLettersCount++;                                                                        // зачёт письма

            if (isTrash)
            {
                db.SetUserData(processedLettersCount, false, Stamps.EMPTY);
            }
            else if (isExamined) {

                db.SetUserData(processedLettersCount, true, label.GetCurrentLabel());      
            }

            isReceived = false;
            isTrash = false;
            isExamined = false;
            isRestart = false;
            isWrited = false;

            label.ClearLabel();
        }
        else if (!isReceived) {

            if (!isWrited) {

                int addresIndex = 0;

                RightText.text = db.GetRightRandomAddress(ref addresIndex);                  
                RightTextIndex.text = db.AdaptIndex(addresIndex);

                LeftText.text = db.GetLeftRandomAddress();

                RightText.font = RightTextIndex.font = LeftText.font = db.GetRandomFont();

                isWrited = true;
            }
        
            letter.position = Vector2.MoveTowards(letter.position, deskPos, speed * Time.deltaTime);

            if (letter.position.Equals(deskPos)) {

                isReceived = true;
            }
        }
        else if (isTrash)
        {
            letter.position = Vector2.MoveTowards(letter.position, new Vector2(trash_can, letter.position.y), speed * Time.deltaTime);           
        }
        else if (isExamined)
        {
            letter.position = Vector2.MoveTowards(letter.position, new Vector2(back_pack, letter.position.y), speed * Time.deltaTime);        
        }
        else if (isRestart) {

            letter.position = Vector2.MoveTowards(letter.position, new Vector2(deskPos.x, deskPos.y), speed * Time.deltaTime);
        }
        
    }


}
