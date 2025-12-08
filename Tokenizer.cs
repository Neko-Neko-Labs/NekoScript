using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NSToken {
    public class Tokenizer {
        private static readonly HashSet<string> keywords = new HashSet<string> {
            "if", "elif", "else", "fi", "let", "increment", "do"
        };

        private static readonly HashSet<string> symbols = new HashSet<string> {
            "(", ")", "{", "}", "[", "]", "!", "=", "==", ">", "<", "<=", ">=", "?", "-", "+", "/", "*", "!="
        };


        public static List<Token> Tokenize(IEnumerable<string> lines) {
            List<Token> tokens = new List<Token>();
            foreach (var line in lines) {
                //Trim string to remove extra spaces;
                string trimmed = Regex.Replace(line.Trim(), @"\s+", " ");
                if (String.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(NS.Commentfix)) continue;
                if (!NS.DoComments) {
                    string ptr = Regex.Escape(NS.Commentfix) + ".*";
                    trimmed = Regex.Replace(trimmed, ptr, "");
                }

                string[] parts = trimmed.Split(" ");

                bool IsTextMode = true;
                bool IsInComment = false;
                foreach (var word in parts) {
                    string tmp = word;
                    TokenType type = FindKind(tmp, IsTextMode, IsInComment);
                    if (type == TokenType.FUNC) {
                        tmp = tmp.Substring(NS.Prefix.Length);
                        IsTextMode = false;
                    } else if (type == TokenType.KEYWORD) {
                        IsTextMode = false;
                    } else if (type == TokenType.VAR) {
                        tmp = tmp.Substring(NS.VarFix.Length);
                    } else if (type == TokenType.COMMENT) {
                        IsInComment = true;
                    }
                    tokens.Add(new Token(type, tmp));
                }

                tokens.Add(new Token(TokenType.LINEEND, "br"));
            }
            return tokens;
        }

        public static TokenType FindKind(string part, bool TextMode, bool CommentMode) {
            if (part.StartsWith(NS.Prefix)) return TokenType.FUNC;
            else if (part.StartsWith(NS.Labfix)) return TokenType.LABEL;
            else if (keywords.Contains(part)) return TokenType.KEYWORD;
            else if (part.StartsWith(NS.VarFix)) return TokenType.VAR;
            else if (part.StartsWith(NS.Commentfix) || CommentMode) return TokenType.COMMENT;
            else {
                if (TextMode) return TokenType.TEXT;
                else if (symbols.Contains(part)) return TokenType.SYMBOL;
                else return TokenType.IDENTIFIER;
            }

        }

        public static TokenType FindKindLine(string[] parts) {
            if (parts[0].StartsWith(NS.Prefix)) return TokenType.FUNC;
            else if (parts[0].StartsWith(NS.Labfix)) return TokenType.LABEL;
            else if (keywords.Contains(parts[0])) return TokenType.KEYWORD;
            else return TokenType.TEXT;
        }

    }

    public class Token {
        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType type, string val) {
            this.Type = type;
            this.Value = val;
        }

        public override string ToString() {
            return $"{this.Type}({this.Value})";
        }
    }

    public enum TokenType {
        FUNC, IDENTIFIER, SYMBOL, KEYWORD, TEXT, LABEL, UNKNOWN, VAR, LINEEND, COMMENT
    }
}

