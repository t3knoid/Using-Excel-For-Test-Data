using System;
//using System.Collections.Generic;
//using System.Text;

using System.IO;
using CribbageLib;
using System.Data.OleDb;
using System.Data;

namespace CribbageLibTest
{
  class Program
  {
    static void Main(string[] args)
    {
      try
      {
        Console.WriteLine("\nBegin lightweight test harness with Excel storage demo\n");
        Console.WriteLine("Verifying Excel test case data exists");
        if (!File.Exists("..\\..\\..\\TestCases.xls"))
          throw new Exception("Test case data not found");
        else
          Console.WriteLine("Found test case data");

        // =====

        Console.WriteLine("Determining number of test cases");
        string probeConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=..\\..\\..\\testCases.xls;" + "Extended Properties=\"Excel 8.0;HDR=YES\"";
        OleDbConnection probeConn = new OleDbConnection(probeConnStr);
        probeConn.Open();
        string probe = "SELECT COUNT(*) FROM [tblTestCases$A1:A65536] Where caseID IS NOT NULL";
        OleDbCommand probeCmd = new OleDbCommand(probe, probeConn);
        int count = (int)probeCmd.ExecuteScalar();
        probeConn.Close();
        Console.WriteLine("Number of test cases = " + count);

        // =====

        Console.WriteLine("Reading all test case data from Excel into memory");
        string tcdConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=..\\..\\..\\testCases.xls;" + "Extended Properties=\"Excel 8.0;HDR=YES\"";
        OleDbConnection tcdConn = new OleDbConnection(tcdConnStr);
        tcdConn.Open();
        int lastRow = count + 1;
        string select = "SELECT * FROM [tblTestCases$A1:D" + lastRow + "]";
        OleDbDataAdapter oda = new OleDbDataAdapter(select, tcdConn);
        DataTable dt = new DataTable();
        oda.FillSchema(dt, SchemaType.Source);
        oda.Fill(dt);
        tcdConn.Close();
        Console.WriteLine("All test case data now in memory");

        // =====

        Console.WriteLine("\nCreating Excel test case results file");
        string stamp = DateTime.Now.ToString("s");
        stamp = stamp.Replace(":", "-");
        string resultsFile = "testResults" + stamp + ".xls";

        string tcrConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=..\\..\\..\\" + resultsFile + ";" + "Extended Properties=\"Excel 8.0;HDR=YES\"";
        OleDbConnection tcrConn = new OleDbConnection(tcrConnStr);
        tcrConn.Open();
        string create = "CREATE TABLE tblResults (caseID char(5), Result char(4), WhenRun DateTime)";
        OleDbCommand createCmd = new OleDbCommand(create, tcrConn);
        createCmd.ExecuteNonQuery();
        tcrConn.Close();
        Console.WriteLine("Results file is " + resultsFile);

        // =====

        Console.WriteLine("\nExecuting CribbageLib tests\n");
        string insertConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=..\\..\\..\\" + resultsFile + ";" + "Extended Properties=\"Excel 8.0;HDR=YES\"";
        OleDbConnection insertConn = new OleDbConnection(insertConnStr);
        insertConn.Open();
        OleDbCommand insertCmd = new OleDbCommand();
        insertCmd.Connection = insertConn;
               
        string caseid, input, method, result;
        int actual, expected;
        Console.WriteLine("caseID  input              method       expected  case result");
        Console.WriteLine("=============================================================");
        for (int row = 0; row < dt.Rows.Count; ++row) // main test loop
        {
          //if ( (dt.Rows[row]["caseid"]).GetType() == System.DBNull) break;
          object o = dt.Rows[row]["caseid"];
          if (o == null) break;

          caseid = (string)dt.Rows[row]["caseid"];
          input = (string)dt.Rows[row]["input"];
          method = (string)dt.Rows[row]["method"];
          expected = int.Parse( (string)(dt.Rows[row]["expected"]) );

          CribbageLib.Hand h = new Hand(new Card(input.Substring(0, 2)), new Card(input.Substring(2, 2)),
            new Card(input.Substring(4, 2)), new Card(input.Substring(6, 2)), new Card(input.Substring(8, 2)));

          if (method == "ValueOf15s")
            actual = h.ValueOf15s;
          else if (method == "ValueOfPairs")
            actual = h.ValueOfPairs;
          else
            throw new Exception("Unknown method in test case data");

          DateTime whenRun = DateTime.Now;
          if (actual == expected)
            result = "Pass";
          else
            result = "FAIL";
          
          Console.WriteLine(caseid + "   " + h.ToString() + " " + method.PadRight(15, ' ') + expected.ToString().PadRight(8, ' ') + result);

          string insert = "INSERT INTO tblResults (caseID, Result, WhenRun) values ('" + caseid + "', '" + result + "', '" + whenRun + "')";
          insertCmd.CommandText = insert;
          insertCmd.ExecuteNonQuery();
        } // main test loop

        insertConn.Close();
        Console.WriteLine("\nAll test results stored to Excel\n");

        Console.WriteLine("End Excel as test storage demo\n");
        Console.ReadLine();
      }
      catch (Exception ex)
      {
        Console.WriteLine("Fatal error: " + ex.Message);
        Console.ReadLine();
      }

    } // Main()
  } // class Program
} // ns CribbageLibTest
