using System.IO.Compression;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
public class OnActivationParser
{
    public ActionParser _actionParser = new ActionParser();
    public List<Token> actionTokens = new List<Token>();
    public List<Token> PostActionTokens = new List<Token>();
    public Effect effect = new Effect();
    public Effect effectPostAction = new Effect();
    public List<List<Card>> Targets = new List<List<Card>>();
    public List<List<Card>> TargetsPostAction = new List<List<Card>>();
    public bool Single = false;
    public bool SinglePostAction = false;
    public object OnActivation(List<Token> tokens, List<Effect> effects, Card card,ref List<Exceptions> exceptions, int i = 0)
    {
        //Effect
        if (i < tokens.Count) { 
            if (tokens[i].Value == "Effect")
            {
                if (tokens[i + 1].Value == ":")
                {
                    if (tokens[i + 2].Key == "STRING")
                    {
                        FindEffect(effects, tokens, i + 2,ref effect);
                        for (int k = i + 3;k < tokens.Count; k++)
                        {
                            if (tokens[k].Value == "Selector")
                            {
                                break;
                            }
                            else if (tokens[k].Key == "PALABRA")
                            {
                                if (tokens[k + 1].Value == ":")
                                {
                                    foreach (var param in effect.Params)
                                    {
                                        if (param.Name == tokens[k].Value)
                                        {
                                            param.Value = tokens[k + 2].Value;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (tokens[1 + 2].Value == "{")
                    {
                        for (int k = i + 3;k < tokens.Count; k++) 
                        {
                            if (tokens[k].Value == "Name")
                            {
                                if (tokens[k + 1].Value == ":")
                                {
                                    if (tokens[k + 2].Key == "PALABRA")
                                    {
                                        FindEffect(effects,tokens,k + 2,ref effect);
                                    }
                                }
                            }
                            //buscar en la lista de parámetros del efecto que debe haber sido previamente cargado
                            else if (tokens[k].Key == "PALABRA") 
                            {
                                foreach (Params @params in effect.Params)
                                {
                                    if (@params.Name == tokens[k].Value)
                                    {
                                        //realizar la acción
                                        if (tokens[k + 1].Value == ":")
                                        {
                                            //TODO: tengo que verificar el tokens[k + 2].value
                                            @params.Value = tokens[k + 2].Value;
                                        }
                                    }
                                }
                            }
                            else if (tokens[k].Value == "}")
                            {
                                //TODO : tengo que ver que pasa aquí cuando retorno y mejorar esto para que no repita casos
                                break;
                            }
                        }
                    }
                }
            }
            //Selector
            else if (tokens[i].Value == "Selector")
            {
                
                int v = -1;
                v = _actionParser.ReturnPlayerPosition(card,v);
                if (tokens[i + 1].Value == ":")
                {
                    if (tokens[i + 2].Value == "{")
                    {
                        for (int k = i + 3; k < tokens.Count; k++)
                        {
                            //Source
                            if (tokens[k].Value == "Source")
                            {
                                if (v != -1) { 
                                    if (tokens[k + 1].Value == ":")
                                    {
                                        if (tokens[k + 2].Key == "STRING")
                                        {
                                            AddToTargetList(tokens,k + 2,v,ref Targets);
                                        }
                                    }
                                }
                            }
                            //Single
                            else if (tokens[k].Value == "Single")
                            {
                                if (tokens[k + 1].Value == ":")
                                {
                                    if (tokens[k + 2].Value == "true")
                                    {
                                        Single = true;
                                        SinglePostAction = true;
                                    }
                                }
                            }

                            //Predicate
                            else if (tokens[k].Value == "Predicate")
                            {
                                if (tokens[k + 1].Value == ":")
                                {
                                    if (tokens[k + 2].Value == "(")
                                    {
                                        if (tokens[k + 3].Value == "unit")
                                        {
                                            if (tokens[k + 4].Value == ")")
                                            {
                                                if (tokens[k + 5].Value == "=")
                                                {
                                                    if (tokens[k + 6].Value == ">")
                                                    {
                                                        foreach (List<Card> cards in Targets)
                                                        {
                                                            for (int r = cards.Count - 1; r >= 0; r--)
                                                            {
                                                                bool aux = PredicateCompiler(tokens, k + 7, ref exceptions, cards[r]);
                                                                if (!aux)
                                                                {
                                                                    cards.RemoveAt(r); // Elimina el elemento en la posición r  
                                                                }
                                                            }
                                                        }
                                                        TargetsPostAction = Targets;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (tokens[k].Value == "}")
                            {
                                break;
                            }
                        }
                    }
                }
            }
            //PostAction
            else if (tokens[i].Value == "PostAction")
            {
                if (tokens[i + 1].Value == ":")
                {
                    if (tokens[i + 2].Value == "{")
                    {
                        for (int k = i + 3; k < tokens.Count; k++)
                        {
                            //Nombre del efecto
                            if (tokens[k].Value == "Type")
                            {
                                if (tokens[k + 1].Value == ":")
                                {
                                    if (tokens[k + 2].Key == "STRING")
                                    {
                                        foreach (Effect effect in effects)
                                        {
                                            if (effect.Name == tokens[k + 2].Value)
                                            {
                                                effectPostAction = effect;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            //Selector 
                            else if (tokens[k].Value == "Selector")
                            {
                                int v = -1;
                                v = _actionParser.ReturnPlayerPosition(card,v);
                                if (tokens[k + 1].Value == ":")
                                {
                                    if (tokens[k + 2].Value == "{")
                                    {
                                        for (int j = k + 3; j < tokens.Count; j++)
                                        {
                                            if (tokens[j].Value == "Source")
                                            {
                                                if (v == 0 || v == 1) { 
                                                    if (tokens[j + 1].Value == ":")
                                                    {
                                                        if (tokens[j + 2].Key == "STRING")
                                                        {
                                                            AddToTargetList(tokens,j + 2,v, ref TargetsPostAction);
                                                            j = j + 3;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (tokens[j].Value == "Single")
                                            {
                                                if (tokens[j + 1].Value == ":")
                                                {
                                                    if (tokens[j + 2].Value == "true")
                                                    {
                                                        SinglePostAction = true;
                                                    }
                                                    else if (tokens[j + 2].Value == "false")
                                                    {
                                                        SinglePostAction = false;
                                                    }
                                                }
                                            }
                                            //TODO: tengo que hacer el predicado de PostAction también 
                                            else if (tokens[j].Value == "Predicate")
                                            {
                                                
                                                if (tokens[j + 1].Value == ":")
                                                {
                                                    if (tokens[j + 2].Value == "(")
                                                    {
                                                        if (tokens[j + 3].Value == "unit")
                                                        {
                                                            if (tokens[j + 4].Value == ")")
                                                            {
                                                                if (tokens[j + 5].Value == "=")
                                                                {
                                                                    if (tokens[j + 6].Value == ">")
                                                                    {
                                                                        foreach (List<Card> cards in TargetsPostAction)
                                                                        {
                                                                            for (int r = cards.Count - 1; r >= 0; r--)
                                                                            {
                                                                                bool aux = PredicateCompiler(tokens, j + 7, ref exceptions, cards[r]);
                                                                                if (!aux)
                                                                                {
                                                                                    
                                                                                    cards.RemoveAt(r); // Elimina el elemento en la posición r  
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
                                            if (tokens[j].Value == "}")
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            //TODO : me falta el Predicate
                            if (tokens[k].Value == "}")
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        if (i < tokens.Count - 2)
        {
            return OnActivation(tokens, effects, card,ref exceptions, i + 1);
        }
        else
        {
            return 0;
        }
    }

    //método para buscar efecto en la lista de efectos

    public void FindEffect(List<Effect> effects,List<Token> tokens,int position,ref Effect efecto)
    {
        foreach (Effect effect in effects)
        {
            if (effect.Name == tokens[position].Value)
            {
                actionTokens = effect.tokens;
                efecto = effect;
                break;
            }
        }
    }

    //método para agregar a la lista de targets
    public void AddToTargetList(List<Token> tokens,int position,int v,ref List<List<Card>> targets)
    {
        switch (tokens[position].Value)
        {
            //Todo ,luego esto reflejarlo en las cosas visuales
            case ("'board'"):
                targets.Add(SummonScript.InvoquedCards);
                break;
            case "'hand'":
                targets.Add(SelectDeckScript.players[v].Hand);
                break;
            case "'otherHand'":
                targets.Add(SelectDeckScript.players[(v + 3) % 2].Hand);
                break;
            case "'deck'":
                targets.Add(SelectDeckScript.players[v].DeckOfPlayer);
                break;
            case "'otherDeck'":
                targets.Add(SelectDeckScript.players[(v + 3) % 2].DeckOfPlayer);
                break;
            case "'field'":
                if (v == 0)
                {
                    targets.Add(SummonScript.InvoquedCardsPlayer1);
                }
                else
                {
                    targets.Add(SummonScript.InvoquedCardsPlayer2);
                }
                break;
            case "'otherField'":
                if (v == 0)
                {
                    targets.Add(SummonScript.InvoquedCardsPlayer2);
                }
                else
                {
                    targets.Add(SummonScript.InvoquedCardsPlayer1);
                }
                break;
            case "'Parent'":
                targets = Targets;
                break;
            default:
                //TODO : lanzar excepción
                break;
        }
    }

    //Minicompilador de predicado

    public bool PredicateCompiler(List<Token>tokens,int Position,ref List<Exceptions> exceptions, Card Target)
    {
        if (tokens[Position].Value == "unit")
        {
            if (tokens[Position + 1].Value == ".")
            {
                switch(tokens[Position + 2].Value)
                {
                    case "Faction":
                        return PredicateCompilerAction(tokens,Position + 3,ref exceptions, Target.Faction,Target);
                    case "Power":
                        return PredicateCompilerAction(tokens, Position + 3, ref exceptions, Target.Power.ToString(),Target);
                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public bool PredicateCompilerAction(List<Token> tokens,int Position,ref List<Exceptions> exceptions,string Propiedades,Card card)
    {
        switch(tokens[Position].Value)
        {
            //TODO : me falta hacer la concatenación y el manejo de errores
            case "=":
                if (tokens[Position + 1].Value == "=")
                {
                    if (tokens[Position + 2].Key == "STRING")
                    {
                        if (Propiedades == tokens[Position + 2].Value)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            case "<":
                if (tokens[Position - 1].Value == "Power")
                {
                    if (tokens[Position + 1].Key == "NUMERO")
                    {
                        if (card.Power < int.Parse(tokens[Position + 1].Value))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            default:
                return false;
        }
    }
}