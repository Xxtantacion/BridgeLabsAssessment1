using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;

public class ReportManager
{
    public static ExtentReports extent;
    public static ExtentTest test;

    public static void InitializeReport()
    {
        // Specify the report file path
        var sparkReporter = new ExtentSparkReporter(@"C:\Reports\TestReport.html");

        // Initialize the ExtentReports object
        extent = new ExtentReports();
        extent.AttachReporter(sparkReporter);

        // You can add system info to the report
        extent.AddSystemInfo("OS", "Windows");
        extent.AddSystemInfo("Environment", "QA");
    }

    public static void CreateTest(string testName)
    {
        test = extent.CreateTest(testName);
    }

    public static void Log(Status status, string details)
    {
        test.Log(status, details);
    }

    public static void FlushReport()
    {
        extent.Flush(); // This is important to write the report at the end
    }
}
