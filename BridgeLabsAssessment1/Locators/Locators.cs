using OpenQA.Selenium;

public static class Locators
{
    // Form fields
    public static By NameField => By.Id("name");
    public static By SurnameField => By.Id("surname");
    public static By EmailField => By.Id("email");
    public static By PhoneField => By.Name("phone");

    // Buttons
    public static By NextButton => By.XPath("//button[text()='Next']");
    public static By CrimeTypeDropdown => By.XPath("//button[@id=':r4:-form-item']");
    public static By SubmitButton => By.XPath("//button[text()='Submit']");

    // Crime type selection
    public static By CrimeSelectElement => By.XPath("//select");

    // Description and location
    public static By DescriptionBox => By.Id("crime-description");
    public static By LocationInput => By.Id("crimeLocation");
    public static By AutoCompleteResult => By.CssSelector(".pac-item");

    // Date and time
    public static By DateButton => By.Id(":r8:-form-item");
    public static By TimeInput => By.XPath("//input[@placeholder='Select time']");

    // File attachment
    public static By AttachFiles => By.XPath("//input[@type='file']");

    // Radio button and case details
    public static By RadioButtonYes => By.XPath("//button[@role='radio' and @value='Yes']");
    public static By CaseNumberInput => By.Id("caseNumber");
    public static By PoliceStationInput => By.Id("policeStation");

    // Success message
    public static By SuccessMessage => By.XPath("//div[contains(text(),'Thank you for keeping the city safe')]");
}
