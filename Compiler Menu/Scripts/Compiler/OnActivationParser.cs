using System.IO.Compression;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System;
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
        

        if (i < tokens.Count) {


            //Lectura directa del efecto dentro del OnActivation
            if (tokens[i].Value == "Effect")
            {
                if (tokens[i + 1].Value == ":")
                {
                    if (tokens[i + 2].Key == "STRING")
                    {

                        //Busca sí está el efecto dentro de la lista de efectos y lo pone como efecto principal si existe
                        VerificateEffectName(tokens, i + 2, effects, ref exceptions,ref effect);

                        //Bucle para buscar los parámetros declarados y si existen darle su valor correspondiente
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
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    exceptions.Add(new Exceptions("Error ,faltan los : en la declaración del parámetro",0,0));
                                }
                            }
                        }
                    }
                    //Lo mismo que arriba pero en un caso diferente ,carga los parámetros y el nombre
                    else if (tokens[1 + 2].Value == "{")
                    {
                        for (int k = i + 3;k < tokens.Count; k++) 
                        {
                            //Busqueda de nombre del efecto
                            if (tokens[k].Value == "Name")
                            {
                                VerificateEffectName(tokens, k, effects, ref exceptions,ref effect);
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
                                            break;
                                        }
                                        else
                                        {
                                            exceptions.Add(new Exceptions("Error ,faltan los : en la declaración del parámetro", 0, 0));
                                        }

                                    }
                                }
                            }
                            else if (tokens[k].Value == "}")
                            {
                                break;
                            }
                        }
                    }
                }
            }
            //Selector
            else if (tokens[i].Value == "Selector")
            {
                //Guardo un contador v que va a ser la posición del jugador 
                int v = -1;
                v = _actionParser.ReturnPlayerPosition(card,v);

                //Verifico que la forma de declaración del Selector es correcta
                if (tokens[i + 1].Value == ":")
                {
                    if (tokens[i + 2].Value == "{")
                    {
                        //Recorro los tokens correspondientes dentro del Selector

                        for (int k = i + 3; k < tokens.Count; k++)
                        {
                            //Source
                            if (tokens[k].Value == "Source")
                            {
                                SourceVerificator(tokens,k,ref exceptions,v);
                            }
                            //Single
                            else if (tokens[k].Value == "Single")
                            {
                                SingleVerificator(tokens,k,ref Single);
                            }

                            //Predicate
                            else if (tokens[k].Value == "Predicate")
                            {
                                VerificatePredicate(tokens,k,ref exceptions,ref Targets);
                            }
                        }
                    }
                    else
                    {
                        exceptions.Add(new Exceptions("Error ,faltan el { en la declaración del Selector de la carta", 0, 0));
                    }
                }
                else
                {
                    exceptions.Add(new Exceptions("Error ,faltan los : en la declaración del parámetro", 0, 0));
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
                                VerificateEffectName(tokens, k, effects, ref exceptions, ref effectPostAction);
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

                                            //Source
                                            if (tokens[j].Value == "Source")
                                            {
                                                SourceVerificator(tokens,j,ref exceptions,v);
                                            }

                                            //Single
                                            else if (tokens[j].Value == "Single")
                                            {
                                                SingleVerificator(tokens, j,ref SinglePostAction);
                                            }


                                            //TODO: tengo que hacer el predicado de PostAction también
                                            else if (tokens[j].Value == "Predicate")
                                            {
                                                VerificatePredicate(tokens, j,ref exceptions,ref TargetsPostAction);
                                            }
                                            if (tokens[j].Value == "}")
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        exceptions.Add(new Exceptions("Error ,falta el { en la declaración del PostAction", 0, 0));
                                    }
                                }
                                else
                                {
                                    exceptions.Add(new Exceptions("Error ,faltan los : en la declaración del PostAction", 0, 0));
                                }
                            }
                            //TODO : me falta el Predicate
                            if (tokens[k].Value == "}")
                            {
                                break;
                            }
                        }

                    }
                    else
                    {
                        exceptions.Add(new Exceptions("Error ,faltan el { en la declaración del PostAction", 0, 0));
                    }
                }
                else
                {
                    exceptions.Add(new Exceptions("Error ,faltan los : en la declaración del PostAction", 0, 0));
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
    //Método para verificar el nombre del efecto
    private void VerificateEffectName(List<Token>tokens,int k,List<Effect> effects,ref List<Exceptions> exceptions,ref Effect efecto)
    {
        if (tokens[k + 1].Value == ":")
        {
            if (tokens[k + 2].Key == "STRING")
            {
                //busqueda del efecto en la lista de efectos
                foreach (Effect effect in effects)
                {
                    if (effect.Name == tokens[k + 2].Value)
                    {
                        efecto = effect;
                        break;
                    }
                }
            }
            else
            {
                exceptions.Add(new Exceptions("Error ,debe ser un String lo que se está manejando en el tipo de carta", 0, 0));
            }
        }
        else
        {
            exceptions.Add(new Exceptions("Error ,faltan los : en la declaración del tipo del PostAction", 0, 0));
        }
    }

    //Método para verificar el Source
    private void SourceVerificator(List<Token> tokens,int j ,ref List<Exceptions> exceptions,int v)
    {
        if (v == 0 || v == 1)
        {
            if (tokens[j + 1].Value == ":")
            {
                if (tokens[j + 2].Key == "STRING")
                {
                    AddToTargetList(tokens, j + 2, v, ref TargetsPostAction);
                    j = j + 3;
                }
                else
                {
                    exceptions.Add(new Exceptions("Error ,La declaración del Source del PostAction debe ser un String", 0, 0));
                }
            }
            else
            {
                exceptions.Add(new Exceptions("Error ,faltan los : en la declaración del PostAction", 0, 0));
            }
        }
        else
        {
            exceptions.Add(new Exceptions("Error ,La posición del Player en el Source del Selector es incorrecta", 0, 0));
        }
    }

    //Método Verificador del Single

    private void SingleVerificator(List<Token> tokens,int j,ref bool BoolToEdit)
    {
        if (tokens[j + 1].Value == ":")
        {
            if (tokens[j + 2].Value == "true")
            {
                BoolToEdit = true;
            }
            else if (tokens[j + 2].Value == "false")
            {
                BoolToEdit = false;
            }
        }
    }
    //Método verificador de predicado
    private void VerificatePredicate(List<Token> tokens,int j,ref List<Exceptions> exceptions,ref List<List<Card>> targets)
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
                                foreach (List<Card> cards in targets)
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
                            else
                            {
                                exceptions.Add(new Exceptions("Error ,faltan el > en la declaración del parámetro", 0, 0));
                            }
                        }
                        else
                        {
                            exceptions.Add(new Exceptions("Error ,faltan el = en la declaración del parámetro", 0, 0));
                        }
                    }
                    else
                    {
                        exceptions.Add(new Exceptions("Error ,faltan el ) en la declaración del parámetro", 0, 0));
                    }
                }
                else
                {
                    exceptions.Add(new Exceptions("Error ,faltan el 'Unit' en la declaración del parámetro", 0, 0));
                }
            }
            else
            {
                exceptions.Add(new Exceptions("Error ,faltan el ( en la declaración del parámetro", 0, 0));
            }
        }
        else
        {
            exceptions.Add(new Exceptions("Error ,faltan los : en la declaración del predicado", 0, 0));
        }
    }

    //método para agregar a la lista de targets
    public void AddToTargetList(List<Token> tokens,int position,int v,ref List<List<Card>> targets)
    {
        //Veo que los tipos de objetivos que puede tener
        switch (tokens[position].Value)
        {
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
                Debug.Log("El objetivo no es válido");
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
                //solo estoy barriendo los casos de facción y poder que son los que salen en los ejemplo ,caso de algún otro lo agrego y listo
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

            //Aquí están las posibles operaciones que se pueden realizar con el predicado ,por ahora tengo igual y menor ,caso de que hayan más simplemente las agrego y listo
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