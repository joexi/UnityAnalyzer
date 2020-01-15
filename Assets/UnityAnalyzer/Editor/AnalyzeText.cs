using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


public class AnalyzeText : AnalyzeObject
{
    public Text Text;
    public Font Font;
    public int FontSize;
    public List<Text> Texts = new List<Text>();
    public override int GetColumn()
    {
        return 4;
    }

    public override string GetNames(int column)
    {
        switch (column)
        {
            case 0:
                return "字体";
                break;
            case 1:
                return "字体大小";
            case 2:
                return "文本数量";
        }
        return string.Empty;
    }

    public override object GetValue(int column)
    {
        switch (column)
        {
            case 0:
                return Font;
            case 1:
                return this.FontSize;
            case 2:
                return this.Texts.Count;
            case 3:
                return Texts;
        }
        return string.Empty;
    }

    public override int GetValueMax(int column)
    {
        //switch (column)
        //{
        //    case 1:
        //        return 30;
        //    case 2:
        //        return 200000;
        //}
        return 100000;
    }

    public override void OnGUI(int column)
    {
        base.OnGUI(column);
    }

    public override void Parse()
    {
        this.Font = this.Text.font;
        this.FontSize = this.Text.fontSize;
    }
}