open System.IO

let readLines (filePath: string) : string list =
    let lines =  seq {
        use sr = new StreamReader (filePath)
        while not sr.EndOfStream do 
            yield sr.ReadLine()
        }
    lines |> Seq.toList

let seqToList(sequence: seq<'a>) = sequence |> Seq.toList

// TODO
// Implement additional commment types!
let matchComment(language: string) : char = 
    match language with
    | "py" | "python" | "#" -> '#'
    | "c" | "/" -> '/'
    | "lisp" | ";" -> ';' 
    | _ -> 'l' 
    

// TODO
// Allow the user to input the command char in the CLI
let matchExtension(extension: string) : char = 
    match extension with
    | ".py" | "rb" -> '#' 
    | ".c" | ".cpp" | ".java" | ".cs" | ".fs" | ".js" | ".ts" -> '/' 
    | _ -> 'L'


let getExtension(file: string) : string = 
    let fileList = file |> List.ofSeq in 
        let rec findExtension (splitFile : char list) : char list = 
            match splitFile with
            | '.'::t -> '.'::t 
            | h::t -> findExtension t
            | [] -> [] 
        in (findExtension fileList) |> List.toArray |> System.String;

let rec stripString (line: List<char>) : List<char> =
    match line with
    | [] -> [];
    | h::t -> 
        match h with
        |'/' -> h::t
        | _ -> stripString t;


let checkComment (line: List<char>, commentChar: char) : bool =
    let line = stripString line in  
    match line with 
    | commentChar::_ -> true
    | _ -> false



// Checking for these types of comments will not work for certain languages (think python!)
let rec countLoc (lines: list<string>, count: int) : int =
    match lines with
    | [] -> count
    | h::t ->
        match h with 
        | "" -> countLoc(t, count)
        | "\n" -> countLoc(t, count)
        | _ -> if checkComment (h |> List.ofSeq, '/') then countLoc(t, count) else countLoc(t, (count + 1))

// Check if the user has inputted the -d flag indicating directory support
let checkInputArgs (args: string[]) : bool =
    match args with
    | x when x.[0] = "-d" || x.[0] = "-D" -> true
    | _ -> false

let processFile (path: string) : int = 
    let lines = seqToList(readLines path)
    let loc = countLoc(lines, 0)
    loc


// Parallel array processing docs: https://fsharp.github.io/fsharp-core-docs/reference/fsharp-collections-arraymodule-parallel.html
let parellelFileProcess (directory: string) : int =
    let files = Directory.GetFiles(directory)
    let locCount = files |> Array.Parallel.map processFile 
    let finalCount = Array.sum locCount
    finalCount


// TODO:
// Add Directory support, scan every file in the directory and return the total loc / loc per file in the directory
[<EntryPoint>]
let main args = 
    if (checkInputArgs args) then
        let locCount = parellelFileProcess(args[1])
        printfn "Lines of code: %d" locCount
        locCount
        // printfn "Directory support not yet implemented!"
        // 0
    else
    let list = seqToList(readLines(args[0]))
    let locCount = countLoc(list, 0)
    printfn "Lines of code: %d" locCount
    locCount