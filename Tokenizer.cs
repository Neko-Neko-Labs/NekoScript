using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NSToken {
    public class Tokenizer {
        private static readonly HashSet<string> keywords = new HashSet<string> {
            "if", "elif", "else", "fi"
        };

        public static List<Token> Tokenize(IEnumerable<string> lines) {
            List<Token> tokens = new List<Token>();
            int linenm = 0;
            foreach (var line in lines) {
                string trimmed = Regex.Replace(line.Trim(), @"\s+", " ");
                if (String.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(NS.Commentfix)) continue;
                string[] parts = trimmed.Split(' ');
                TokenType type = FindKind(parts);
                switch (type) {
                    case TokenType.FUNC:
                        parts[0] = parts[0].Substring(NS.Prefix.Length);
                        tokens.Add(new Token(TokenType.FUNC, parts[0], linenm, parts.Skip(1).ToArray()));
                        break;;
                    case TokenType.LABEL:
                        tokens.Add(new Token(TokenType.LABEL, parts[0], linenm));
                        break;;
                    case TokenType.KEYWORD:
                        if (parts[^1].EndsWith(':')) parts[^1] = parts[^1][..^1];
                        tokens.Add(new Token(TokenType.KEYWORD, parts[0], linenm, parts.Skip(1).ToArray()));
                        break;;
                    case TokenType.TEXT:
                        tokens.Add(new Token(TokenType.TEXT, trimmed, linenm));
                        break;;
                    default:
                        tokens.Add(new Token(TokenType.UNKNOWN, parts[0], linenm, parts.Skip(1).ToArray()));
                        break;;
                }
                ++linenm;
            }
            return tokens;
        }

        public static TokenType FindKind(string[] parts) {
            if (parts[0].StartsWith(NS.Prefix)) return TokenType.FUNC;
            else if (parts[0].StartsWith(NS.Labfix)) return TokenType.LABEL;
            else if (keywords.Contains(parts[0])) return TokenType.KEYWORD;
            else return TokenType.TEXT;
        }

    }

    public class Token {
        public TokenType Type { get; }
        public string Value { get; }
        public string[] Args { get; private set; }
        public int LineNumber { get; }

        public Token(TokenType type, string val, int line, string[] args = null) {
            this.Type = type;
            this.Value = val;
            this.LineNumber = line;
            this.Args = args ?? Array.Empty<string>();
        }

        public override string ToString() {
            return $"{this.Type}({this.Value}) {String.Join(" ", this.Args.Select(x => $"ARG({x})"))}";
        }
    }

    public enum TokenType {
        FUNC, IDENTIFIER, KEYWORD, TEXT, LABEL, VAR, UNKNOWN
    }
}

