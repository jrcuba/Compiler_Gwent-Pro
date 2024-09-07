using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System.Runtime.InteropServices;

public class SystemMouseMover : MonoBehaviour
{
    public static List<GameObject> cards = new List<GameObject>();
    public float moveSpeed = 5000;
    public static bool FirstMovement = false;
    public static bool SecondMovement = false;
    private bool TercerMovimiento = false;
    private bool CuartoMovimiento = false;
    private bool QuintoMovimiento = false;
    public static bool SextoMovimiento = false;
    public static bool SectimoMovimiento = false;
    public static bool OctavoMovimiento = false;
    public static bool NovenoMovimiento = false;
    public static bool isMoving = false;
    private Vector3 targetPosition = new Vector3(1580, 970, 0);
    public Canvas canvas;

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

    private const int MOUSEEVENTF_LEFTDOWN = 0x02;
    private const int MOUSEEVENTF_LEFTUP = 0x04;

    private void Update()
    {
        if (SummonScript.IsplayinWithIa)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                targetPosition = new Vector3(1580, 970, 0);
                isMoving = false;
            }
            if (isMoving)
            {
                Vector3 currentPosition = Mouse.current.position.ReadValue();
                Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

                // Establecer la nueva posici�n del cursor  
                Mouse.current.WarpCursorPosition(newPosition);
                    // Verificar si el cursor ha alcanzado la posici�n objetivo  
                if (Vector3.Distance(newPosition, targetPosition) < 1f) // Ajustar el valor de tolerancia si es necesario  
                {
                    if (SummonScript.InvoquedCardsPlayer2.Count < 4)
                    {
                        int position = 0;
                        if (!FirstMovement)
                        {
                            cards = new List<GameObject>();
                            targetPosition = new Vector3(180, 970, 0);
                            FirstMovement = true;
                        }
                        else if (!SecondMovement)
                        {
                            position = VerificateCardWithLowerPower();
                            GetPosition(position);
                            cards.RemoveAt(position);
                            SecondMovement = true;
                        }
                        else if (!TercerMovimiento)
                        {
                            ClickMouse((uint)targetPosition.x, (uint)targetPosition.y);
                            TercerMovimiento = true;
                        }
                        else if (!CuartoMovimiento)
                        {
                            position = VerificateCardWithLowerPower();
                            GetPosition(position);
                            CuartoMovimiento = true;
                        }
                        else if (!QuintoMovimiento)
                        {
                            ClickMouse((uint)targetPosition.x, (uint)targetPosition.y);
                            QuintoMovimiento = true;
                            isMoving = false;
                        }
                        else if (SextoMovimiento)
                        {
                            position = VerificateCardWithMorePower();
                            GetPosition(position);
                            SextoMovimiento = false;
                        }
                        else if (SectimoMovimiento)
                        {
                            //antes de dar click tengo que verificar si se puede invocar ,si no se puede tengo que buscar otra carta e invocarla
                            int k = -1;
                            Card card = cards[position].GetComponent<Card>();
                            if (card.PlayerAlQuePertenece == SelectDeckScript.players[0].Id)
                            {
                                k = 0;
                            }
                            else
                            {
                                k = 1;
                            }
                            foreach (var range in card.Range)
                            {
                                switch (range) 
                                {
                                    case "Melee":
                                        if (k == 0)
                                        {
                                            if (SummonScript.CardsOnMeleePlayer1.Count < 2)
                                            {
                                                ClickMouse((uint)targetPosition.x, (uint)targetPosition.y);
                                                SectimoMovimiento = false;
                                                isMoving = false;
                                            }
                                            else
                                            {
                                                //elimino la carta de la lista de cartas 
                                                cards.RemoveAt(position);
                                            }
                                        }
                                        else if (k == 1)
                                        {
                                            if (SummonScript.CardsOnMeleePlayer2.Count < 2)
                                            {
                                                ClickMouse((uint)targetPosition.x, (uint)targetPosition.y);

                                                SectimoMovimiento = false;
                                                isMoving = false;
                                            }
                                            else
                                            {
                                                cards.RemoveAt(position);
                                            }
                                        }
                                        break;
                                    case "Ranged":
                                        if (k == 0)
                                        {
                                            if (SummonScript.CardsOnRangedPlayer1.Count < 2)
                                            {
                                                ClickMouse((uint)targetPosition.x, (uint)targetPosition.y);

                                                SectimoMovimiento = false;
                                                isMoving = false;
                                            }
                                            else
                                            {
                                                //elimino la carta de la lista de cartas 
                                                cards.RemoveAt(position);
                                            }
                                        }
                                        else if (k == 1)
                                        {
                                            if (SummonScript.CardsOnRangedPlayer2.Count < 2)
                                            {
                                                ClickMouse((uint)targetPosition.x, (uint)targetPosition.y);

                                                SectimoMovimiento = false;
                                                isMoving = false;
                                            }
                                            else
                                            {
                                                cards.RemoveAt(position);
                                            }
                                        }
                                        break;
                                    case "Siege":
                                        if (k == 0)
                                        {
                                            if (SummonScript.CardsOnSiegePlayer1.Count < 2)
                                            {
                                                ClickMouse((uint)targetPosition.x, (uint)targetPosition.y);

                                                SectimoMovimiento = false;
                                                isMoving = false;
                                            }
                                            else
                                            {
                                                //elimino la carta de la lista de cartas 
                                                cards.RemoveAt(position);
                                            }
                                        }
                                        else if (k == 1)
                                        {
                                            if (SummonScript.CardsOnSiegePlayer2.Count < 2)
                                            {
                                                ClickMouse((uint)targetPosition.x, (uint)targetPosition.y);

                                                SectimoMovimiento = false;
                                                isMoving = false;
                                            }
                                            else
                                            {
                                                cards.RemoveAt(position);
                                            }
                                        }
                                        break;
                                    default:
                                        //dirijirse a la posici�n del bot�n y pasar
                                        if (!OctavoMovimiento)
                                        {
                                            targetPosition = new Vector3(1800, 1000, 0);
                                            OctavoMovimiento = true;
                                        }
                                        else if (!NovenoMovimiento)
                                        {
                                            ClickMouse((uint)targetPosition.x, (uint)targetPosition.y);
                                            NovenoMovimiento = true;
                                            isMoving = false;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        //dirijirse a la posici�n del bot�n y pasar
                        if (!OctavoMovimiento)
                        {
                            targetPosition = new Vector3(1800, 1000, 0);
                            OctavoMovimiento = true;
                        }
                        else if (!NovenoMovimiento)
                        {
                            ClickMouse((uint)targetPosition.x, (uint)targetPosition.y);
                            NovenoMovimiento = true;
                            isMoving = false;
                        }
                    }
                }
            }
        }
    }
    private void GetPosition(int position )
    {
        RectTransform rectTransform = cards[position].GetComponent<RectTransform>();
        Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, rectTransform.position);
        targetPosition = new Vector3(screenPosition.x, screenPosition.y, 0);
    }
    private int VerificateCardWithMorePower()
    {
        int position = 0;
        Card aux = cards[0].GetComponent<Card>();
        for (int i = 1; i < cards.Count; i++)
        {
            Card card = cards[i].GetComponent<Card>();
            if (card.Power > aux.Power)
            {
                position = i;
                aux = card;
            }
        }
        return position;
    }
    private int VerificateCardWithLowerPower()
    {
        int position = 0;
        Card aux = cards[0].GetComponent<Card>();
        for (int i = 1; i < cards.Count; i++)
        {
            Card card = cards[i].GetComponent<Card>();
            if (card.Power < aux.Power)
            {
                position = i;
                aux = card;
            }
        }
        return position;
    }
    private void ClickMouse(uint x, uint y)
    {
        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
    }
}
