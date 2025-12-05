using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AJGToken {
    public class Tokenizer {
        private static HashSet<string> keywords = new HashSet<string> {
            "if", "elif", "else", "fi"
        };

        public static List<Token> Tokenize(IEnumerable<string> lines) {
            List<Token> tokens = new List<Token>();
            int linenm = 1;
            foreach (var line in lines) {
                string trimmed = line.TrimStart().TrimEnd();
                trimmed = Regex.Replace(trimmed, @"\s+", " ");
                if (String.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(AJG.Commentfix)) continue;
                string[] linear = trimmed.Split(' ');
                if (trimmed.StartsWith(AJG.Prefix)) {
                    linear[0] = linear[0].Substring(AJG.Prefix.Length);
                    tokens.Add(new Token(TokenType.FUNC, linear[0], linenm, linear.Skip(1).ToArray()));
                } else if (linear[0].StartsWith(AJG.Labfix)) {
                    tokens.Add(new Token(TokenType.LABEL, linear[0], linenm));
                } else if (keywords.Contains(linear[0]) && line.EndsWith(':')) {
                    int len = linear.Length;
                    linear[len-1] = linear[len-1].Substring(0, linear[len-1].Length-1);
                    tokens.Add(new Token(TokenType.KEYWORD, linear[0], linenm, linear.Skip(1).ToArray()));
                } else {
                    tokens.Add(new Token(TokenType.TEXT, trimmed, linenm));
                }
                ++linenm;
            }
            return tokens;
        }

    }

    public class Token {
        public TokenType Type;
        public string Value;
        public string[] Args;
        public int LineNumber;

        public Token(TokenType type, string val, int line, string[] args = null) {
            this.Type = type;
            this.Value = val;
            this.LineNumber = line;
            this.Args = args ?? new string[0];
        }

        public override string ToString() {
            if (this.Type == TokenType.FUNC)
                return $"{this.LineNumber}: {this.Type} -> {this.Value}({String.Join(", ", this.Args)})";
            else if (this.Type == TokenType.KEYWORD)
                return $"{this.LineNumber}: {this.Type} -> {this.Value} ({String.Join(" ", this.Args)})";
            else
                return $"{this.LineNumber}: {this.Type} -> {this.Value}";
        }
    }

    public enum TokenType {
        FUNC, IDENTIFIER, KEYWORD, TEXT, LABEL, VAR
    }
}

