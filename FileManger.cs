using System;
using System.IO;
using System.Collections.Generic;
using NSToken;

public class FileManager {
    public static void Main(string[] args) {
        String path = "test.ns";
        if (!File.Exists(path)) {
            Console.WriteLine("Can't accses to file!");
            return;
        }
        NS.SetFuncPrefix("::");
        NS.SetLabFix("#");
        NS.SetCommentPrefix("//");
        NS.SetVarFix("$");
        NS.EnableCommentToken();
        IEnumerable<string> codefrfr = File.ReadLines(path);
        List<Token> tkn = Tokenizer.Tokenize(codefrfr);

        foreach(var tk in tkn) {
            Console.Write(tk.ToString() + " ");
        }
        return;
    }
}
