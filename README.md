# NekoScript

NekoScript is a lightweight scripting framework/engine for C# game developers. It allows you to define and run custom commands with ease, making your game logic more flexible and modular.

---

## Features

- **Simple initialization**: Set up your script engine with a single call:
```csharp
    NS.Initialize();
```

- Custom prefixes: Choose your own prefixes for functions, labels, and comments. Defaults:
    - Function prefix: ::
    - Label prefix: #
    - Comment prefix: //
- Register C# functions as commands:
```csharp
    public static void MyFunc(string[] argv) {
        // do something
    }

    NS.RegisterCommand("test", MyFunc);
```
Using this, ::test arg1 arg2 in a script will call your C# function with the arguments.

- Multiple parsers included:
1. .sh parser (most feature-rich):
```nsh
// Comment
::Go label START

#START
::Move enemy 3 4

let x = 1
let y = $x

if x < 4 do
    this
    increment x
    ::Wait blah blah 
fi
```

2. .c parser (C-like syntax):
```nsc
// Comment
::Go label START

#START
::Move enemy 3 4

let x be 1
let y be $x

if x < 4 {
    this
    increment x
    ::Wait blah blah 
}

y++
let myvar = $x + $y
Print $myvar
```

3. AJG parser (experimental, details coming soon).

- Parser selection:
```csharp
    NS.SetParser(ParserType.C); // or .SH, .AJG
```
- Run scripts from a directory:
```csharp
    NS.RunDir("path/to/dir");
```
This runs `main.ns` first, and any `::Go file otherfile` commands will jump to the specified file automatically.

---

## Getting Started
1. Add NekoScript to your C# project.
2. Initialize the engine:
```csharp
    NS.Initialize();
```
3. Register your commands.
4. Select your parser.
5. Run your scripts with `NS.RunDir("path/to/dir")`.




