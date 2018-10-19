#r @"..\..\..\bin\FSharp.Data.SqlClient.dll"
//#r @"bin\Debug\Lib.dll"
#r @"System.Configuration"
#r @"System.Transactions"

let connectionString = "Data Source=.;Initial Catalog=AdventureWorks2012;Integrated Security=True"
type GetEmployeeByLevel = FSharp.Data.SqlClient.SqlCommandProvider<"
    SELECT OrganizationNode FROM HumanResources.Employee WHERE OrganizationNode = @OrganizationNode"
    , connectionString, SingleRow = true>


//DataAccess.get42()

//open System

////let shifts = new DataAccess.AdventureWorks.HumanResources.Tables.Shift()
//let shifts = DataAccess.getShiftTable()
//do 
//    use tran = new System.Transactions.TransactionScope()
//    shifts.AddRow("French coffee break", StartTime = TimeSpan.FromHours 10., EndTime = TimeSpan.FromHours 12.)
//    shifts.Update() |> printfn "Records affected %i"
