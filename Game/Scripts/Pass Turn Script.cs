using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PassTurnScript : MonoBehaviour
{
    public static int RoundPassCount = 0;
    public GameObject CardPrefab;
    public Transform HandPlayer1;
    public Transform HandPlayer2;
    SummonScript summonScript = new SummonScript();
    public void PassTurn()
    {
        List<Card> auxlist = new List<Card> ();
        if (SummonScript.InvocatedBool)
        {
            if (SummonScript.RoundCount != 4)
            {
                if (SummonScript.count % 2 == 0)
                {
                    if (RoundPassCount == 0)
                    {
                        RoundPassCount++;
                        SummonScript.count++;
                        EffectsNoCompilables.PutIncreaseBool = false;
                        if (SummonScript.IsplayinWithIa)
                        {
                            SummonScript.IsPlayingWithIaMethod();
                        }
                    }
                    else if (RoundPassCount == 1)
                    {
                        if (SummonScript.PowerPointsPlayer1 > SummonScript.PowerPointsPlayer2)
                        {
                            SummonScript.count = 4;
                            SummonScript.RoundsWinsPlayer1++;
                        }
                        else if (SummonScript.PowerPointsPlayer1 < SummonScript.PowerPointsPlayer2)
                        {
                            SummonScript.count = 5;
                            SummonScript.RoundsWinsPlayer2++;

                            if (SummonScript.IsplayinWithIa)
                            {
                                SummonScript.IsPlayingWithIaMethod();
                            }
                        }
                        else
                        {
                            SummonScript.RoundsWinsPlayer1++;
                            SummonScript.RoundsWinsPlayer2++;
                        }
                        // Eliminar las instancias que no estén en la posición inicial
                        foreach (GameObject aux in SummonScript.CardsOnMeleePlayer1Object)
                        {
                            SummonScript.CementeryPlayer1Object.Add(aux);
                            Destroy(aux);
                        }
                        foreach (GameObject aux in SummonScript.CardsOnRangedPlayer1Object)
                        {
                            SummonScript.CementeryPlayer1Object.Add(aux);
                            Destroy(aux);
                        }
                        foreach (GameObject aux in SummonScript.CardsOnSiegePlayer1Object)
                        {
                            SummonScript.CementeryPlayer1Object.Add(aux);
                            Destroy(aux);
                        }
                        foreach (GameObject aux in SummonScript.CardsOnMeleePlayer2Object)
                        {
                            SummonScript.CementeryPlayer2Object.Add(aux);
                            Destroy(aux);
                        }
                        foreach (GameObject aux in SummonScript.CardsOnRangedPlayer2Object)
                        {
                            SummonScript.CementeryPlayer2Object.Add(aux);
                            Destroy(aux);
                        }
                        foreach (GameObject aux in SummonScript.CardsOnSiegePlayer2Object)
                        {
                            SummonScript.CementeryPlayer2Object.Add(aux);
                            Destroy(aux);
                        }

                        EffectsNoCompilables.PutIncreaseBool = false;

                        SummonScript.RoundCount++;
                        SummonScript.count++;
                        RoundPassCount = 0;
                        SummonScript.PowerPointsPlayer1 = 0;
                        SummonScript.PowerPointsPlayer2 = 0;

                        SummonScript.CementeryPlayer1 = SummonScript.InvoquedCardsPlayer1;
                        SummonScript.CementeryPlayer2 = SummonScript.InvoquedCardsPlayer2;
                        SummonScript.InvoquedCardsPlayer1 = new List<Card>();
                        SummonScript.InvoquedCardsPlayer2 = new List<Card>();
                        SummonScript.CardsOnMeleePlayer1 = new List<Card>();
                        SummonScript.CardsOnMeleePlayer2 = new List<Card>();
                        SummonScript.CardsOnRangedPlayer1 = new List<Card>();
                        SummonScript.CardsOnRangedPlayer2 = new List<Card>();
                        SummonScript.CardsOnSiegePlayer1 = new List<Card>();
                        SummonScript.CardsOnSiegePlayer2 = new List<Card>();
                        Debug.Log(SummonScript.CardsOnMeleePlayer1.Count);
                        if (SummonScript.CantCardsInHandPlayer1 <= 9)
                        {
                            if (SummonScript.CantCardsInHandPlayer1 == 9)
                            {
                                summonScript.DrawCard(SelectDeckScript.players[0].DeckOfPlayer[SelectDeckScript.players[0].DeckOfPlayer.Count - 1], HandPlayer1, SelectDeckScript.players[0], CardPrefab,0);
                                SummonScript.CantCardsInHandPlayer1++;
                            }
                            else
                            {
                                SummonScript.CantCardsInHandPlayer1 += 2;
                                summonScript.DrawCard(SelectDeckScript.players[0].DeckOfPlayer[SelectDeckScript.players[0].DeckOfPlayer.Count - 1], HandPlayer1, SelectDeckScript.players[0], CardPrefab, 0);
                                summonScript.DrawCard(SelectDeckScript.players[0].DeckOfPlayer[SelectDeckScript.players[0].DeckOfPlayer.Count - 1], HandPlayer1, SelectDeckScript.players[0], CardPrefab, 0);
                            }
                        }
                        if (SummonScript.CantCardsInHandPlayer2 <= 9)
                        {
                            if (SummonScript.CantCardsInHandPlayer2 == 9)
                            {
                                SummonScript.CantCardsInHandPlayer2++;
                                summonScript.DrawCard(SelectDeckScript.players[1].DeckOfPlayer[SelectDeckScript.players[1].DeckOfPlayer.Count - 1], HandPlayer2, SelectDeckScript.players[1], CardPrefab,1);
                            }
                            else
                            {
                                SummonScript.CantCardsInHandPlayer2 += 2;
                                summonScript.DrawCard(SelectDeckScript.players[1].DeckOfPlayer[SelectDeckScript.players[1].DeckOfPlayer.Count - 1], HandPlayer2, SelectDeckScript.players[1], CardPrefab,1);
                                summonScript.DrawCard(SelectDeckScript.players[1].DeckOfPlayer[SelectDeckScript.players[1].DeckOfPlayer.Count - 1], HandPlayer2, SelectDeckScript.players[1], CardPrefab, 1);
                            }
                        }
                    }
                }
                else if (SummonScript.count % 2 == 1)
                {
                    if (RoundPassCount == 0)
                    {
                        EffectsNoCompilables.PutIncreaseBool = false;
                        RoundPassCount++;
                        SummonScript.count++;
                    }
                    else if (RoundPassCount == 1)
                    {
                        if (SummonScript.PowerPointsPlayer1 > SummonScript.PowerPointsPlayer2)
                        {
                            SummonScript.count = 4;
                            SummonScript.RoundsWinsPlayer1++;
                        }
                        else if (SummonScript.PowerPointsPlayer1 < SummonScript.PowerPointsPlayer2)
                        {
                            SummonScript.count = 5;
                            SummonScript.RoundsWinsPlayer2++;
                            if (SummonScript.IsplayinWithIa)
                            {
                                SummonScript.IsPlayingWithIaMethod();
                            }
                        }
                        else
                        {
                            SummonScript.RoundsWinsPlayer1++;
                            SummonScript.RoundsWinsPlayer2++;
                        }
                        foreach (GameObject aux in SummonScript.InvoquedCardsObjects)
                        {
                            Destroy(aux);
                        }
                        // Eliminar las instancias que no estén en la posición inicial
                        foreach (GameObject aux in SummonScript.CardsOnMeleePlayer1Object)
                        {
                            SummonScript.CementeryPlayer1Object.Add(aux);
                            Destroy(aux);
                        }
                        foreach (GameObject aux in SummonScript.CardsOnRangedPlayer1Object)
                        {
                            SummonScript.CementeryPlayer1Object.Add(aux);
                            Destroy(aux);
                        }
                        foreach (GameObject aux in SummonScript.CardsOnSiegePlayer1Object)
                        {
                            SummonScript.CementeryPlayer1Object.Add(aux);
                            Destroy(aux);
                        }
                        foreach (GameObject aux in SummonScript.CardsOnMeleePlayer2Object)
                        {
                            SummonScript.CementeryPlayer2Object.Add(aux);
                            Destroy(aux);
                        }
                        foreach (GameObject aux in SummonScript.CardsOnRangedPlayer2Object)
                        {
                            SummonScript.CementeryPlayer2Object.Add(aux);
                            Destroy(aux);
                        }
                        foreach (GameObject aux in SummonScript.CardsOnSiegePlayer2Object)
                        {
                            SummonScript.CementeryPlayer2Object.Add(aux);
                            Destroy(aux);
                        }

                        EffectsNoCompilables.PutIncreaseBool = false;

                        SummonScript.RoundCount++;
                        RoundPassCount = 0;
                        SummonScript.PowerPointsPlayer1 = 0;
                        SummonScript.PowerPointsPlayer2 = 0;

                        SummonScript.CementeryPlayer1 = SummonScript.InvoquedCardsPlayer1;
                        SummonScript.CementeryPlayer2 = SummonScript.InvoquedCardsPlayer2;
                        SummonScript.InvoquedCardsPlayer1 = new List<Card>();
                        SummonScript.InvoquedCardsPlayer2 = new List<Card>();
                        SummonScript.CardsOnMeleePlayer1 = new List<Card>();
                        SummonScript.CardsOnMeleePlayer2 = new List<Card>();
                        SummonScript.CardsOnRangedPlayer1 = new List<Card>();
                        SummonScript.CardsOnRangedPlayer2 = new List<Card>();
                        SummonScript.CardsOnSiegePlayer1 = new List<Card>();
                        SummonScript.CardsOnSiegePlayer2 = new List<Card>();

                        if (SummonScript.CantCardsInHandPlayer1 <= 9)
                        {
                            if (SummonScript.CantCardsInHandPlayer1 == 9)
                            {
                                summonScript.DrawCard(SelectDeckScript.players[0].DeckOfPlayer[SelectDeckScript.players[0].DeckOfPlayer.Count - 1], HandPlayer1, SelectDeckScript.players[0], CardPrefab, 0);
                                SummonScript.CantCardsInHandPlayer1++;
                            }
                            else
                            {
                                SummonScript.CantCardsInHandPlayer1 += 2;
                                summonScript.DrawCard(SelectDeckScript.players[0].DeckOfPlayer[SelectDeckScript.players[0].DeckOfPlayer.Count - 1], HandPlayer1, SelectDeckScript.players[0], CardPrefab, 0);
                                summonScript.DrawCard(SelectDeckScript.players[0].DeckOfPlayer[SelectDeckScript.players[0].DeckOfPlayer.Count - 1], HandPlayer1, SelectDeckScript.players[0], CardPrefab, 0);
                            }
                        }
                        if (SummonScript.CantCardsInHandPlayer2 <= 9)
                        {
                            if (SummonScript.CantCardsInHandPlayer2 == 9)
                            {
                                SummonScript.CantCardsInHandPlayer2++;
                                summonScript.DrawCard(SelectDeckScript.players[1].DeckOfPlayer[SelectDeckScript.players[1].DeckOfPlayer.Count - 1], HandPlayer2, SelectDeckScript.players[1], CardPrefab, 1);
                            }
                            else
                            {
                                SummonScript.CantCardsInHandPlayer2 += 2;
                                summonScript.DrawCard(SelectDeckScript.players[1].DeckOfPlayer[SelectDeckScript.players[1].DeckOfPlayer.Count - 1], HandPlayer2, SelectDeckScript.players[1], CardPrefab, 1);
                                summonScript.DrawCard(SelectDeckScript.players[1].DeckOfPlayer[SelectDeckScript.players[1].DeckOfPlayer.Count - 1], HandPlayer2, SelectDeckScript.players[1], CardPrefab, 1);
                            }
                        }
                    }
                }
            }
            else
            {
                if (SummonScript.RoundsWinsPlayer1 > SummonScript.RoundsWinsPlayer2)
                {
                    Debug.Log("El juego ha terminado, gana player 1");
                }
                else if (SummonScript.RoundsWinsPlayer1 < SummonScript.RoundsWinsPlayer2)
                {
                    Debug.Log("El juego ha terminado, gana player 2");
                }
                //devolver todas las cartas robadas al deck
            }
        }
    }
}