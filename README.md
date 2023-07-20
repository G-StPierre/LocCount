# LOCCount (Line of Code Counter)

Simple application build using F# and the .NET runtime allowing users to count the amount of lines of code in a file. 

To run the application, run the following command
```dotnetcli
dotnet run [file]
```

For example, to run the application on the Program.fs file found within this applications, we would run
```dotnetcli
dotnet run ./Program.fs
```


To run the application on every file in a directory, run the following

```dotnetcli
dotnet run -D [directory]
```

## Todo
- Allow for full directory scans rather than single files
- Allow for further types of files to be scanned (currently only files using C style comments(//) are supported) 