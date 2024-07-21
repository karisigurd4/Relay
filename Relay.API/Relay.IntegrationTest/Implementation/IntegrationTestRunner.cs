namespace Relay.IntegrationTest.Implementation
{
  using Relay.IntegrationTest.Attributes;
  using Relay.IntegrationTest.Interfaces;
  using Relay.IntegrationTest.Utils;
  using System;
  using System.Linq;

  public class IntegrationTestRunner : IIntegrationTestRunner
  {
    private IIntegrationTest[] integrationTests;

    public IntegrationTestRunner(IIntegrationTest[] integrationTests)
    {
      this.integrationTests = integrationTests;
    }

    public void RunTests()
    {
      bool allTestCasesSuccessful = true;

      var integrationTestsOrdered = integrationTests
        .OrderBy(x => ((IntegrationTestAttribute)Attribute.GetCustomAttribute(x.GetType(), typeof(IntegrationTestAttribute))).Order)
        .ToArray();

      foreach (var integrationTest in integrationTests)
      {
        try
        {
          var testDescription = ((IntegrationTestAttribute)Attribute
            .GetCustomAttribute(integrationTest.GetType(), typeof(IntegrationTestAttribute)))
            .Description;

          IntegrationTestLogger.NewLine();
          IntegrationTestLogger.Info($"Starting integration test");
          IntegrationTestLogger.Info($"{testDescription}");
          IntegrationTestLogger.NewLine();

          IntegrationTestLogger.Info($"Pre execution");
          integrationTest.BeforeExecution();

          var testCases = integrationTest
            .GetType()
            .GetMethods()
            .Where(x => Attribute.IsDefined(x, typeof(IntegrationTestCaseAttribute)))
            .OrderBy(x => ((IntegrationTestCaseAttribute)Attribute.GetCustomAttribute(x, typeof(IntegrationTestCaseAttribute))).Order)
            .ToArray();

          foreach (var testCase in testCases)
          {
            var testCaseDescription = ((IntegrationTestCaseAttribute)Attribute
             .GetCustomAttribute(testCase, typeof(IntegrationTestCaseAttribute)))
             .Description;

            IntegrationTestLogger.NewLine();
            IntegrationTestLogger.Info($"Starting test case");

            var result = testCase.Invoke(integrationTest, null);
            if ((bool)result)
            {
              IntegrationTestLogger.Success($"Test case ran successfully");
            }
            else
            {
              IntegrationTestLogger.Error($"Test case did not run successfully");
              allTestCasesSuccessful = false;
            }
          }

          IntegrationTestLogger.NewLine();
          IntegrationTestLogger.Info($"Post execution");
          IntegrationTestLogger.NewLine();

          integrationTest.AfterExecution();
        }
        catch (Exception e)
        {
          IntegrationTestLogger.Error($"Test case failed with exception" + e);
          allTestCasesSuccessful = false;
        }
      }
    }
  }
}
