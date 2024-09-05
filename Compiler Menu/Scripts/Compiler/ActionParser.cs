using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
public class ActionParser
{
    GameControllerScript gameControllerScript = new GameControllerScript();
    public object ActionParser_(Card card, List<Token> tokens, List<List<Card>> Targets, Context context, ref List<Params> Params, int i,ref List<Exceptions> exceptions)
    {
        //Casos de manejo de propiedades    
        if (tokens[i].Value == "context")
        {
            return ActionContext(card, tokens, Targets, context, ref Params, i,ref exceptions);
        }
        else if (tokens[i].Value == "target")
        {
            if (tokens[i + 1].Value == ".")
            {
                switch(tokens[i + 2].Value) 
                {
                    case "Owner":
                        return card.Owner();
                    case "Power":
                        if (tokens[i + 4].Value == "=")
                        {
                            if (tokens[i +3].Value == "=")
                            {
                                card.Power = (double)ActionParser_(card,tokens,Targets,context,ref Params,i + 5,ref exceptions);
                            }
                            else if (tokens[i + 4].Value == "+")
                            {
                                card.Power += (double)ActionParser_(card, tokens, Targets, context, ref Params, i + 5, ref exceptions);
                            }
                            else if (tokens[i + 4].Value == "-")
                            {
                                card.Power -= (double)ActionParser_(card, tokens, Targets, context, ref Params, i + 5, ref exceptions);
                            }
                        }
                        else
                        {
                            return card.Power;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        //Casos de variables
        else if (tokens[i].Key == "PALABRA")
        {
           if (tokens[i + 1].Value == "=")
             {
                int aux = ComprobateParam(tokens[i], ref Params);
                //busco el punto y coma para diferenciar de linea 
                List<Token> tokens1 = new List<Token>();
                int index = tokens.Count - 1;
                for (int k = i + 2; k < tokens.Count; k++)
                {
                    tokens1.Add(tokens[k]);
                    if (tokens[k].Value == ";")
                    {
                        index = k;
                        tokens1.Add(tokens[k]);
                        break;
                    }
                }
                Params[aux].Value = ActionParser_(card, tokens1, Targets, context, ref Params, 0,ref exceptions);
                for (int z = index; z >= i; z--)
                {
                    tokens.RemoveAt(z);
                }
                return ActionParser_(card, tokens, Targets, context, ref Params, i,ref exceptions);
            }
            //TODO: caso de que no sea asignación con variable ,sino que sea sumar el valor de la variable
            else if (tokens[i + 1].Key == "OPERADOR")
            {
                ResolveOperator(card, tokens, Targets, context, ref Params, i, ref exceptions);
            }
            else if (tokens[i + 1].Value == ";")
            {
                int aux = Params.Count;
                int aux2 = ComprobateParam(tokens[i], ref Params);
                if (aux2 == Params.Count && aux2 != aux)
                {
                    exceptions.Add(new Exceptions("Error ,la variable siguiente no está declarada como parámetro : " + tokens[i + 2].Value, 0,0));
                }
                else
                {
                    tokens.RemoveAt(i + 1);
                    tokens.RemoveAt(i);
                    return Params[aux2].Value;
                }
            }
            else
            {
                exceptions.Add(new Exceptions("Token no reconocido ,uso de variable incorrecta",0,0));
            }
        }
        //Casos de números
        else if (tokens[i].Key == "NUMERO")
        {
            //TODO: tengo que hacer el caso de que no sea un número el token[i + 2],sino una palabra o variable
            if (tokens[i + 1].Key == "OPERADOR")
            {
                ResolveOperator(card, tokens, Targets, context, ref Params, i, ref exceptions);
            }
            else
            {
                return tokens[i].Value;
            }
        }
        //bucle for
        else if (tokens[i].Value == "for")
        {
            if (tokens[i + 1].Value == "target")
            {
                if (tokens[ i + 2].Value == "in")
                {
                    if (tokens[i + 3].Value == "targets")
                    {
                        if (tokens[i + 4].Value == "{")
                        {

                            List<Token> auxlist = new List<Token>();
                            for (int k = i + 5;k < tokens.Count;k++)
                            {
                                if (tokens[k].Value == "}")
                                {
                                    i = k;
                                    break;
                                }
                                auxlist.Add(tokens[k]);
                            }
                            foreach (List<Card> targets in Targets)
                            {
                                foreach (Card target in targets)
                                {
                                    ActionParser_(target, auxlist, Targets, context, ref Params, i + 5, ref exceptions);
                                }
                            }
                        }
                    }
                }
            }
        }
        //Bucle while
        else if (tokens[i].Value == "while")
        {
            if (tokens[i + 1].Value == "i")
            {
                //busco a i en los parámetros a ver si está declarada con un valor
                int k = ComprobateParam(tokens[i],ref Params);
                if (tokens[i + 2].Value == "+")
                {
                    if (tokens[i + 3].Value == "+")
                    {
                        if (tokens[i + 4].Value == "<")
                        {
                            if (tokens[ i + 5].Key == "PALABRA")
                            {
                                //busco al token en los parámetros a ver si está declarado con un valor
                                int j = ComprobateParam(tokens[i + 5],ref Params);
                                while ((int)Params[k].Value < (int)Params[j].Value)
                                {
                                    List<Token> aux = new List<Token>();
                                    for (int r = i + 6; r < tokens.Count; r++)
                                    {
                                        if (tokens[r].Value == ";")
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            aux.Add(tokens[r]);
                                        }
                                    }
                                    ActionParser_(card,aux,Targets,context,ref Params,i + 6,ref exceptions);
                                }
                            }
                            else if (tokens[i + 5].Key == "NUMERO")
                            {
                                //hago el while
                                while ((int)Params[k].Value < int.Parse(tokens[i + 5].Value))
                                {
                                    List<Token> aux = new List<Token>();
                                    for (int r = i + 6; r < tokens.Count; r++)
                                    {
                                        if (tokens[r].Value == ";")
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            aux.Add(tokens[r]);
                                        }
                                    }
                                    ActionParser_(card, aux, Targets, context, ref Params, i + 6, ref exceptions);
                                }
                            }
                        }
                    }
                }
            }
        }
        if (i < tokens.Count - 2)
        {
            return ActionParser_(card, tokens, Targets, context, ref Params, i + 1, ref exceptions);
        }
        return "";
    }


    //método para comprobar si un parámetro ya está declarado 
    public int ComprobateParam(Token token, ref List<Params> @params)
    {
        for (int i = 0; i < @params.Count; i++)
        {
            if (token.Value == @params[i].Name)
            {
                return i;
            }
        }
        @params.Add(new Params(token.Value));
        return @params.Count - 1;
    }
    //Método para realizar operaciones entre variables y números
    public object ResolveOperator(Card card, List<Token> tokens, List<List<Card>> Targets, Context context, ref List<Params> Params, int i,ref List<Exceptions> exceptions)
    {
        if (tokens[i].Key == "NUMERO")
        {
            if (tokens[i + 2].Key == "NUMERO")
            {
                double aux;
                switch (tokens[i + 1].Value)
                {
                    case "+":
                        aux = double.Parse(tokens[i].Value) + double.Parse(tokens[i + 2].Value);
                        break;
                    case "-":
                        aux = double.Parse(tokens[i].Value) - double.Parse(tokens[i + 2].Value);
                        break;
                    case "/":
                        aux = double.Parse(tokens[i].Value) / double.Parse(tokens[i + 2].Value);
                        break;
                    case "*":
                        aux = double.Parse(tokens[i].Value) * double.Parse(tokens[i + 2].Value);
                        break;
                    case "^":
                        aux = Math.Pow(double.Parse(tokens[i].Value), double.Parse(tokens[i + 2].Value));
                        break;
                    default:
                        aux = 0;
                        break;
                }
                Token token = new Token("NUMERO", aux.ToString());
                tokens.Insert(i, token);
                tokens.RemoveAt(i + 3);
                tokens.RemoveAt(i + 2);
                tokens.RemoveAt(i + 1);
                return ActionParser_(card, tokens, Targets, context, ref Params, i,ref exceptions);
            }
            else if (tokens[i + 2].Key == "PALABRA")
            {
                double aux3;
                int aux = Params.Count;
                int aux2 = ComprobateParam(tokens[i + 2], ref Params);
                if (aux2 == Params.Count && aux2 != aux)
                {
                    exceptions.Add(new Exceptions("Error ,la variable siguiente no está declarada como parámetro : " + tokens[i + 2].Value,0,0));
                }
                else
                {
                    switch (tokens[i + 1].Value)
                    {
                        case "+":
                            aux3 = double.Parse(tokens[i].Value) + double.Parse((string)Params[aux2].Value);
                            break;
                        case "-":
                            aux3 = double.Parse(tokens[i].Value) - double.Parse((string)Params[aux2].Value);
                            break;
                        case "/":
                            aux3 = double.Parse(tokens[i].Value) / double.Parse((string)Params[aux2].Value);
                            break;
                        case "*":
                            aux3 = double.Parse(tokens[i].Value) * double.Parse((string)Params[aux2].Value);
                            break;
                        case "^":
                            aux3 = Math.Pow(double.Parse(tokens[i].Value), double.Parse((string)Params[aux2].Value));
                            break;
                        default:
                            aux3 = 0;
                            break;
                    }
                    Token token = new Token("NUMERO", aux3.ToString());
                    tokens.Insert(i, token);
                    tokens.RemoveAt(i + 3);
                    tokens.RemoveAt(i + 2);
                    tokens.RemoveAt(i + 1);
                    return ActionParser_(card, tokens, Targets, context, ref Params, i,ref exceptions);
                }
            }
        }
        else if (tokens[i].Key == "PALABRA")
        {
            double aux3;
            int aux = Params.Count;
            int aux2 = ComprobateParam(tokens[i], ref Params);
            if (aux2 == Params.Count && aux2 != aux)
            {
                exceptions.Add(new Exceptions("Error ,la variable siguiente no está declarada como parámetro : " + tokens[i].Value, 0, 0));
            }
            else
            {

                if (tokens[i + 2].Key == "NUMERO")
                {
                    switch (tokens[i + 1].Value)
                    {
                        case "+":
                            aux3 = double.Parse((string)Params[aux2].Value) + double.Parse(tokens[i + 2].Value);
                            break;
                        case "-":
                            aux3 = double.Parse((string)Params[aux2].Value) - double.Parse(tokens[i + 2].Value);
                            break;
                        case "/":
                            aux3 = double.Parse((string)Params[aux2].Value) / double.Parse(tokens[i + 2].Value);
                            break;
                        case "*":
                            aux3 = double.Parse((string)Params[aux2].Value) * double.Parse(tokens[i + 2].Value);
                            break;
                        case "^":
                            aux3 = Math.Pow(double.Parse((string)Params[aux2].Value), double.Parse(tokens[i + 2].Value));
                            break;
                        default:
                            aux3 = 0;
                            break;
                    }
                    Token token = new Token("NUMERO", aux3.ToString());
                    tokens.Insert(i, token);
                    tokens.RemoveAt(i + 3);
                    tokens.RemoveAt(i + 2);
                    tokens.RemoveAt(i + 1);
                    return ActionParser_(card, tokens, Targets, context, ref Params, i, ref exceptions);
                }
                else if (tokens[i + 2].Key == "PALABRA")
                {
                    int aux4 = Params.Count;
                    int aux5 = ComprobateParam(tokens[i + 2], ref Params);
                    if (aux4 != Params.Count)
                    {
                        exceptions.Add(new Exceptions("Error ,la variable siguiente no está declarada como parámetro : " + tokens[i + 2].Value, 0, 0));
                    }
                    else
                    {
                        switch (tokens[i + 1].Value)
                        {
                            case "+":
                                aux3 = double.Parse((string)Params[aux2].Value) + double.Parse((string)Params[aux5].Value);
                                break;
                            case "-":
                                aux3 = double.Parse((string)Params[aux2].Value) - double.Parse((string)Params[aux5].Value);
                                break;
                            case "/":
                                aux3 = double.Parse((string)Params[aux2].Value) / double.Parse((string)Params[aux5].Value);
                                break;
                            case "*":
                                aux3 = double.Parse((string)Params[aux2].Value) * double.Parse((string)Params[aux5].Value);
                                break;
                            case "^":
                                aux3 = Math.Pow(double.Parse((string)Params[aux2].Value), double.Parse((string)Params[aux5].Value));
                                break;
                            default:
                                aux3 = 0;
                                break;
                        }
                        Token token = new Token("NUMERO", aux3.ToString());
                        tokens.Insert(i, token);
                        tokens.RemoveAt(i + 3);
                        tokens.RemoveAt(i + 2);
                        tokens.RemoveAt(i + 1);
                        return ActionParser_(card, tokens, Targets, context, ref Params, i, ref exceptions);
                    }
                }
            }
        }
        return 0;
    }
    //las acciones que se van a realizar sobre cierta lista que incluye al contexto
    public object ActionContext(Card card, List<Token> tokens, List<List<Card>> Targets, Context context, ref List<Params> Params, int i,ref List<Exceptions> exceptions)
    {
        if (tokens[i + 1].Value == ".")
        {
            int k = -1;
            k = ReturnPlayerPosition(card, k);
            if (tokens[i + 3].Value == ".")
            {
                if (k != -1)
                {
                    switch (tokens[i + 2].Value)
                    {
                        case "Board":
                            return ActionsInListsOfContext(card, tokens, Targets, context, ref Params, i, ref SummonScript.InvoquedCards, ref exceptions);
                        case "Hand":
                            return ActionsInListsOfContext(card, tokens, Targets, context, ref Params, i, ref SelectDeckScript.players[k].Hand, ref exceptions);
                        case "Deck":
                            return ActionsInListsOfContext(card, tokens, Targets, context, ref Params, i, ref SelectDeckScript.players[k].DeckOfPlayer, ref exceptions);
                        case "Field":
                            if (k == 0)
                            {
                                return ActionsInListsOfContext(card, tokens, Targets, context, ref Params, i, ref SummonScript.InvoquedCardsPlayer1, ref exceptions);
                            }
                            else if (k == 1)
                            {
                                return ActionsInListsOfContext(card, tokens, Targets, context, ref Params, i, ref SummonScript.InvoquedCardsPlayer2, ref exceptions);
                            }
                            else
                            {
                                exceptions.Add(new Exceptions("Error ,no se encontró el player al que pertenece la carta", 0, 0));
                            }
                            break;
                        case "Graveyard":
                            if (k == 0)
                            {
                                return ActionsInListsOfContext(card, tokens, Targets, context, ref Params, i, ref SummonScript.CementeryPlayer1, ref exceptions);
                            }
                            else if (k == 1)
                            {
                                return ActionsInListsOfContext(card, tokens, Targets, context, ref Params, i, ref SummonScript.CementeryPlayer2, ref exceptions);
                            }
                            else
                            {
                                exceptions.Add(new Exceptions("Error ,no se encontró el player al que pertenece la carta", 0, 0));
                            }
                            break;
                            //el resto como handofplayer y esas cosas no sirven como propiedad con un punto delante ,así que tengo que verificarla en el siguiente
                    }
                }

            }
            else
            {
                if (k != -1)
                {
                    switch (tokens[i + 2].Value)
                    {
                        case "TriggetPlayer":
                            //TODO : verificar antes que esté bien
                            tokens.RemoveAt(i + 2);
                            tokens.RemoveAt(i + 1);
                            tokens.RemoveAt(i);
                            return card.PlayerAlQuePertenece;
                        case "Board":
                            //lo mismo con esto
                            tokens.RemoveAt(i + 2);
                            tokens.RemoveAt(i + 1);
                            tokens.RemoveAt(i);
                            return SummonScript.InvoquedCards;
                        case "Hand":
                            tokens.RemoveAt(i + 2);
                            tokens.RemoveAt(i + 1);
                            tokens.RemoveAt(i);
                            return SelectDeckScript.players[k].Hand;
                        case "Deck":
                            tokens.RemoveAt(i + 2);
                            tokens.RemoveAt(i + 1);
                            tokens.RemoveAt(i);
                            return SelectDeckScript.players[k].DeckOfPlayer;
                        case "Field":
                            if (k == 0)
                            {
                                return SummonScript.InvoquedCardsPlayer1;
                            }
                            else if (k == 1)
                            {
                                return SummonScript.InvoquedCardsPlayer2;
                            }
                            else
                            {
                                exceptions.Add(new Exceptions("Error ,no se encontró el jugador al que pertenece la carta", 0, 0));
                            }
                            break;
                        case "Graveyard":
                            if (k == 0)
                            {
                                return SummonScript.CementeryPlayer1;
                            }
                            else if (k == 1)
                            {
                                return SummonScript.CementeryPlayer2;
                            }
                            else
                            {
                                exceptions.Add(new Exceptions("Error ,no se encontró el jugador al que pertenece la carta", 0, 0));
                            }
                            break;
                        default:
                            if (tokens[i + 3].Value == "(")
                            {
                                Card cartica = new Card();
                                if (tokens[i + 4].Key == "PALABRA")
                                {
                                    cartica.PlayerAlQuePertenece = (string)Params[k].Value;
                                    k = ReturnPlayerPosition(cartica, k);
                                    if (k != -1)
                                    {
                                        if (k == 0 || k == 1)
                                        {
                                            if (tokens[i + 5].Value == ")")
                                            {
                                                switch (tokens[i + 2].Value)
                                                {
                                                    case "HandOfPlayer":
                                                        return SelectDeckScript.players[k].Hand;
                                                    case "DeckOfPlayer":
                                                        return SelectDeckScript.players[k].DeckOfPlayer;
                                                    case "FieldOfPlayer":
                                                        if (k == 0)
                                                        {
                                                            return SummonScript.InvoquedCardsPlayer1;
                                                        }
                                                        else
                                                        {
                                                            return SummonScript.InvoquedCardsPlayer2;
                                                        }
                                                    case "Graveyard":
                                                        if (k == 0)
                                                        {
                                                            return SummonScript.CementeryPlayer1;
                                                        }
                                                        else
                                                        {
                                                            return SummonScript.CementeryPlayer2;
                                                        }
                                                }
                                            }
                                            else
                                            {
                                                exceptions.Add(new Exceptions("Error ,faltó el corchete de cierre", 0, 0));
                                            }
                                        }
                                        else
                                        {
                                            exceptions.Add(new Exceptions("Error ,no se encontró el jugador al que pertenece la carta", 0, 0));
                                        }
                                    }
                                    else
                                    {
                                        exceptions.Add(new Exceptions("Error ,El token del player al que pertenece la carta debe ser una palabra", 0, 0));
                                    }
                                }
                                else
                                {
                                    exceptions.Add(new Exceptions("Error  ,el player al cual pertenecer debe ser pasado como palabra", 0, 0));
                                }
                            }
                            else
                            {
                                exceptions.Add(new Exceptions("Error ,falta el corchete de apertura en la declaración de HandOfPlayer", 0, 0));
                            }
                            break;
                    }
                }
            }
        }
        else
        {
            exceptions.Add(new Exceptions("Error,falta un punto después del Context ", 0, 0));
        }
        return 0;
    }
    public object ActionsInListsOfContext(Card card, List<Token> tokens, List<List<Card>> Targets, Context context, ref List<Params> Params, int i, ref List<Card> cards,ref List<Exceptions> exceptions)
    {
        switch (tokens[i + 4].Value)
        {
            case "Push":
                if (tokens[i + 5].Value == "(")
                {
                    //lo de abajo debe de ser 1 carta
                    if (tokens[i + 6].Key == "PALABRA")
                    {
                        if (tokens[i + 7].Value == ")")
                        {
                            int aux = Params.Count;
                            int aux2 = ComprobateParam(tokens[i + 6], ref Params);
                            if (aux != Params.Count || aux2 == aux)
                            {
                                exceptions.Add(new Exceptions("Error ,la variable siguiente no está declarada como parámetro : " + tokens[i + 6].Value, 0, 0));
                            }
                            else
                            {
                                cards.Add((Card)Params[aux2].Value);
                            }
                        }
                        else
                        {
                            exceptions.Add(new Exceptions("Error ,falta el en la declaración del Push)",0,0));
                        }
                    }
                    else
                    {
                        exceptions.Add(new Exceptions("Error ,después de ( debe ir una palabra", 0, 0));
                    }
                }
                else
                {
                    exceptions.Add(new Exceptions("Error,falta el ( después de Push", 0, 0));
                }
                break;
            case "SendBottom":
                if (tokens[i + 5].Value == "(")
                {
                    if (tokens[i + 6].Key == "PALABRA")
                    {
                        if (tokens[i + 7].Value == ")")
                        {
                            int aux = Params.Count;
                            int aux2 = ComprobateParam(tokens[i + 6], ref Params);
                            if (aux != Params.Count || aux2 == aux)
                            {
                                exceptions.Add(new Exceptions("Error ,la variable siguiente no está declarada como parámetro : " + tokens[i + 6].Value, 0, 0));
                            }
                            else
                            {
                                cards.Insert(0, (Card)Params[aux2].Value);
                            }
                        }
                        else
                        {
                            exceptions.Add(new Exceptions("Error ,falta el en la declaración del SendBottom)", 0, 0));
                        }
                    }
                    else
                    {
                        exceptions.Add(new Exceptions("Error ,después de ( debe ir una palabra", 0, 0));
                    }
                }
                else
                {
                    exceptions.Add(new Exceptions("Error,falta el ( después de SendBottom", 0, 0));
                }
                break;
            case "Pop":
                if (tokens[i + 5].Value == "(")
                {
                    if (tokens[i + 6].Value == ")")
                    {
                        if (cards.Count != 0)
                        {
                            Card aux = cards[cards.Count - 1];
                            cards.RemoveAt(cards.Count - 1);
                            return aux;
                        }
                        else
                        {
                            exceptions.Add(new Exceptions("Error ,no hay cartas suficientes que remover", 0, 0));
                        }
                    }
                    else
                    {
                        exceptions.Add(new Exceptions("Error,falta el ) del Pop", 0, 0));
                    }
                }
                else
                {
                    exceptions.Add(new Exceptions("Error,falta el ( del Pop", 0, 0));
                }
                break;
            case "Remove":
                if (tokens[i + 5].Value == "(")
                {
                    if (tokens[i + 6].Key == "PALABRA")
                    {
                        if (tokens[i + 7].Value == ")")
                        {
                            int aux = Params.Count;
                            int aux2 = ComprobateParam(tokens[i + 6], ref Params);
                            if (aux != Params.Count || aux2 == aux)
                            {
                                exceptions.Add(new Exceptions("Variable no declarada como parámetro" + tokens[i + 6].Value,0,0));
                            }
                            else
                            {
                                foreach (Card card1 in cards)
                                {
                                    Card cardaux = (Card)Params[aux2].Value;
                                    if (card1.PlayerAlQuePertenece == cardaux.PlayerAlQuePertenece)
                                    {
                                        cards.Remove(card1);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            exceptions.Add(new Exceptions("Error,falta el ) del Remove", 0, 0));
                        }
                    }
                    else
                    {
                        exceptions.Add(new Exceptions("Error, el token después del ( del Remove debe ser una palabra", 0, 0));
                    }
                }
                else
                {
                    exceptions.Add(new Exceptions("Error,falta el ( después del Remove", 0, 0));
                }
                break;

                //TODO: me queda el Shuffle y el Find y termino


            case "Shuffle":
                if (tokens[i + 5].Value == "(")
                {
                    if (tokens[i + 7].Value == ")")
                    {
                        cards = gameControllerScript.Shuffle(ref cards);
                    }
                    else
                    {
                        exceptions.Add(new Exceptions("Error ,falta el corchete de cierre de la declaración de barajear",0,0));
                    }
                }
                else
                {
                    exceptions.Add(new Exceptions("Error ,falta el corchete de apertura de la declaración de barajear", 0, 0));
                }
                break;
            case "Find":
                List<Card> CardsAux = new List<Card>();
                foreach (Card cartica in cards) 
                {
                    if (tokens[i + 5].Value == "(")
                    {
                        if (tokens[i + 6].Value == "(")
                        {
                            if (tokens[i + 7].Value == "card")
                            {
                                if (tokens[i + 8].Value == ")")
                                {
                                    if (tokens[i + 9].Value == "=")
                                    {
                                        if (tokens[i + 10].Value == ">")
                                        {
                                            if (tokens[i + 11].Value == "card") 
                                            {
                                                if (tokens[i + 12].Value == ".") { 
                                                    bool aux = Find(cartica, tokens,i + 13);
                                                    if (aux)
                                                    {
                                                        CardsAux.Add(cartica);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return CardsAux;
        }
        return 0;
    }


    //Lista de acciones que se van a poder hacer sobre el Find(Predicate) , tengo que arreglarlo de modo que en vez de leer un token único lea acciones
    public bool Find(Card card, List<Token> tokens,int position)
    {
        if (tokens[position + 1].Value == "=")
        {
            if (tokens[position + 2].Value == "=")
            {
                switch (tokens[position].Value)
                {
                    case "Power":
                        if (tokens[position + 3].Key == "NUMERO")
                        {
                            if (card.Power == double.Parse(tokens[position + 3].Value))
                            {
                                return true;
                            }
                        }
                        return false;
                    case "Type":
                        if (tokens[position + 3].Key == "STRING")
                        {
                            if (card.Type == tokens[position + 3].Value || card.SpecialType == tokens[position + 3].Value)
                            {
                                return true;
                            }
                        }
                        return false;
                    case "Faction":
                        if (tokens[position + 3].Key == "STRING")
                        {
                            if (card.Faction == tokens[position + 3].Value)
                            {
                                return true;
                            }
                        }
                        return false;
                    case "Name":
                        if (tokens[position + 3].Key == "STRING")
                        {
                            if (card.Name == tokens[position + 3].Value)
                            {
                                return true;
                            }
                        }
                        return false;
                    case "Range":
                        if (tokens[position + 3].Value == "STRING")
                        {
                            if (card.Range.Contains(tokens[position + 3].Value))
                            {
                                return true;
                            }
                        }
                        return false;
                    default : return false;
                }
            }
        }
        return false;
    }
    //método para retornar la posición del jugador que contiene la carta
    public int ReturnPlayerPosition(Card card,int k)
    {
        for (int z = 0; z < SelectDeckScript.players.Count; z++)
        {
            if (card.PlayerAlQuePertenece == SelectDeckScript.players[z].Id)
            {
                k = z;
                break;
            }
        }
        return k;
    }
}