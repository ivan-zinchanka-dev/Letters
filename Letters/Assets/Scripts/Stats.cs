using UnityEngine;

public struct Stats
{
    static public short received = 0;
    static public short sent = 0;
    static public short rejected = 0;
    static public short correctlyProcessed = 0;
    static public short incorrectlyProcessed = 0;

    public static string PrintStats()
    {
        return string.Format("Получено писем: {0}\nОтправлено: {1}\nОтбраковано: {2}\nПравильно обработано: {3}\nНеправильно обработано: {4}\n",
            received, sent, rejected, correctlyProcessed, incorrectlyProcessed);
    }

    public static Color GetTextColor() {

        float percent = correctlyProcessed / (float)received * 100;                 // оценка в процентах

        Debug.Log("P: " + percent);

        if (percent <= 25)
        {
            return new Color(1.0f, 0.0f, 0.0f);
        }
        else if (percent < 50)
        {
            return new Color(1.0f, 0.5f, 0.0f);
        }
        else if (percent <= 75)
        {
            return new Color(1.0f, 1.0f, 0.0f);
        }
        else
        {
            return new Color(0.0f, 1.0f, 0.0f);
        }

    }

    public static string PrintResult()
    {
        return string.Format("{0}/{1}", correctlyProcessed, received);
    }

    public static void Clear()
    {
        received = 0;
        sent = 0;
        rejected = 0;
        correctlyProcessed = 0;
        incorrectlyProcessed = 0;
    }

}

