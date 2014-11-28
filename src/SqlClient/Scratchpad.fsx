
#r "System.Transactions"

open System
open System.IO
open System.Data
open System.Data.SqlClient
open System.Data.SqlTypes

let conn = new SqlConnection(@"Data Source=.;Initial Catalog=ThermionDB;Integrated Security=True")
conn.Open()

//let reader = cmd.ExecuteReader()
//let reader = cmd.ExecuteReader()
//let schema = conn.GetSchema("DataTypes")
//[for c in schema.Columns -> c.ColumnName ]
//[for r in schema.Rows -> [for c in schema.Columns do if not(r.IsNull(c)) then yield sprintf "%s - %A" c.ColumnName r.[c]] |> String.concat "\n" |> sprintf "%s\n\n" ]
//[for r in schema.Rows -> r.["TypeName"], r.["ProviderDbType"], r.["DataType"].GetType().Name]
//
//let metaSchema = conn.GetSchema("MetaDataCollections")
//[for c in metaSchema.Columns -> c.ColumnName ]
//[for r in metaSchema.Rows -> [for c in metaSchema.Columns do if not(r.IsNull(c)) then yield sprintf "%s - %A" c.ColumnName r.[c]] |> String.concat "\n" |> sprintf "%s\n\n" ]


let cmd = new SqlCommand("SELECT * FROM ImportBatch", conn)
let adapter = new SqlDataAdapter(cmd)
let dataTable = adapter.FillSchema(new DataTable(), SchemaType.Source)
dataTable.Columns.Remove("createdTmsp")

//dataTable.Columns.Remove("createdTmsp")
dataTable.Columns.["createdTmsp"].AllowDBNull <- true
//dataTable.Columns.["createdTmsp"].DefaultValue <- null //DBNull.Value
let row = dataTable.NewRow()
row.["description"] <- "my_desc9"
row.["createdTmsp"] <- DateTime.Now.AddDays(10.)
//dataTable.LoadDataRow([| null; box "my_desc3" |], LoadOption.Upsert)

let adapter2 = new SqlDataAdapter("SELECT importBatch, description FROM ImportBatch", conn)
let builder = new SqlCommandBuilder(adapter2)
adapter.InsertCommand <- builder.GetInsertCommand()
dataTable.Rows.Add row
adapter.Update dataTable

//let sqlBulkCopyOptions = SqlBulkCopyOptions.CheckConstraints |||
//                                                        SqlBulkCopyOptions.FireTriggers |||
//                                                        //SqlBulkCopyOptions.KeepNulls |||
//                                                        SqlBulkCopyOptions.TableLock |||
//                                                        SqlBulkCopyOptions.UseInternalTransaction
let sqlBulkCopy = new SqlBulkCopy(conn)
sqlBulkCopy.DestinationTableName <- "dbo.ImportBatch"
sqlBulkCopy.ColumnMappings.Add("importBatch", "importBatch")
sqlBulkCopy.ColumnMappings.Add("description", "description")
//sqlBulkCopy.ColumnMappings.Add("createdTmsp", "createdTmsp")
sqlBulkCopy.WriteToServer([| row |])

dataTable.Columns.[0]

let schemaStorage = new StringWriter()
dataTable.WriteXmlSchema(schemaStorage)
let clone = new DataTable()
clone.Columns.Count
clone.ReadXmlSchema(new StringReader(schemaStorage.ToString()))


//#r "Microsoft.SqlServer.Types"
//let result = cmd.ExecuteScalar()
result.GetType().AssemblyQualifiedName
result.GetType().Assembly.FullName
result |> unbox<Microsoft.SqlServer.Types.SqlHierarchyId>
//result :?> Microsoft.SqlServer.Types.SqlHierarchyId
Convert.ChangeType(result, typeof<Microsoft.SqlServer.Types.SqlHierarchyId>)

[ for c in reader.GetSchemaTable().Columns -> c.ColumnName, c.DataType.Name ]
let r = reader.GetSchemaTable().Rows |> Seq.cast<DataRow> |> Seq.find (fun x -> string x.["ColumnName"] =  "OrganizationLevel")


SqlCommandBuilder.DeriveParameters(cmd)
cmd.Parameters.["@input"].Value <- 12
cmd.Parameters.["@output"].Value <- DBNull.Value
cmd.Parameters.["@nullOutput"].Value <- DBNull.Value
cmd.Parameters.["@nullStringOutput"].Value <- DBNull.Value
[ for p in cmd.Parameters -> sprintf "\n%A, %A: [%O]" p.ParameterName p.Direction p.Value] |> String.concat "," |> printfn "Params: %A" 
using(cmd.ExecuteReader()) (fun reader -> reader |> Seq.cast<IDataRecord> |> Seq.map (fun x -> x.[1], x,[2]) |> Seq.toArray)
[ for p in cmd.Parameters -> sprintf "%A: [%O]" p.ParameterName p.Value] |> String.concat "," |> printfn "Params: %A" 





//DEFAULT PARAMS

//#r "Microsoft.SqlServer.TransactSql.ScriptDom"
//
//open Microsoft.SqlServer.TransactSql.ScriptDom
//open System.IO
//open System.Collections.Generic
//open System.Data.SqlClient
//open System.Data

//let getSpBody = new SqlCommand("exec sp_helptext 'hw.usp_InjectionWellConversion'")
////getUspSearchCandidateResumesBody.Connection <- new SqlConnection(@"Data Source=(LocalDb)\v11.0;Initial Catalog=AdventureWorks2012;Integrated Security=True")
//getSpBody.Connection <- new SqlConnection(@"Data Source=.;Initial Catalog=dbElephant;Integrated Security=True")
//getSpBody.Connection.Open()
//let spBody = getSpBody.ExecuteReader() |> Seq.cast<IDataRecord> |> Seq.map (fun x -> string x.[0]) |> String.concat ""
//
//let parser = TSql110Parser(true)
//let tsqlReader = new StringReader(spBody)
//let mutable errors: IList<ParseError> = null
//let fragment = parser.Parse(tsqlReader, &errors)
//
//let paramInfo = List<ProcedureParameter>()
//
//fragment.Accept {
//    new TSqlFragmentVisitor() with
//        member __.Visit(node : ProcedureParameter) = 
//            base.Visit node
//            paramInfo.Add node
//}
//
//let rec getParamDefaultValue (p: ProcedureParameter) = 
//    match p.Value with
//    | :? Literal as x ->
//        match x.LiteralType with
//        | LiteralType.Default | LiteralType.Null -> Some null
//        | LiteralType.Integer -> x.Value |> int |> box |> Some
//        | LiteralType.Money | LiteralType.Numeric -> x.Value |> decimal |> box |> Some
//        | LiteralType.Real -> x.Value |> float |> box |> Some 
//        | _ -> None
//    //| :? UnaryExpression as expr ->
//    | _ -> None 
//
//
//for p in paramInfo do
//    let xxx = p.Value 
//    match p.Value with
//    | :? Literal as expr -> 
//        printfn "%A=%A of type %O" p.VariableName.Value expr.Value expr.LiteralType
//    //|:? UnaryExpression as expr
//
//    | _ -> ()
//        
//
//

open System.Linq

let xs = [ 1, "l_1"; 2, "left_2" ]
let ys = [ 2, "r_2.1"; 2, "r_2.2"; 3, "r_3"]
query {
    for (y1, y2) in ys do 
    leftOuterJoin (x1, x2) in xs on (y1 = x1) into zs
    select (y1, zs.DefaultIfEmpty((42, "hehe")).ToArray())
} |> Seq.toArray

