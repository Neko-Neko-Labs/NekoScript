public class NS {
    public static string Prefix = "::";
    public static string Labfix = "#";
    public static string Commentfix = "//";

    public static void SetFuncPrefix(string prefix) {
        Prefix = prefix;
    }

    public static void SetLabFix(string prefix) {
        Labfix = prefix;
    }

    public static void SetCommentPrefix(string prefix) {
        Commentfix = prefix;
    }
}
