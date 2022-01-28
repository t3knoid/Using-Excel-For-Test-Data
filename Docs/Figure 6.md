# Figure 6 Creating a Spreadsheet to Store Results

```C#
string stamp = DateTime.Now.ToString("s");
stamp = stamp.Replace(":", "-");
string resultsFile = "testResults" + stamp + ".xls";
string tcrConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                    "Data Source=" + resultsFile + ";" +
                    "Extended Properties=\"Excel 8.0;HDR=YES\"";
using(OleDbConnection tcrConn = new OleDbConnection(tcrConnStr))
{
    tcrConn.Open();
    string create = "CREATE TABLE tblResults (caseID char(5),
                     Result char(4), WhenRun DateTime)";
    using(OleDbCommand createCmd = new OleDbCommand(create, tcrConn))
    {
        createCmd.ExecuteNonQuery();
    }
}