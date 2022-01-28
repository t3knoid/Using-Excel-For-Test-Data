# Creating the Test Harness

For clarity, I've combined the code used to perform several different tasks with Excel test case data into a single test harness program. [Figure 5](Figure 5.md) shows the structure of the test harness that generated the output shown in Figure 2.

I begin my test harness by adding a project reference to the CribbageLib.dll component that houses my library under test. Then I add a using statement for the namespaces in the library so that I can reference the Card and Hand classes without having to fully qualify their names. In addition, I add a using statement to the System.Data.OleDb namespace, which contains classes that can be used to connect to, access, and manipulate OLE DB data sources, including Excel spreadsheets. I also add a using statement to the System.Data namespace so I can easily instantiate and use a DataTable object to act as an in-memory data store for my test case data from Excel. Because this column is essentially a tutorial, for simplicity I organize my test harness into a single Main method—you, however, may want to consider making your harnesses more modular.

I begin by printing a start message to console, then use the static File.Exists method to verify that the Excel test case data is located where I expect it to be. (To simplify this example, I have removed most of the error-checking code you would need in a production environment.) Once I know my test case data exists, I probe the Excel spreadsheet to find out how many rows of data there are:

```C#
int count;
string probeConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                      "Data Source=testCases.xls;" +
                      "Extended Properties=\"Excel 8.0;HDR=YES\"";
using(OleDbConnection probeConn = new OleDbConnection(probeConnStr))
{
    probeConn.Open();
    string probe = "SELECT COUNT(*) FROM [tblTestCases$A1:A65536] " +
                    "Where caseID IS NOT NULL";
    using(OleDbCommand probeCmd = new OleDbCommand(probe, probeConn))
    {
        count = (int)probeCmd.ExecuteScalar();
    }
}
```

Here, I create a connection string that specifies the appropriate OLE DB provider, location, and auxiliary information. You have to be careful with the syntax. Notice that in the Extended Attributes property I use a \" sequence to embed a double quote character into the connection string. The HDR=YES attribute indicates that my Excel worksheet has an initial header row. The "Excel 8.0" part does not directly refer to the version of the Excel program on my computer—it refers to the Jet database ISAM (Indexed Sequential Access Method) format that is installed. You can check the ISAM version on your machine by viewing the system registry setting HKEY_LOCAL_MACHINE\Software\Microsoft\Jet\4.0\ISAM Formats. Next I create an OleDbConnection object, passing the connection string to the constructor. I call the Open method to establish a connection to my Excel spreadsheet. I then craft a select string that will return the number of non-NULL rows in my spreadsheet, which will be the number of test cases since the header row does not contribute to the return value. This, of course, assumes that I do not have any empty rows in my Excel test case data. Notice the somewhat unusual syntax in the SELECT statement, which refers to my Excel data. I surround my virtual table with square bracket characters and I append a $ character to the virtual table name. I specify a range of A1:A65536 because 65536 is the maximum number of rows supported in an Excel worksheet. Finally, I use the ExecuteScalar method of the OleDbCommand class so I can capture the return value of the number of test cases into a variable named count. And I complete the probing code by closing the OleDbConnection.

Now I'm ready to reconnect to my Excel spreadsheet and read all my test case data from Excel into memory like this:

```C#
string tcdConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                    "Data Source=testCases.xls;" +
                    "Extended Properties=\"Excel 8.0;HDR=YES\"";
using(OleDbConnection tcdConn = new OleDbConnection(tcdConnStr))
{
    tcdConn.Open();
    int lastRow = count + 1;
    string select = "SELECT * FROM [tblTestCases$A1:D" + lastRow + "]";
    OleDbDataAdapter oda = new OleDbDataAdapter(select, tcdConn);
    DataTable dt = new DataTable();
    oda.FillSchema(dt, SchemaType.Source);
    oda.Fill(dt);
}
```

In this snippet, I create a new connection string. I could use the same string variable as I used to probe the number of test cases; I use a different string variable only for clarity and to make it easier for you to modularize my code if you wish. Next I create a new OleDbConnection object. Again, I could have reused the connection object I used for the test case count probe. After opening the connection, I compute the last row of test case data by adding 1 to the number of actual test cases in the Excel spreadsheet—the extra 1 accounts for the header row. Then I build a SELECT string by concatenating the first part of the select with the variable holding the number of the last row that contains test case data.

In order to fill my in-memory DataTable object with test case data, I create an OleDbDataAdapter. Then I instantiate a new DataTable object to hold all my test case data. I use the OleDbDataAdapter's FillSchema method to configure my DataTable attributes to conform to my Excel data source. I then call the Fill method, which actually transfers my test case data from Excel into memory. And I finish by closing the OleDbConnection. Very clean and simple.
