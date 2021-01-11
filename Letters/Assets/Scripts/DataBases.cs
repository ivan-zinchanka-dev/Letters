using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;
using TMPro;

public class DataBases : MonoBehaviour
{
    private static short lettersCount = 10;
    private const short correctListSize = 103;
    private const short incorrectListSize = 52;                         //53

    [SerializeField] private Font[] fonts = null;

    private List<Letter> letters = new List<Letter>();
    
    private Address[] correctList = new Address[correctListSize];
    private Address[] incorrectList = new Address[incorrectListSize];

    public TextMeshProUGUI errorMsg;

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
        char choiseList = (char)Random.Range(0, 2);                         // из какой БД выбираем (0 - некорректная, 1 - корректная)

        if (choiseList == 0)
        {
            choiseAddress = (short)Random.Range(0, incorrectList.Length);   // выбор письма по индексу
            return incorrectList[choiseAddress].ToLeftView();
        }
        else
        {
            choiseAddress = (short)Random.Range(0, correctList.Length);     // выбор письма по индексу
            return correctList[choiseAddress].ToLeftView();
        }

    }


    public string GetRightRandomAddress(ref int index) {

        short choiseAddress;                                                    // выбор адреса получателя
        char choiseList = (char) Random.Range(0, 2);                            // из какой БД выбираем (0 - некорректная, 1 - корректная)

        if (choiseList == 0)
        {
            choiseAddress = (short) Random.Range(0, incorrectList.Length);      // выбор письма по индексу
            index = incorrectList[choiseAddress].GetIndex();
            letters.Add(new Letter(false, Stamps.EMPTY));          
            return incorrectList[choiseAddress].ToRightView();
        }
        else
        {
            choiseAddress = (short)Random.Range(0, correctList.Length);          // выбор письма по индексу
            index = correctList[choiseAddress].GetIndex();
            letters.Add(new Letter(true, Letter.StringToStamp(correctList[choiseAddress].GetCity())));
            return correctList[choiseAddress].ToRightView();
        }   
    }

    public Font GetRandomFont()
    {       
        if (fonts.Length == 0)
        {
            return Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;       
        }
        else
        {
            short randomIndex = (short) Random.Range(0, fonts.Length);              // выбор шрифта для письма
            return fonts[randomIndex];
        }
    }

    public void SetUserData(short letterNumber, bool userMark, Stamps userStamp) {

        Debug.Log("№ of letter: " + letterNumber + " Data: " + userMark + " " + userStamp);
        letters[letterNumber - 1].InitSubjectiveData(userMark, userStamp);
    }

    public static short GetLettersCount()
    {
        return lettersCount;
    }

    public static void SetLettersCount(short count)
    {
        lettersCount = count;
    }

    private void Start()
    {             
        try
        {
            short i = 0;
            StreamReader sr = new StreamReader("Assets\\GameData\\Resources\\CorrectLetters.txt", Encoding.Default);

            while (!sr.EndOfStream && i < correctList.Length)
            {
                try
                {
                    correctList[i] = new Address(sr.ReadLine(), sr.ReadLine(), short.Parse(sr.ReadLine()), sr.ReadLine(), int.Parse(sr.ReadLine()), sr.ReadLine());
                    i++;
                }
                catch (System.Exception ex)
                {
                    SceneController.error = true;
                    errorMsg.text = "Файл \"CorrectLetters.txt\" повреждён!";
                    Debug.Log("Файл \"CorrectLetters.txt\" повреждён!");
                    break;
                }
            }

            sr.Close();

            i = 0;
            sr = new StreamReader("Assets\\GameData\\Resources\\IncorrectLetters.txt", Encoding.Default);

            while (!sr.EndOfStream && i < incorrectList.Length)
            {
                try
                {
                    incorrectList[i] = new Address(sr.ReadLine(), sr.ReadLine(), short.Parse(sr.ReadLine()), sr.ReadLine(), int.Parse(sr.ReadLine()), sr.ReadLine());
                    i++;
                }
                catch (System.Exception ex)
                {
                    SceneController.error = true;
                    errorMsg.text = "Файл \"IncorrectLetters.txt\" повреждён!";
                    Debug.Log("Файл \"IncorrectLetters.txt\" повреждён!");
                    break;
                }
            }

            //for (i = 0; i < incorrectList.Length; i++)
            //{
            //    cnt++;
            //    Debug.Log(incorrectList[i].ToString());
            //}

            sr.Close();

        }
        catch (FileNotFoundException ex) 
        {
            SceneController.error = true;
            errorMsg.text = "Ошибка! Файл " + ex.FileName + " не найден.";
            Debug.Log("Ошибка! Файл " + ex.FileName + " не найден.");
        }
        
    }

    public void Summarizing() {

        Stats.received = lettersCount;

        for (short i = 0; i < letters.Count; i++)
        {
            if (letters[i].Check())
            {
                Stats.correctlyProcessed++;               
            }
            else
            {
                Stats.incorrectlyProcessed++;
            }

            if (letters[i].GetSubjectiveMark())
            {
                Stats.sent++;
            }
            else {

                Stats.rejected++;
            }

            Debug.Log(i + ". " + letters[i] + "  RES: " + letters[i].Check());
        }
       
    }

    public void RemoveLastLetter()
    {
        letters.RemoveAt(letters.Count - 1);
    }

}
