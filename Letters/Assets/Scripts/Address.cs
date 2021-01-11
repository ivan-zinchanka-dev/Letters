
public struct Address {

    private string name;
    private string location;
    private short house;
    private string elaboration;
    private int index;
    private string city;

    public Address(string name, string location, short house, string elaboration, int index, string city) {

        this.name = name.Trim(); 
        this.location = location.Trim();
        this.house = house;
        this.elaboration = elaboration.Trim();
        this.index = index;
        this.city = city.Trim();
        this.Correction();
    }

    private void Correction() {

        short i, k = 0;

        if (name.Length > 20)
        {
            for (i = 0; i < name.Length; i++)
            {
                if (name[i] == ' ')
                {
                    k++;

                    if (k == 2)
                    {
                        name = name.Insert(i, "\n");
                        break;
                    }
                }
            }
        }
        else
        {
            name += '\n';
        }        

    }

    public int GetIndex() {

        return index;
    }

    public string GetCity()
    {
        return city;
    }

    public string ToLeftView()
    {
        string result = name;

        if (location.Equals(""))
        {
            result += "\n\n";
        }
        else {

            result += "\nул. " + location + '\n';
        }

        if (house != 0) {

            result += "д. " + house + "   ";
        }

        result += elaboration + "\n";

        if (index != 0)
        {
            result += index + "        ";
        }
        else {

            result += "           ";
        }

        if (!city.Equals(""))
        {
            result += "г. " + city + '\n';
        }

        return result;
          
    }

    public string ToRightView()
    {
        string result = name;

        if (location.Equals(""))
        {
            result += "\n\n";
        }
        else
        {
            result += "\nул. " + location + '\n';
        }

        if (house != 0)
        {
            result += "д. " + house + "   ";
        }

        result += elaboration + "\n";        

        if (!city.Equals(""))
        {
            result += "               г. " + city + '\n';
        }

        return result;
    }

    public override string ToString()
    {         
        return name + "\nул. " + location + "\nд. " + house + "  " + elaboration + "\n               г. " + city + "\n";         
    }

}

    

