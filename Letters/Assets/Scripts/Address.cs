
public struct Address {

    private string _name;
    private string _location;
    private short _house;
    private string _elaboration;
    private int _index;
    private string _city;

    public Address(string name, string location, short house, string elaboration, int index, string city) {

        this._name = name.Trim(); 
        this._location = location.Trim();
        this._house = house;
        this._elaboration = elaboration.Trim();
        this._index = index;
        this._city = city.Trim();
        this.Correction();
    }

    private void Correction() {

        short i, k = 0;

        if (_name.Length > 20)
        {
            for (i = 0; i < _name.Length; i++)
            {
                if (_name[i] == ' ')
                {
                    k++;

                    if (k == 2)
                    {
                        _name = _name.Insert(i, "\n");
                        break;
                    }
                }
            }
        }
        else
        {
            _name += '\n';
        }        

    }

    public int GetIndex() {

        return _index;
    }

    public string GetCity()
    {
        return _city;
    }

    public string ToLeftView()
    {
        string result = _name;

        if (_location.Equals(""))
        {
            result += "\n\n";
        }
        else {

            result += "\nул. " + _location + '\n';
        }

        if (_house != 0) {

            result += "д. " + _house + "   ";
        }

        result += _elaboration + "\n";

        if (_index != 0)
        {
            result += _index + "        ";
        }
        else {

            result += "           ";
        }

        if (!_city.Equals(""))
        {
            result += "г. " + _city + '\n';
        }

        return result;
          
    }

    public string ToRightView()
    {
        string result = _name;

        if (_location.Equals(""))
        {
            result += "\n\n";
        }
        else
        {
            result += "\nул. " + _location + '\n';
        }

        if (_house != 0)
        {
            result += "д. " + _house + "   ";
        }

        result += _elaboration + "\n";        

        if (!_city.Equals(""))
        {
            result += "               г. " + _city + '\n';
        }

        return result;
    }

    public override string ToString()
    {         
        return _name + "\nул. " + _location + "\nд. " + _house + "  " + _elaboration + "\n               г. " + _city + "\n";         
    }

}

    

