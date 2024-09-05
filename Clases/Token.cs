using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


public class Token
{
    public string Key { get; private set; }
    public string Value { get; set; }

    public Token(string key, string value)
    {
        this.Value = value;
        this.Key = key;
    }
}
public class TokenType
{
    public static Dictionary<string, Regex> Tokens = new Dictionary<string, Regex>
    {
        { "CORCHETE_ABIERTO", new Regex(@"\[", RegexOptions.Compiled) },
        { "CORCHETE_CERRADO", new Regex(@"\]", RegexOptions.Compiled) },
        { "STRING", new Regex(@"([""'])(?:(?=(\\?))\2.)*?\1", RegexOptions.Compiled) }, // Expresión regular ajustada para strings
        { "PALABRA_CLAVE", new Regex(@"\b(public|private|if|else|for|while|return|effect|Action|in|while)\b", RegexOptions.Compiled) },
        { "PARENTESIS_CERRADO", new Regex(@"\)", RegexOptions.Compiled) },
        { "PARENTESIS_ABIERTO", new Regex(@"\(", RegexOptions.Compiled) },
        { "LLAVE_CERRADA", new Regex(@"\}", RegexOptions.Compiled) },
        { "LLAVE_ABIERTA", new Regex(@"\{", RegexOptions.Compiled) },
        { "NUMERO", new Regex(@"\d+", RegexOptions.Compiled) },
        { "PALABRA", new Regex(@"[a-zA-ZñÑáéíóúÁÉÍÓÚ]+", RegexOptions.Compiled) },
        { "OPERADOR", new Regex(@"[+\-*/%=&|<>^!]|=>", RegexOptions.Compiled) },
        { "IDENTIFICADOR", new Regex(@"[_a-zA-Z][_a-zA-Z0-9]*", RegexOptions.Compiled) },
        { "DOS_PUNTOS", new Regex(@":", RegexOptions.Compiled) },
        { "PUNTO_Y_COMA", new Regex(@";", RegexOptions.Compiled) },
        { "COMILLA_SIMPLE", new Regex(@"'", RegexOptions.Compiled) },
        { "COMA", new Regex(@",", RegexOptions.Compiled) },
        { "PUNTO", new Regex(@".", RegexOptions.Compiled) },
        { "COMENTARIO_SIMPLE", new Regex(@"//", RegexOptions.Compiled) }, // para soporte de errores
    };
}