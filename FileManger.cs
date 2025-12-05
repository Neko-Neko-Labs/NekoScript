using System;
using System.IO;
using System.Collections.Generic;
using AJGToken;

public class FileManager {
    public static void Main(string[] args) {
        String path = "test.ajg";
        if (!File.Exists(path)) {
            Console.WriteLine("Can't accses to file!");
            return;
        }
        AJG.SetFuncPrefix(">");
        AJG.SetLabFix("#");
        AJG.SetCommentPrefix("!!");
        IEnumerable<string> codefrfr = File.ReadLines(path);
        List<Token> tkn = Tokenizer.Tokenize(codefrfr);

        foreach(var tk in tkn) {
            Console.WriteLine(tk.ToString());
        }
        return;
    }
}
