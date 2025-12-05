using System;
using System.Collections.Generic;

namespace AJGToken {
    public class Tokenizer {
        private static HashSet<string> keywords = new HashSet<string> {
            "if", "elif", "else", "fi"
        };

        public static List<Token> Tokenize(IEnumerable<string> lines) {
            List<Token> tokens = new List<Token>();
            int linenm = 1;
            foreach (var line in lines) {
                string trimmed = line.TrimStart();
                string[] linear = trimmed.Split(' ');
                if (String.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(AJG.Commentfix)) continue;
                if (trimmed.StartsWith(AJG.Prefix)) {
                    tokens.Add(new Token(TokenType.FUNC, linear[0], linenm, linear.Skip(1).ToArray()));
                } else if (linear[0].StartsWith(AJG.Labfix)) {
                    tokens.Add(new Token(TokenType.LABEL, trimmed, linenm));
                } else if (keywords.Contains(linear[0])) {
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
                return $"{this.LineNumber}: {this.Type} -> {this.Value} {String.Join(" ", this.Args)}";
            else
                return $"{this.LineNumber}: {this.Type} -> {this.Value}";
        }
    }

    public enum TokenType {
        FUNC, IDENTIFIER, KEYWORD, TEXT, LABEL, VAR
    }
}

