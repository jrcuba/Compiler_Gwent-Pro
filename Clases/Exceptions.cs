public class Exceptions
{
    public string excepción { get; private set; }
    public int Columna { get; private set; }
    public int Fila { get; private set; }
    public Exceptions(string excepción, int Columna, int Fila)
    {
        this.excepción = excepción;
        this.Columna = Columna;
        this.Fila = Fila;
    }
}