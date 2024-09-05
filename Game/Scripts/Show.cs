using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Show : MonoBehaviour
{
    public Text text1;
    public Text text2;
    public Text text3;
    public Text text4;
    public Text text5;
    public Text text6;
    public Text text7;
    public Text text8;
    public Text text9;
    public Text text10;
    public Text text11;
    public Text text12;
    public Text text13;
    public Text text14;
    public Image image1;
    public Image image2;
    public void Update()
    {
        text1.text = text2.text;
        text3.text = text4.text;
        text5.text = text6.text;
        text7.text = text8.text;
        text9.text = text10.text;
        text11.text = text12.text;
        text13.text = text14.text;
        image2.sprite = image1.sprite;
    }
}