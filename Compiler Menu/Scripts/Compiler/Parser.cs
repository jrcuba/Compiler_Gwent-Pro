using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
public class Parser
{
    public object ReadTokens(List<Token> tokens, int i, ref List<Exceptions> exceptions, ref List<Effect> effects, ref List<Card> cards)
    {
        //Lectura de efectos
        if (tokens[i].Value == "effect")
        {
            effects.Add(new Effect());
            if (tokens[i + 1].Value == "{")
            {
                for (int j = i + 2; j < tokens.Count; j++)
                {
                    //lógica del nombre
                    if (tokens[j].Value == "Name")
                    {
                        if (tokens[j + 1].Value == ":")
                        {
                            if (tokens[j + 2].Key == "STRING")
                            {
                                effects[effects.Count - 1].Name = tokens[j + 2].Value;
                            }
                            else
                            {
                                exceptions.Add(new Exceptions("Error ,no se puede tomar como nombre de el efecto algo que no sea un string", 0, 0));
                            }
                        }
                        else
                        {
                            exceptions.Add(new Exceptions("Error ,faltan los 2 puntos en la declaración del nombre del efecto o está algo que no debería en su logar", 0, 0));
                        }
                    }
                    //lógica de carga de parámetros
                    else if (tokens[j].Value == "Params")
                    {
                        if (tokens[j + 1].Value == ":")
                        {
                            if (tokens[j + 2].Value == "{")
                            {
                                for (int k = j + 3; k < tokens.Count; k++)
                                {
                                    if (tokens[k].Value == "}")
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        if (tokens[k].Key == "PALABRA" && tokens[k].Value != "Bool" && tokens[k].Value != "String" && tokens[k].Value != "Number")
                                        {
                                            if (tokens[k + 1].Value == ":")
                                            {
                                                if (tokens[k + 2].Value == "Number" || tokens[k + 2].Value == "Bool" || tokens[k + 2].Value == "String")
                                                {
                                                    Params Aux = new Params();
                                                    Aux.Name = tokens[k].Value;
                                                    Aux.Type = tokens[k + 2].Value;
                                                    effects[effects.Count - 1].Params.Add(Aux);
                                                }
                                                else
                                                {
                                                    exceptions.Add(new Exceptions("Error en la declaración del tipo de parámetro,el tipo de parámetro a adquirir es incorrecto", 0, 0));
                                                }
                                            }
                                            else
                                            {
                                                exceptions.Add(new Exceptions("Error en la declaración del tipo de parámetro,faltaron los 2 puntos", 0, 0));
                                            }
                                        }
                                        else
                                        {
                                            //TODO : ver que hago con esta excepción porque se repite con mal manejo
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //Lógica de carga de acción
                    else if (tokens[j].Value == "Action")
                    {
                        if (tokens[j + 1].Value == ":")
                        {
                            if (tokens[j + 2].Value == "(")
                            {
                                if (tokens[j + 3].Value == "targets")
                                {
                                    if (tokens[j + 4].Value == ",")
                                    {
                                        if (tokens[j + 5].Value == "context")
                                        {
                                            if (tokens[j + 6].Value == ")")
                                            {
                                                if (tokens[j + 7].Value == "=")
                                                {
                                                    if (tokens[j + 8].Value == ">")
                                                    {
                                                        if (tokens[j + 9].Value == "{")
                                                        {
                                                            for (int k = j + 10; k < tokens.Count; k++)
                                                            {
                                                                if (tokens[k].Value == "card" || tokens[k].Value == "effect")
                                                                {
                                                                    break;
                                                                }
                                                                else if (k == tokens.Count - 1)
                                                                {
                                                                    effects[effects.Count - 1].tokens.Add(tokens[k]);
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    effects[effects.Count - 1].tokens.Add(tokens[k]);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            exceptions.Add(new Exceptions("Error ,falta el corchete de apertura", 0, 0));
                                                        }
        
                                                    }
                                                    else
                                                    {
                                                        exceptions.Add(new Exceptions("Error ,falta '>'", 0, 0));
                                                    }
                                                }
                                                else
                                                {
                                                    exceptions.Add(new Exceptions("Error ,falta '='", 0, 0));
                                                }
                                            }
                                            else
                                            {
                                                exceptions.Add(new Exceptions("Error ,falta ')'", 0, 0));
                                            }
                                        }
                                        else
                                        {
                                            exceptions.Add(new Exceptions("Error ,falta 'context'", 0, 0));
                                        }
                                    }
                                    else
                                    {
                                        exceptions.Add(new Exceptions("Error ,falta ','", 0, 0));
                                    }
                                }
                                else
                                {
                                    exceptions.Add(new Exceptions("Error ,falta 'targets'", 0, 0));
                                }
                            }
                            else
                            {
                                exceptions.Add(new Exceptions("Error ,falta '('", 0, 0));
                            }
                        }
                        else
                        {
                            exceptions.Add(new Exceptions("Error ,falta ':'", 0, 0));
                        }
                    }
                    else if (tokens[j].Value == "effect" || tokens[j].Value == "card" || j == tokens.Count - 1)
                    {
                        break;
                    }
                }
            }
            else
            {
                exceptions.Add(new Exceptions("Error ,falta el corchete de apertura en la declaración del efecto", 0, 0));
            }
        }
        //Parseo de cartas
        else if (tokens[i].Value == "card")
        {
            // Crear un GameObject padre para las cartas
            GameObject cardParent = new GameObject("CardParent");

            // Añadir una nueva carta usando el método de fábrica
            cards.Add(Card.CreateCard(cardParent, "", 0, "", "", new List<string>(), new Player(new List<Card>(), ""), ""));
            if (i + 1 < tokens.Count) {
                if (tokens[i + 1].Value == "{")
                {
                    for (int j = i + 2; j < tokens.Count; j++)
                    {
                        //Lógica de tipo de carta
                        if (tokens[j].Value == "Type")
                        {
                            if (tokens[j + 1].Value == ":")
                            {
                                if (tokens[j + 2].Key == "STRING")
                                {
                                    if (tokens[j + 2].Value == "'Oro'" || tokens[j + 2].Value == "'Plata'" || tokens[j + 2].Value == "'Clima'" || tokens[j + 2].Value == "'Aumento'" || tokens[j + 2].Value == "'Líder'")
                                    {
                                        switch (tokens[j + 2].Value)
                                        {
                                            case "'Oro'":
                                                cards[cards.Count - 1].Type = "Monster";
                                                cards[cards.Count - 1].SpecialType = "Gold";
                                                break;
                                            case "'Plata'":
                                                cards[cards.Count - 1].Type = "Monster";
                                                cards[cards.Count - 1].SpecialType = "Silver";
                                                break;
                                            case "'Clima'":
                                                cards[cards.Count - 1].Type = "Effect";
                                                break;
                                            case "'Aumento'":
                                                cards[cards.Count - 1].Type = "Effect";
                                                break;
                                            case "'Líder'":
                                                cards[cards.Count - 1].SpecialType = "Líder";
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        exceptions.Add(new Exceptions("Error ,el tipo de la carta no es válido", 0, 0));
                                    }
                                }
                                else
                                {
                                    exceptions.Add(new Exceptions("Error ,el tipo de carta debe ser un String", 0, 0));
                                }
                            }
                            else
                            {
                                exceptions.Add(new Exceptions("Error ,faltan los 2 puntos en la declaración del tipo de carta", 0, 0));
                            }
                        }
                        //Lógica del nombre de la carta
                        else if (tokens[j].Value == "Name")
                        {
                            if (tokens[j + 1].Value == ":")
                            {
                                if (tokens[j + 2].Key == "STRING")
                                {
                                    cards[cards.Count - 1].Name = tokens[j + 2].Value;
                                }
                                else
                                {
                                    exceptions.Add(new Exceptions("Error ,el nombre de la carta debe ser un string", 0, 0));
                                }
                            }
                            else
                            {
                                exceptions.Add(new Exceptions("Error ,faltan los 2 puntos en la declaración del nombre de la carta", 0, 0));
                            }
                        }
                        //Lógica de la facción de la carta
                        else if (tokens[j].Value == "Faction")
                        {
                            if (tokens[j + 1].Value == ":")
                            {
                                if (tokens[j + 2].Key == "STRING")
                                {
                                    cards[cards.Count - 1].Faction = tokens[j + 2].Value;
                                }
                                else
                                {
                                    exceptions.Add(new Exceptions("Error ,el nombre de la facción de la carta debe ser un string", 0, 0));
                                }
                            }
                            else
                            {
                                exceptions.Add(new Exceptions("Error ,faltan los 2 puntos en la declaración de la facción de la carta", 0, 0));
                            }
                        }
                        //Lógica del poder de la carta
                        else if (tokens[j].Value == "Power")
                        {
                            if (tokens[j + 1].Value == ":")
                            {
                                if (tokens[j + 2].Key == "NUMERO")
                                {
                                    cards[cards.Count - 1].Power = double.Parse(tokens[j + 2].Value);
                                }
                                else
                                {
                                    exceptions.Add(new Exceptions("Error , el valor de poder de la carta debe ser un número", 0, 0));
                                }
                            }
                            else
                            {
                                exceptions.Add(new Exceptions("Error ,faltan los 2 puntos en la declaración del poder de la carta", 0, 0));
                            }
                        }

                        //Lógica del rango de la carta
                        else if (tokens[j].Value == "Range")
                        {
                            if (tokens[j + 1].Value == ":")
                            {
                                if (tokens[j + 2].Value == "[")
                                {
                                    for (int k = j + 3; k < tokens.Count; k++)
                                    {
                                        if (tokens[k].Value == "]")
                                        {
                                            AddRange(tokens[k], cards[cards.Count - 1]);
                                            return ReadTokens(tokens,k + 1,ref exceptions,ref effects,ref cards);
                                        }
                                        else if (tokens[k].Value == "'Melee'" || tokens[k].Value == "'Ranged'" || tokens[k].Value == "'Siege'")
                                        {
                                            AddRange(tokens[k], cards[cards.Count - 1]);
                                        }
                                        else
                                        {
                                            exceptions.Add(new Exceptions("Error, tipo de rango incorrecto", 0, 0));
                                        }
                                    }
                                }
                                else
                                {
                                    exceptions.Add(new Exceptions("Falta el corchete después de los 2 puntos", 0, 0));
                                }
                            }
                            else
                            {
                                exceptions.Add(new Exceptions("Error, faltan los 2 puntos luego de declarar el rango", 0, 0));
                            }
                        }
                    }
                }
                else
                {
                    exceptions.Add(new Exceptions("Error ,falta el corchete de apertura de la carta", 0, 0));
                }
            }
        }
        //Caso de efecto abstracto de la carta
        else if (tokens[i].Value == "OnActivation")
        {
            List<Token> Aux = new List<Token>();
            for (int k = i; k < tokens.Count; k++)
            {
                if (tokens[k].Value == "card" || tokens[k].Value == "effect")
                {
                    break;
                }
                Aux.Add(tokens[k]);
            }
            cards[cards.Count - 1].OnActivationTokens = Aux;
        }
        else if (tokens[i].Value == ",")
        {
            if (i + 1 > tokens.Count)
            {
                exceptions.Add(new Exceptions("Error ,debe ir algo después de la coma", 0, 0));
            }
        }
        else if (tokens[i].Value == "}")
        {

        }
        //Lectura de cartas
        if (i <= tokens.Count - 2)
        {
            ReadTokens(tokens, i + 1, ref exceptions, ref effects, ref cards);
        }
        return 0;
    }
    public void AddRange(Token token,Card card)
    {
        switch (token.Value)
        {
            case "'Melee'":
                card.Range.Add("Melee");
                break;
            case "'Ranged'":
                card.Range.Add("Ranged");
                break;
            case "'Siege'":
                card.Range.Add("Siege");
                break;
            default:
                break;
        }
    }
}