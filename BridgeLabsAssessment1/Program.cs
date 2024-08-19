using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json.Linq;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium.Interactions;

class CrimeReportFormAutomation
{
    // Declare ExtentReports and ExtentTest
    static ExtentReports extent;
    static ExtentTest test;

    static void Main(string[] args)
    {
        // Initialize ExtentReports
        InitializeReport();

        // Start a new test in the report
        test = extent.CreateTest("Crime Report Form Automation");

        // Load the JSON data
        var jsonData = LoadJsonData("TestData/testdata.json");

        IWebDriver driver = new ChromeDriver();

        Actions actions = new Actions(driver);

        try
        {
            driver.Navigate().GoToUrl("https://deploy-preview-1--safercitywebapp.netlify.app/crime");
            driver.Manage().Window.Maximize();
            test.Log(Status.Info, "Navigated to crime reporting page.");
            CaptureScreenshot(driver, "NavigatedToCrimePage");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Enter data into the form fields
            IWebElement nameField = driver.FindElement(Locators.NameField);
            nameField.SendKeys(jsonData["name"].ToString());
            test.Log(Status.Pass, "Entered name.");
            CaptureScreenshot(driver, "EnteredName");

            IWebElement surnameField = wait.Until(ExpectedConditions.ElementIsVisible(Locators.SurnameField));
            surnameField.SendKeys(jsonData["surname"].ToString());
            test.Log(Status.Pass, "Entered surname.");
            CaptureScreenshot(driver, "EnteredSurname");

            IWebElement emailField = driver.FindElement(Locators.EmailField);
            emailField.SendKeys(jsonData["email"].ToString());
            test.Log(Status.Pass, "Entered email.");
            CaptureScreenshot(driver, "EnteredEmail");

            IWebElement phoneField = driver.FindElement(Locators.PhoneField);
            phoneField.SendKeys(jsonData["phone"].ToString());
            test.Log(Status.Pass, "Entered phone number.");
            CaptureScreenshot(driver, "EnteredPhoneNumber");

            IWebElement nextButton = driver.FindElement(Locators.NextButton);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", nextButton);
            test.Log(Status.Info, "Clicked Next button.");
            CaptureScreenshot(driver, "ClickedNextButton");

            // Select crime type
            IWebElement crimeTypeDropdown = wait.Until(ExpectedConditions.ElementToBeClickable(Locators.CrimeTypeDropdown));
            crimeTypeDropdown.Click();

            IWebElement crimeSelectElement = wait.Until(ExpectedConditions.ElementIsVisible(Locators.CrimeSelectElement));
            SelectElement crimeSelect = new SelectElement(crimeSelectElement);
            crimeSelect.SelectByText(jsonData["crimeType"].ToString());
            test.Log(Status.Pass, "Selected crime type.");
            CaptureScreenshot(driver, "SelectedCrimeType");

            // Enter description and location
            IWebElement body = driver.FindElement(By.TagName("body"));
            body.SendKeys(Keys.End);
            Thread.Sleep(2000);
            body.SendKeys(Keys.PageUp);

            IWebElement descriptionBox = wait.Until(ExpectedConditions.ElementIsVisible(Locators.DescriptionBox));
            descriptionBox.Click();
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", descriptionBox);
            descriptionBox.SendKeys(jsonData["description"].ToString());
            test.Log(Status.Pass, "Entered crime description.");
            CaptureScreenshot(driver, "EnteredCrimeDescription");

            IWebElement locationInput = driver.FindElement(Locators.LocationInput);
            locationInput.SendKeys(jsonData["location"].ToString());
            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement autoCompleteResult = wait2.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(Locators.AutoCompleteResult));
            autoCompleteResult.Click();
            test.Log(Status.Pass, "Entered and selected location.");
            CaptureScreenshot(driver, "EnteredLocation");

            // Select date
            IWebElement dateButton = driver.FindElement(Locators.DateButton);
            dateButton.Click();
            IWebElement todayDate = driver.FindElement(By.XPath($"//button[text()='{jsonData["date"].ToString()}']"));
            todayDate.Click();
            test.Log(Status.Pass, "Selected date.");
            CaptureScreenshot(driver, "SelectedDate");

            // Select time
            IWebElement timeInput = driver.FindElement(Locators.TimeInput);
            timeInput.Click();
            IWebElement timeNow = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath($"//a[contains(text(),'{jsonData["time"].ToString()}')]")));
            timeNow.Click();
            test.Log(Status.Pass, "Selected time.");
            CaptureScreenshot(driver, "SelectedTime");

            // Attach files
            IWebElement attachFiles = driver.FindElement(Locators.AttachFiles);
            string filePath = jsonData["filePath"].ToString();
            attachFiles.SendKeys(filePath);
            test.Log(Status.Pass, "Attached file.");
            CaptureScreenshot(driver, "AttachedFile");

            // Select radio button and enter case details
            IWebElement radioButtonYes = driver.FindElement(Locators.RadioButtonYes);
            radioButtonYes.Click();
            test.Log(Status.Pass, "Selected Yes for radio button.");
            CaptureScreenshot(driver, "SelectedYesRadioButton");

            IWebElement caseNumberInput = driver.FindElement(Locators.CaseNumberInput);
            caseNumberInput.SendKeys(jsonData["caseNumber"].ToString());
            test.Log(Status.Pass, "Entered case number.");
            CaptureScreenshot(driver, "EnteredCaseNumber");

            IWebElement policeStationInput = driver.FindElement(Locators.PoliceStationInput);
            policeStationInput.SendKeys(jsonData["policeStation"].ToString());
            test.Log(Status.Pass, "Entered police station name.");
            CaptureScreenshot(driver, "EnteredPoliceStation");

            // Submit the form
            IWebElement submitButton = driver.FindElement(Locators.SubmitButton);
            submitButton.Click();
            test.Log(Status.Info, "Clicked Submit button.");
            CaptureScreenshot(driver, "ClickedSubmitButton");

       
            
                // Locate the map marker element
                IWebElement marker = driver.FindElement(By.CssSelector(".mapboxgl-marker"));

                // Define the new location where you want to move the marker
                int newXOffset = 50; // Pixels to move right
                int newYOffset = -30; // Pixels to move up

                // Perform the drag and drop action
                actions.ClickAndHold(marker)
                       .MoveByOffset(newXOffset, newYOffset)
                       .Release()
                       .Build()
                       .Perform();

                test.Log(Status.Pass, "Marker moved successfully!");
            //CaptureScreenshot(driver, "haa");


            // Validate form submission
            try
            {
                IWebElement successMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(Locators.SuccessMessage));
                if (successMessage.Text.Contains("Thank you"))
                {
                    test.Log(Status.Pass, "Form submitted successfully!");
                }
                else
                {
                    test.Log(Status.Fail, "Form submission failed: Success message not found.");
                }
            }
            catch (NoSuchElementException)
            {
                test.Log(Status.Fail, "Form submission failed: Success message not found.");
            }
            CaptureScreenshot(driver, "FormSubmissionResult");
        }
        catch (Exception ex)
        {
            test.Log(Status.Fail, $"Test failed with exception: {ex.Message}");
            CaptureScreenshot(driver, "ExceptionOccurred");
        }
        finally
        {
            // Close the browser and flush the report
            driver.Quit();
            test.Log(Status.Info, "Browser closed.");
            extent.Flush();  // Important to write the report at the end
        }
    }

    static JObject LoadJsonData(string filePath)
    {
        // Combine base directory with file path
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("The JSON file was not found.", path);
        }

        string json = File.ReadAllText(path);
        return JObject.Parse(json);
    }

    static void InitializeReport()
    {
        // Specify the report file path
        var reporter = new ExtentSparkReporter(@"C:\Reports\CrimeReportAutomation.html");

        // Initialize ExtentReports object
        extent = new ExtentReports();
        extent.AttachReporter(reporter);

        // Add system info to the report
        extent.AddSystemInfo("OS", "Windows 10");
        extent.AddSystemInfo("Environment", "QA");
        extent.AddSystemInfo("User", "Macauze");
    }

   


    static void CaptureScreenshot(IWebDriver driver, string screenshotName)
    {
        string screenshotDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
        Directory.CreateDirectory(screenshotDirectory);

        Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
        string filePath = Path.Combine(screenshotDirectory, $"{screenshotName}.png");
        screenshot.SaveAsFile(filePath);  // Save the file directly

        test.AddScreenCaptureFromPath(filePath);
    }

}
