
class Letter
{
    private bool objectiveMark;
    private bool subjectiveMark;
    private Stamps objectiveStamp;
    private Stamps subjectiveStamp;

    public Letter(bool objectiveMark, Stamps objectiveStamp) {

        this.objectiveMark = objectiveMark;
        this.objectiveStamp = objectiveStamp;
    }

    public static Stamps StringToStamp(string city) {

        city = city.Trim();

        if (city.Equals("Минск"))
        {
            return Stamps.RED;
        }
        else if (city.Equals("Гродно")) {

            return Stamps.BROWN;
        }
        else if (city.Equals("Брест"))
        {
            return Stamps.SWAMPY;
        }
        else if (city.Equals("Гомель"))
        {
            return Stamps.GREEN;
        }
        else if (city.Equals("Витебск"))
        {
            return Stamps.BLUE;
        }
        else if (city.Equals("Могилёв") || city.Equals("Могилев"))
        {
            return Stamps.PURPLE;
        }
        else
        {
            return Stamps.EMPTY;
        }
        
    }

    public void InitSubjectiveData(bool subjectiveMark, Stamps subjectiveStamp) {

        this.subjectiveMark = subjectiveMark;
        this.subjectiveStamp = subjectiveStamp;
    }

    public override string ToString()
    {
        return objectiveMark.ToString() + "  " + objectiveStamp; 
    }

    public bool Check()
    {
        if (objectiveMark == subjectiveMark)
        {
            if (objectiveStamp == subjectiveStamp)
            {
                return true;
            }
            else {

                return false;
            }
            
        }
        else {

            return false;
        }
    }

    public bool GetSubjectiveMark() {

        return subjectiveMark;
    }

}

