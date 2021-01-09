using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextHelp
{
    public static string SpecTextChar(string st) {
        return st.Replace ("<n>", "\n").Replace ("<t>", "\t");
    }
}
