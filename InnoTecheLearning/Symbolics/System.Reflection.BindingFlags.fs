namespace System.Reflection
    open System

    [<Flags>]
    type BindingFlags = 
        | CreateInstance = 512
        | DeclaredOnly = 2
        | Default = 0
        | ExactBinding = 65536
        | FlattenHierarchy = 64
        | GetField = 1024
        | GetProperty = 4096
        | IgnoreCase = 1
        | IgnoreReturn = 16777216
        | Instance = 4
        | InvokeMethod = 256
        | NonPublic = 32
        | OptionalParamBinding = 262144
        | Public = 16
        | PutDispProperty = 16384
        | PutRefDispProperty = 32768
        | SetField = 2048
        | SetProperty = 8192
        | Static = 8
        | SuppressChangeType = 131072