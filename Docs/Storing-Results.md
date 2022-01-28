# Storing Results

Now that I've gotten all my test case data into memory, I am ready to create a new Excel spreadsheet to store my test case results. The code is similar to my other Excel tasks except this time I use an OleDbCommand object (see [Figure 6](Figure 6.md).

I begin by fetching the system date and time so that I can create a time-stamped results file name. I pass an "s" argument to the ToString() method so I will get a date-time string in a format that is sortable (such as 2006-07-23T16:56:44). But because the : character is not valid in a file name, I use the String.Replace method to replace all : characters with hyphens.

My connection string is similar to the two others I've already used, except that I embed the time-stamped file name into it. I then open the connection to a new Excel file. The file does not actually exist yet, so you can think of this as a kind of virtual connection.

Next I craft a SQL-like CREATE string specifying a virtual table name of tblResults. Notice that I can specify the data type for each column. You have to be a bit careful here because many SQL data types do not map exactly to Excel data types. An alternative approach is to simply design your results file so that all the columns are varchar type data. Then, if you need to perform some numeric analysis of your test results in Excel (say, computing an average or a maximum value), you can manually format the columns you are analyzing into the appropriate Excel type (such as Number or Percentage).

In this example, I store the test case ID, the test result as "pass" or "fail", and a DateTime variable that holds the time at which the test case was executed. I finish by instantiating an OleDbCommand object and using the ExecuteNonQuery method. As I mentioned earlier, this process creates an Excel spreadsheet that contains a worksheet named tblResults, which has a Named Area tblResults.

Now that my test case data is in memory and my Excel results file has been created, I am ready to execute my tests and store my results. I begin by creating a connection to my newly created Excel results file, as shown here:

```C#
Console.WriteLine("\nExecuting CribbageLib tests\n");
string insertConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                       "Data Source=" + resultsFile + ";" +
                       "Extended Properties=\"Excel 8.0;HDR=YES\"";
using(OleDbConnection insertConn = new OleDbConnection(insertConnStr))
{
    insertConn.Open();
    OleDbCommand insertCmd = new OleDbCommand();
    insertCmd.Connection = insertConn;
```

The next thing I do is set up an INSERT command, which I will use to insert a test result after determining whether a test case has passed or failed.

Next I set up my test harness variables and then print a simple output header. I declare string variables to hold my test case ID, my test case input, the method (or Property in this example) under test, and an indicator of the result ("pass" or "fail"). I declare int variables expected and actual to hold the expected return value from the method under test (which is stored in my in-memory DataTable) and the actual result returned when the method under test is called by the test harness. This is done like so:

```C#
string caseid, input, method, result;
int actual, expected;
Console.WriteLine("caseID  input method  expected  case result");
Console.WriteLine("===========================================");
```

Now the main test loop iterates through each row of the DataTable object, as shown in the following:

```C# 
for (int row = 0;
row <
dt.Rows.Count;
++row)
```

Inside the main test loop, I first perform a rudimentary check to make sure I have a valid test case. Then I fetch each column of the current row of the DataTable and store into the variables with more meaningful names that I declare just outside the loop:

```C#
object o = dt.Rows[row]["caseid"];
if (o == null) break;
   caseid = (string)dt.Rows[row]["caseid"];
   input = (string)dt.Rows[row]["input"];
   method = (string)dt.Rows[row]["method"];
   expected = int.Parse( (string)(dt.Rows[row]["expected"]) );
```

Notice that this is the point at which I do my type conversions. Remember that I originally stored all my test case data as type Text in my Excel spreadsheet and read all the data as type String into my DataTable object. Now I can call the method under test and retrieve the actual value:

```C#
CribbageLib.Hand h = new Hand(new Card(input.Substring(0, 2)),
                              new Card(input.Substring(2, 2)),
                              new Card(input.Substring(4, 2)),
                              new Card(input.Substring(6, 2)),
                              new Card(input.Substring(8, 2)));
if (method == "ValueOf15s") actual = h.ValueOf15s;
   else if (method == "ValueOfPairs") actual = h.ValueOfPairs;
           else throw new Exception("Unknown method in test case data");
```

I use the String.Substring method to parse out each pair of characters, such as 7c, which represent a Card object. The first integer argument to Substring is a zero-based index value of where in the string to begin extracting the substring. The second integer argument is the number of characters to extract (as opposed to the ending index as you might have thought). I branch on the value of variable method and call the method under test, capturing the actual return value. Finally, to finish my main test loop, I determine the date and time of my test case execution, determine the test result, build up an INSERT string, print my result to console, and insert the result into the Excel results worksheet:

```C#
DateTime whenRun = DateTime.Now;
result = (actual == expected) ? "Pass" : "FAIL";
          
Console.WriteLine(caseid + "   " + h.ToString() + " " +
                  method.PadRight(15, ' ') +
                  expected.ToString().PadRight(8, ' ') + result);
string insert = "INSERT INTO tblResults (caseID, Result, WhenRun)
                 values ('" + caseid + "', '" + result +
                 "', '" + whenRun + "')";
insertCmd.CommandText = insert;
insertCmd.ExecuteNonQuery();
```

I use the PadRight method—this is an old trick—to line up my output in columns. The insert statement is a bit ugly because of the embedded single quotes, commas, and paren characters—you just have to be careful here. Because I am invoking an INSERT command, I use the ExecuteNonQuery method to actually perform the insert operation.
