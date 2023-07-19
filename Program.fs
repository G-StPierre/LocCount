open System.IO

let readLines (filePath: string) = seq {
    use sr = new StreamReader (filePath)
    while not sr.EndOfStream do 
        yield sr.ReadLine()
}

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
let rec countLoc (lines: List<string>, count: int) : int =
    match lines with
    | [] -> count
    | h::t ->
        match h with 
        | "" -> countLoc(t, count)
        | _ -> if checkComment (h |> List.ofSeq, '/') then countLoc(t, count) else countLoc(t, (count + 1))

// TODO:
// Add Directory support, scan every file in the directory and return the total loc / loc per file in the directory
[<EntryPoint>]
let main args = 
    let list = seqToList(readLines(args[0]))
    let locCount = countLoc(list, 0)
    printfn "Lines of code: %d" locCount
    locCount