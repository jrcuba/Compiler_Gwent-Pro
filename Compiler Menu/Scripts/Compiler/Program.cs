using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Program : MonoBehaviour
{
    public Text text;
    public InputField InputField;
    
    
    private Lexer Lexer = new Lexer();
    private List<Token> tokens = new List<Token>();

    private Parser parser = new Parser();
    public static ActionParser actionParser = new ActionParser();
    public static List<Exceptions> exceptions = new List<Exceptions>();
    
    
    public static List<Effect> effects;//los efectos se guardan permanentemente
    public static List<Card> cards = new List<Card>();
    void Update()
    {
        cards = new List<Card>();
        effects = new List<Effect>(); 
        int visitant = 0;
        string texto = "";
        tokens = Lexer.Tokenize(InputField.text,exceptions);

        if (visitant < tokens.Count)
        {
            parser.ReadTokens(tokens,visitant,ref exceptions,ref effects,ref cards);
        }
        foreach (var exception in exceptions)
        {
            texto += exception.excepción + "\n";
        }
        text.text = texto;
        exceptions = new List<Exceptions>();
        //TODO : arreglar ligero error en excepción de rango
    }
}