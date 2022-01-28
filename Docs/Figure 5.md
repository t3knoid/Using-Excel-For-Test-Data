# Figure 5 Test Harness Structure

```C#
using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using CribbageLib;
namespace CribbageLibTest
{
  class Program
  {
    static void Main(string[] args)
    {
      try
      {
        Console.WriteLine("\nBegin lightweight test harness with " +
            "Excel storage demo\n");
        Console.WriteLine("Verifying Excel test case data exists");
        if (!File.Exists("..\\..\\..\\TestCases.xls"))
          throw new Exception("Test case data not found");
        else Console.WriteLine("Found test case data");
        Console.WriteLine("Determining number of test cases");
        ... // probe Excel test case data code here
        Console.WriteLine("Number of test cases = " + count);
        Console.WriteLine("Reading all test case data from Excel " +
            "into memory");
        ... // read all test case data code here
        Console.WriteLine("All test case data now in memory");
        Console.WriteLine("\nCreating Excel test case results file");
        ... // create Excel code here
        Console.WriteLine("Results file is " + resultsFile);
        Console.WriteLine("\nExecuting CribbageLib tests\n");
        ... // connect to Excel test results data here
               
        string caseid, input, method, result;
        int actual, expected;
        Console.WriteLine("caseID  input method  expected  case result");
        Console.WriteLine("===========================================");
        for (int row = 0;
row <
dt.Rows.Count;
++row) // main test loop
        {
          ... // read test case, execute, print result, 
              // save result to Excel
        }

        Console.WriteLine("\nAll test results stored to Excel\n");
        Console.WriteLine("End Excel as test storage demo\n");
      }
      catch (Exception ex)
      {
        Console.WriteLine("Fatal error: " + ex.Message);
      }
      Console.ReadLine();
    } // Main()
  } // class Program
} // ns CribbageLibTest
```
