namespace FSharp.Data

///<summary>Enum describing output type</summary>
type ResultType =
///<summary>Sequence of custom records with properties matching column names and types</summary>
    | Records = 0
///<summary>Sequence of tuples matching column types with the same order</summary>
    | Tuples = 1
///<summary>Typed DataTable <see cref='T:FSharp.Data.DataTable`1'/></summary>
    | DataTable = 2
///<summary>raw DataReader</summary>
    | DataReader = 3

type SqlEnumKind = 
| Default = 0
| CLI = 1
| UnitsOfMeasure = 2

[<CompilerMessageAttribute("This API supports the FSharp.Data.SqlClient infrastructure and is not intended to be used directly from your code.", 101, IsHidden = true)>]
[<RequireQualifiedAccess>]
type ResultRank = 
    | Sequence = 0
    | SingleRow = 1
    | ScalarValue = 2

type Mapper private() = 
    static member GetMapperWithNullsToOptions(nullsToOptions, mapper: obj[] -> obj) = 
        fun values -> 
            nullsToOptions values
            mapper values

open System
open System.Data 

module RuntimeInternals =
    let setupTableFromSerializedColumns (serializedSchema: string) (table: System.Data.DataTable) =
        let primaryKey = ResizeArray()
        for line in serializedSchema.Split('\n') do
            let xs = line.Split('\t')
            let col = new DataColumn()
            col.ColumnName <- xs.[0]
            col.DataType <- Type.GetType( xs.[1], throwOnError = true)  
            col.AllowDBNull <- Boolean.Parse xs.[2]
            if col.DataType = typeof<string>
            then 
                col.MaxLength <- int xs.[3]
            col.ReadOnly <- Boolean.Parse xs.[4]
            col.AutoIncrement <- Boolean.Parse xs.[5]
            if Boolean.Parse xs.[6]
            then    
                primaryKey.Add col 
            table.Columns.Add col

        table.PrimaryKey <- Array.ofSeq primaryKey
