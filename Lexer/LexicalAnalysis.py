import sys
import re

KEYWORDS = {"System","out","println","main","def","static","void","if","elif","else", "while", "for", "class", "return", "try", "except", "True", "False", "None", "import",
            "break","scanner","public","private","or","not","and", "SELECT", "TABLE", "INSERT", "CREATE", "FROM", "WHERE", "GROUP BY", "HAVING", "ORDER BY", "JOIN", "UPDATE", 
            "DELETE", "print","input","in","range", "PRIMARY", "SECONDARY", "KEY", "REFERENCES", "FOREIGN", "INTO", "VALUES" }
OPERATORS = {"+", "-", "*", "/", "%", "**", "//", "#","<<", ">>", "&", "&&", "|", "^", "~", "<", "<=", ">", ">=", "!=", "=="}
DATA = {"int", "double", "float", "long", "short", "byte", "boolean", "char", "String", "Object", "complex", "bool", "str", "bytes", "bytearray", 
        "list", "tuple", "set", "frozenset", "dict", "NoneType", "INT", "BIGINT", "SMALLINT", "DECIMAL", "NUMERIC", "FLOAT", "REAL", "CHAR", "VARCHAR", 
        "TEXT", "DATE", "TIME", "DATETIME", "BOOLEAN", "BLOB"}


def lex(chars_iter):
    chars = PeekableStream(chars_iter)
    result = ""
    while chars.next is not None:
        c = chars.move_next()
        if c == " ":
            result += " "  
        elif c == "\n":
            result += "\n" 
        elif c == "\t":
            result += "\t" 
        elif c == "#":
            while c != "\n" and c is not None:
                c = chars.move_next()
        elif c in "(){}[],;=:": 
            result += f"<span class='special'>{c}</span>"
        elif re.match("[+\-*/%&|^~<>=!]", c):  # Match potential start of an operator
            operator = _scan(c, chars, "[+\-*/%&|^~<>=!]")
            if operator in OPERATORS:
                result += f"<span class='operation'>{operator}</span>"
            else:
                raise Exception(f"Unexpected operator: '{operator}'")
        elif c in ("'", '"'): 
            result += f"<span class='string'>{_scan_string(c, chars)}</span>"
        elif re.match("[.0-9]", c): 
            result += f"<span class='number'>{_scan(c, chars, '[.0-9]')}</span>"
        elif re.match("[_a-zA-Z]", c):
            symbol = _scan(c, chars, "[_a-zA-Z0-9]")
            if symbol in KEYWORDS:
                result += f"<span class='keyword'>{symbol}</span>"
            elif symbol in DATA:
                result += f"<span class='data'>{symbol}</span>"
            else:
                result += f"<span class='words'>{symbol}</span>"
        elif c == "\t": 
            raise Exception("Tabs are not allowed in Cell.")
        else: 
            raise Exception(f"Unexpected character: '{c}'.")
    return result

class PeekableStream:
    def __init__(self, iterator):
        self.iterator = iter(iterator)
        self._fill()
    def _fill(self):
        try:
            self.next = next(self.iterator)
        except StopIteration:
            self.next = None
    def move_next(self):
        ret = self.next
        self._fill()
        return ret 

def _scan_string(delim, chars):
    ret = ""
    while chars.next != delim:
        c = chars.move_next()
        if c is None:
            raise Exception("A string ran off the end of the program.")
        ret += c
    chars.move_next()  # Skip the closing delimiter
    return ret

def _scan(first_char, chars, allowed):
    ret = first_char
    p = chars.next
    while p is not None and re.match(allowed, p):
        ret += chars.move_next()
        p = chars.next
    return ret

		