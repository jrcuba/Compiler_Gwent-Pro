using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Lexer
{
    //método que devuelve una lista de tokens a partir del string de entrada
    public List<Token> Tokenize(string input, List<Exceptions> exceptions)
    {
        //lista de tokens vacía que se va a retornar
        List<Token> tokens = new List<Token>();
        //línea y columna en la que se va trabajando para manejo de errores
        int lineNumber = 1;
        int columnNumber = 0;


        //Bucle para buscar el tipo de token usando expresiones regulares
        foreach (Match match in Regex.Matches(input, @"""([^""\\]*(?:\\.[^""\\]*)*)""|'([^'\\]*(?:\\.[^'\\]*)*)'|[\wñÑáéíóúÁÉÍÓÚ]+|[^a-zA-Z0-9\s]"))
        {
            string value = match.Value;//guardo el valor de la expresión regular
            bool matchFound = false;//booleano que revisa si encuentra alguna coincidencia con la expresiones regulares
            //recorro todas mis expresiones regulares
            foreach (var x in TokenType.Tokens)
            {
                //verifico si hay alguna coincidencia con algún token
                if (x.Value.IsMatch(value))
                {
                    tokens.Add(new Token(x.Key, value));
                    matchFound = true;
                    break;
                }
            }
            //si no hay la coincidencia doy un error de token no reconocido
            if (!matchFound)
            {
                //agregar a la lista de errores un nuevo error en esa linea y columna
                exceptions.Add(new Exceptions("Token No reconocido", columnNumber, lineNumber));
            }
            //manejo las filas y las columnas
            columnNumber += value.Length;
            if (match.Index > 0 && input[match.Index - 1] == '\n')
            {
                lineNumber++;
                columnNumber = 0;
            }
        }
        //devuelve la lista de tokens
        return tokens;
    }
}