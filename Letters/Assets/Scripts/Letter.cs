
class Letter
{
    private bool _objectiveMark = default;
    private bool _subjectiveMark = default;
    private Stamps _objectiveStamp = default;
    private Stamps _subjectiveStamp = default;

    public Letter(bool objectiveMark, Stamps objectiveStamp) {

        _objectiveMark = objectiveMark;
        _objectiveStamp = objectiveStamp;
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

        _subjectiveMark = subjectiveMark;
        _subjectiveStamp = subjectiveStamp;
    }

    public override string ToString()
    {
        return _objectiveMark.ToString() + "  " + _objectiveStamp; 
    }

    public bool Check()
    {
        if (_objectiveMark == _subjectiveMark)
        {
            if (_objectiveStamp == _subjectiveStamp)
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

        return _subjectiveMark;
    }

}

