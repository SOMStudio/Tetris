public static class TextHelp
{
    public static string SpecTextChar(string st) {
        return st.Replace ("<n>", "\n").Replace ("<t>", "\t");
    }
}
