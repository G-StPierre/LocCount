open System.IO

let readDirectory (dirPath: String) = "test"

// https://stackoverflow.com/questions/2365527/how-read-a-file-into-a-seq-of-lines-in-f
let readLines (filePath: string) = seq {
    use sr = new StreamReader (filePath)
    while not sr.EndOfStream do 
        yield sr.ReadLine()
}
// let main = let sequence = readLines("./Program.fs") in test(sequence)

let seqToList(sequence: seq<'a>) = sequence |> Seq.toList

let matchComment(language: string) : char = 
    match language with
    | "py" | "python" | "#" -> '#'
    | "c" | "/" -> '/'
    | "lisp" | ";" -> ';' // I'm actually not sure if lisp is the one with different comment types
    | _ -> 'l' // This should throw an error, this is just a placeholder for now
    // Maybe look into bash??

let matchExtension(extension: string) : char = 
    match extension with
    | ".py" | "rb" -> '#' // Multi line comments are different in ruby!! Python uses ''' '''
    | ".c" | ".cpp" | ".java" | ".cs" | ".fs" | ".js" | ".ts" -> '/' // Evidently we want to support more than just these
    | _ -> 'L' // Here we don't have support for that language, Ideally we ask the user to input the comment char?


let getExtension(file: string) : string = 
    let fileList = file |> List.ofSeq in 
        let rec findExtension (splitFile : char list) : char list = // I need to look into stripping all directories, maybe split on / and take last element of array??
            match splitFile with
            | '.'::t -> '.'::t // This is failing because we use ./ in the directory :( Otherwise it works as expected
            | h::t -> findExtension t
            | [] -> [] 
        in (findExtension fileList) |> List.toArray |> System.String;

// Looks like stripString is working :>
// Maybe I should be passing the comment symbol around, like have # as an input and check if lang is python
let rec stripString (line: List<char>) : List<char> =
    match line with
    | [] -> [];
    | h::t -> 
        match h with
        |'/' -> h::t
        | _ -> stripString t;


// I will need to take in multiple lines and consume everything between the comments!
// let rec consumeComment = 
// Here I think we want to match the end characters, so for c style we match '*/' to '*/' and for some like python, we just match ''' to '''

// This doesn't check for multiline, maybe I can write a function to consume until the end of the comment?
// I don't think this is working if there is a leading white space!
let checkComment (line: List<char>, commentChar: char) : bool =
    let line = stripString line in  
    match line with 
    | commentChar::_ -> true // I can't see a reason to only use one / that isn't a comment?
    | _ -> false



// Checking for these types of comments will not work for certain languages (think python!)
let rec countLoc (lines: List<string>, count: int) : int =
    match lines with
    | [] -> count
    | h::t ->
        match h with 
        | "" -> countLoc(t, count) // this works but \n does not???
        | _ -> if checkComment (h |> List.ofSeq, '/') then countLoc(t, count) else countLoc(t, (count + 1))
    // | "\n" -> (countLoc sequence.Tail, count) ABOVE IS HARDCODED, WE WANT TO DETERMINE THAT MANUALLY
    // | _ -> (countLoc sequence.Tail, (count + 1))


let main2 = let list = seqToList(readLines("./Program.fs")) in countLoc(list, 0)

// Need this to be a -> String[] -> int instead of a -> unit
let run = printf "%d" main2

run

// let run = printf "%s" (getExtension("Program.fs")) 



// [<EntryPoint>]
// let main args = printfn "args : %A" args; 0;




// I think in main, I just want to check for the directory, and have mapping somewhere else to the comment type 
// [<EntryPoint>]
// let main args : string[] = 
//     let length = args.Length in
//     match length with 
    // | 1 -> 0
    // | 2 -> 0
    // | 3 -> 0
    // | _ -> 5
