using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnapsackProblem
{
  class Program
  {
    struct Task
    {
      public int Fee;
      public int Days;

      public Task(int fee, int days)
      {
        Fee = fee;
        Days = days;
      }
    }

    static Task[] Tasks = new[] { 
        new Task(0, 0)
       ,new Task(130,  9)
       ,new Task(150, 12)
       ,new Task(190, 20)
       ,new Task(190, 23)
       ,new Task(230, 27)
       ,new Task(290, 33)
       ,new Task(330, 31)
       ,new Task( 70,  9)
       ,new Task(330, 30)
       ,new Task(110,  9)
       ,new Task( 90,  6)
       ,new Task(310, 34)
       ,new Task(330, 34)
       ,new Task(190, 22)
       ,new Task(230, 25)
       ,new Task(170, 13)
    };

    /// <summary>
    /// The amount of tasks to select from
    /// </summary>
    static int taskCount = Tasks.Count();
    /// <summary>
    /// The maximum allowed amount of days
    /// </summary>
    static int maxDays = 100;
    /// <summary>
    /// The calculated values for combined items
    /// </summary>
    static int[,] Values = new int[taskCount,maxDays+1];
    /// <summary>
    /// The tasks chosen for the most value
    /// </summary>
    static bool[] ChosenTasks = new bool[taskCount];

    static void Main(string[] args)
    {
      Calculate();
      Backtrack();
      PrintStats();
    }

    /// <summary>
    /// Calculates the the combination giving the most value
    /// </summary>
    private static void Calculate()
    {
      // the first task is empty since the algorithm always needs one previous task to compare with
      for (int d = 0; d <= maxDays; d++)
      {
        Values[0, d] = 0;
      }

      for (int task = 1; task < taskCount; task++)
      {
        for (int days = 0; days <= maxDays; days++)
        {
          if (Tasks[task].Days <= days)
          {
            Values[task, days] = GetMax(
              // previous tasks value at the same amount of days
              Values[task - 1, days]
              // the new value if the current task is chosen
             ,Values[task - 1, days - Tasks[task].Days] + Tasks[task].Fee
            );
          }
          else
          {
            // skip this task and keep previous tasks
            Values[task, days] = Values[task - 1, days];
          }
        }
      }
    }

    /// <summary>
    /// Backtracks through the calculated values to finds the chosen items
    /// </summary>
    private static void Backtrack()
    {
      int daysLeft = maxDays;
      for (int task = taskCount-1; task > 0; task--)
      {
        // the max value for the current item with the given amount of days left
        int max = Values[task, daysLeft];
        // the value at the previous item if the current item was used
        var previous = Values[task - 1, daysLeft - Tasks[task].Days];
        // if the item was used then the current tasks value added to the previous tasks value should match the current value
        if (previous + Tasks[task].Fee == max)
        {
          // the item was used
          ChosenTasks[task] = true;
          // adjust the days left since the item was used
          daysLeft = daysLeft - Tasks[task].Days;
        }
      }
    }

    private static void PrintItemDefinitions()
    {
      for (int t = 0; t < taskCount; t++)
      {
        Console.Write(" # " + t.ToString("00") + " ");
      }
      Console.Write("\r\n");

      for (int t = 0; t < taskCount; t++)
      {
        Console.Write(" F" + Tasks[t].Fee.ToString("000") + " ");
      }
      Console.Write("\r\n");

      for (int t = 0; t < taskCount; t++)
      {
        Console.Write(" D " + Tasks[t].Days.ToString("00") + " ");
      }
      Console.Write("\r\n");
    }

    private static void PrintStats()
    {
      Console.WriteLine("The Matrix:");
      Console.WriteLine();

      PrintItemDefinitions();

      Console.WriteLine(new String('-', taskCount * 6) + "|");

      for (int d = 0; d <= maxDays; d++)
      {
        for (int t = 0; t < Tasks.Count(); t++)
        {
          if (t == taskCount - 1 && d == maxDays)
          {
            Console.Write("[" + Values[t, d].ToString("0000") + "]");
          }
          else
          {
            Console.Write(" " + Values[t, d].ToString("0000") + " ");
          }
        }
        Console.Write("|" + d + "\r\n");
        // uncomment for extra vertical space
        //Console.Write(new String(' ', taskCount * 6) + "|\r\n");
      }

      Console.WriteLine(new String('-', taskCount * 6) + "|");

      PrintItemDefinitions();
      
      Console.WriteLine();
      Console.WriteLine("Max fees: " + Values[taskCount - 1, maxDays]);
      Console.Write("Tasks: ");

      int fees = 0;
      for (int t = 0; t < ChosenTasks.Count(); t++)
      {
        if (ChosenTasks[t] == true)
        {
          fees += Tasks[t].Fee;
          Console.Write("#" + t.ToString("00") + "(F" + Tasks[t].Fee +")");
          if (t < ChosenTasks.Count() - 1)
          {
            Console.Write(" + ");
          }
        }
      }
      Console.Write(" = " + fees);

      Console.WriteLine();

      Console.ReadKey();
    }

    /// <summary>
    /// Returns the maximum of the two integers.
    /// </summary>
    private static int GetMax(int x, int y)
    {
      return x > y ? x : y;
    }
  }
}
