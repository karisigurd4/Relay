using System;

namespace Relay.IntegrationTest.Utils
{
  public static class IntegrationTestLogger
  {
    public static void Info(string log)
    {
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine($"Info: {log}");
    }

    public static void Error(string log)
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine(log);
    }

    public static void Success(string log)
    {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(log);
    }

    public static void NewLine()
    {
      Console.WriteLine();
    }
  }
}
