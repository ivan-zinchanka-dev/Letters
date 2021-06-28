using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;
using TMPro;

public class DataBases : MonoBehaviour
{
    private static short _lettersCount = 10;
    private const short CorrectListSize = 103;
    private const short IncorrectListSize = 52;                         //53

    [SerializeField] private Font[] _fonts = null;

    private List<Letter> _letters = new List<Letter>();

    private Address[] _correctList = new Address[CorrectListSize];
    private Address[] _incorrectList = new Address[IncorrectListSize];

    private const string CorrectLettersFileName = "CorrectLetters";
    private const string IncorrectLettersFileName = "IncorrectLetters";

    [SerializeField] private TextMeshProUGUI _errorMsg = null;

    public static short LettersCount { get { return _lettersCount;} set { _lettersCount = value; } }

    public string AdaptIndex(int numbers)
    {  
        if (numbers == 0) {

            return "";
        }

        string tmp = numbers.ToString();
        string result = "";

        for (short i = 0; i < tmp.Length - 1; i++) {

            result += tmp[i] + " ";
        }

        result += tmp[tmp.Length - 1];
        return result;
    }

    public string GetLeftRandomAddress() {

        short choiseAddress;                                                // выбор адреса отпраителя
        byte choiseList = (byte)Random.Range(0, 2);                         // из какой БД выбираем (0 - некорректная, 1 - корректная)

        if (choiseList == 0)
        {
            choiseAddress = (short)Random.Range(0, _incorrectList.Length);   // выбор письма по индексу
            return _incorrectList[choiseAddress].ToLeftView();
        }
        else
        {
            choiseAddress = (short)Random.Range(0, _correctList.Length);     // выбор письма по индексу
            return _correctList[choiseAddress].ToLeftView();
        }

    }


    public string GetRightRandomAddress(ref int index) {

        short choiseAddress;                                                    // выбор адреса получателя
        byte choiseList = (byte) Random.Range(0, 2);                            // из какой БД выбираем (0 - некорректная, 1 - корректная)

        if (choiseList == 0)
        {
            choiseAddress = (short) Random.Range(0, _incorrectList.Length);      // выбор письма по индексу
            index = _incorrectList[choiseAddress].GetIndex();
            _letters.Add(new Letter(false, Stamps.EMPTY));          
            return _incorrectList[choiseAddress].ToRightView();
        }
        else
        {
            choiseAddress = (short)Random.Range(0, _correctList.Length);          // выбор письма по индексу
            index = _correctList[choiseAddress].GetIndex();
            _letters.Add(new Letter(true, Letter.StringToStamp(_correctList[choiseAddress].GetCity())));
            return _correctList[choiseAddress].ToRightView();
        }   
    }

    public Font GetRandomFont()
    {       
        if (_fonts.Length == 0)
        {
            return Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;       
        }
        else
        {
            short randomIndex = (short) Random.Range(0, _fonts.Length);              // выбор шрифта для письма
            return _fonts[randomIndex];
        }
    }

    public void SetUserData(short letterNumber, bool userMark, Stamps userStamp) {

        Debug.Log("№ of letter: " + letterNumber + " Data: " + userMark + " " + userStamp);
        _letters[letterNumber - 1].InitSubjectiveData(userMark, userStamp);
    }

    //public static short GetLettersCount()
    //{
    //    return _lettersCount;
    //}

    //public static void SetLettersCount(short count)
    //{
    //    _lettersCount = count;
    //}

    private void Start()
    {
        try
        {
            short i = 0;
            TextAsset textAsset = Resources.Load(CorrectLettersFileName) as TextAsset;
            StringReader reader = new StringReader(textAsset.text);

            while (i < _correctList.Length) {

                _correctList[i] = new Address(reader.ReadLine(), reader.ReadLine(), 
                    short.Parse(reader.ReadLine()), reader.ReadLine(), int.Parse(reader.ReadLine()), reader.ReadLine());
                i++;
            }

            i = 0;
            textAsset = Resources.Load(IncorrectLettersFileName) as TextAsset;
            reader = new StringReader(textAsset.text);

            while (i < _incorrectList.Length)
            {
                _incorrectList[i] = new Address(reader.ReadLine(), reader.ReadLine(), short.Parse(reader.ReadLine()),
                    reader.ReadLine(), int.Parse(reader.ReadLine()), reader.ReadLine());
                i++;
            }
        }
        catch (System.Exception ex) 
        {
            SceneController.Error = true;
            _errorMsg.text = ex.Message;
            Debug.Log(_errorMsg.text);
        } 
    }

    public void Summarizing() {

        Stats.received = _lettersCount;

        for (short i = 0; i < _letters.Count; i++)
        {
            if (_letters[i].Check())
            {
                Stats.correctlyProcessed++;               
            }
            else
            {
                Stats.incorrectlyProcessed++;
            }

            if (_letters[i].GetSubjectiveMark())
            {
                Stats.sent++;
            }
            else {

                Stats.rejected++;
            }

            Debug.Log(i + ". " + _letters[i] + "  RES: " + _letters[i].Check());
        }
       
    }

    public void RemoveLastLetter()
    {
        _letters.RemoveAt(_letters.Count - 1);
    }

}
