public class Params
{
    public string Name { get; set; }
    public string Type { get; set; }
    public object Value { get; set; }

    public Params()
    {
        Name = "";
        Type = "";
        Value = "";
    }
    public Params(string Name)
    {
        this.Name = Name;
        Type = "";
        Value = "";
    }
    public Params(string Name, string Type, string Value)
    {
        this.Name = Name;
        this.Type = Type;
        this.Value = Value;
    }

}