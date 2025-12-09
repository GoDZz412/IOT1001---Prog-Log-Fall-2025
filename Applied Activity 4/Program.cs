using System;
using System.Diagnostics;
using System.Reflection;

namespace Applied_Activity_4
{
    class Program
    {
        //Function for displaying the menu and handling the options
        static void DisplayMenu()
        {
            string[] tasks = new string[0]; //Main tasks array
            string[] temp = new string[10]; //Temporary array for storing tasks before moving it to main task array (otherwise there will be default elements)
            int choice;
            bool runProgram = true;

            //Displaying menu until users choose exit and manage option cases
            while (runProgram == true)
            {
                Console.WriteLine("===TODO List Manager===\n");
                Console.WriteLine("1. Add a task");
                Console.WriteLine("2. View all tasks");
                Console.WriteLine("3. Mark a task as complete");
                Console.WriteLine("4. Delete a task");
                Console.WriteLine("5. Modify a task");
                Console.WriteLine("6. View incomplete tasks only");
                Console.WriteLine("7. Exit");
                Console.WriteLine("8. Export all tasks into a file");
                Console.WriteLine("9. Import tasks from a file");
                Console.Write("\nEnter an option number: ");

                //Check integer parsing
                if (int.TryParse(Console.ReadLine(), out choice) == true)
                {
                    //Switch case to handle the options
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            tasks = AddTask(tasks, temp);
                            break;
                        case 2:
                            Console.Clear();
                            ViewAllTasks(tasks);
                            break;
                        case 3:
                            Console.Clear();
                            MarkTaskComplete(tasks, temp);
                            break;
                        case 4:
                            Console.Clear();
                            tasks = HandleDeletionOptions(tasks, temp);
                            break;
                        case 5:
                            Console.Clear();
                            ModifyTask(tasks, temp);
                            break;
                        case 6:
                            Console.Clear();
                            ViewIncompleteTasks(tasks);
                            break;
                        case 7:
                            Console.WriteLine("Exiting the program...");
                            runProgram = false; //Set the loop condition to false to break out of it
                            continue; //Skip the rest of the loop
                        case 8:
                            Console.Clear();
                            ExportTasks(tasks);
                            break;
                        case 9:
                            Console.Clear();
                            tasks = ImportTask(tasks, temp);
                            break;
                        default:
                            Console.WriteLine("Invalid option! Try again"); ;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter a number");
                }

                Console.Write("\nEnter any key or ENTER to return");
                Console.ReadLine();
                Console.Clear();
            }

        }

        //Function for adding new tasks
        static string[] AddTask(string[] tasks, string[] temp)
        {
            int taskCount = tasks.Length;
            string addedTask = "";

            //Keep allowing users to add task until they enter "Quit"
            while (true)
            {
                //Break the loop immediately if the list is full
                if (taskCount >= 10)
                {
                    Console.WriteLine("Task list is full!");
                    break;
                }
                else
                {
                    Console.Write("Enter a task (enter 'quit' to exit): ");
                    addedTask = Console.ReadLine();
                    addedTask = addedTask.ToLower();

                    if (addedTask.Equals("quit"))
                    {
                        Console.WriteLine("Exiting...");
                        break;
                    }
                    else
                    {
                        if (addedTask.Equals(""))
                        {
                            Console.WriteLine("Invalid input! Task can not be empty");
                        }
                        else
                        {
                            temp[taskCount] = addedTask;
                            Console.WriteLine("New task added successfully!\n");
                            taskCount++;
                        }
                    }
                }
            }

            //Re-create the array with new size and assign new value of elements to it after getting all the added tasks
            tasks = new string[taskCount];
            for (int i = 0; i < taskCount; i++)
            {
                tasks[i] = temp[i];
            }

            return tasks;
        }

        //Function for displaying all tasks
        static void ViewAllTasks(string[] tasks)
        {
            int doneTaskCount = 0;

            if (tasks.Length == 0)
            {
                Console.WriteLine("No tasks yet!");
                Console.WriteLine("\nExiting...");
            }
            else
            {
                Console.WriteLine("Tasks List:");
                for (int i = 0; i < tasks.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {tasks[i]}");

                    if (tasks[i].Contains("[DONE]") == true)
                    {
                        doneTaskCount++;
                    }
                }

                Console.WriteLine($"\nTotal number of task: {tasks.Length}");
                //Count the number of complete and incomplete tasks (Bonus Challenge)
                Console.WriteLine($"Number of complete tasks: {doneTaskCount} | Number of incomplete tasks: {tasks.Length - doneTaskCount}");
            }
        }

        //Function for marking tasks as complete
        static void MarkTaskComplete(string[] tasks, string[] temp)
        {
            int taskNumber = 0;
            while (true)
            {
                ViewAllTasks(tasks); //Re-trigger view tasks everytime the loop runs to get the up to date data

                //Break the loop immediately if there is no task
                if (tasks.Length == 0)
                {
                    break;
                }

                Console.Write("\nEnter a task number to mark as DONE (enter 0 to exit): ");

                //Check integer parsing
                if (int.TryParse(Console.ReadLine(), out taskNumber) == true)
                {
                    if (taskNumber == 0)
                    {
                        Console.WriteLine("Exiting...");
                        break;
                    }
                    else
                    {
                        //Check to make sure the number is within the tasks range
                        if (taskNumber > 0 && taskNumber <= tasks.Length)
                        {
                            //Check if "[DONE]" is already in the string
                            if (tasks[taskNumber - 1].Contains("[DONE]") == true)
                            {
                                Console.WriteLine("Task is already completed!");
                            }
                            else
                            {
                                tasks[taskNumber - 1] = "[DONE] " + tasks[taskNumber - 1];
                                temp[taskNumber - 1] = "[DONE] " + temp[taskNumber - 1]; //Also change the temp array in case users add new task again
                                Console.WriteLine($"Task number {taskNumber} is marked as DONE successfully");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Invalid input! Task number {taskNumber} does not exist");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter a number");
                }

                Console.Write("\nEnter any key or ENTER to continue");
                Console.ReadLine();
                Console.Clear();
            }

        }

        //Function for deleting all tasks at once (Bonus Challenge)
        static string[] DeleteAllTasks(string[] tasks, string[] temp)
        {
            tasks = new string[0]; //Re-create the task array and return it

            //Set all existed element values to empty string for temp array
            for (int i = 0; i < tasks.Length; i++)
            {
                temp[i] = "";
            }

            Console.WriteLine("All tasks are deleted successfully!");

            return tasks;
        }

        //Function for deleting tasks
        static string[] DeleteTask(string[] tasks, string[] temp)
        {
            int taskCount = tasks.Length;
            int taskNumber = 0;

            while (true)
            {
                //Break the loop immediately if there is no task
                if (taskCount == 0)
                {
                    break;
                }
                else //Put else here to make sure no ViewAllTask duplication when the program goes back to Delete Menu
                {
                    ViewAllTasks(tasks); //Re-trigger view tasks everytime the loop runs to get the up to date data
                }

                Console.Write("\nEnter a task number for deleting (enter 0 to exit): ");

                //Check integer parsing
                if (int.TryParse(Console.ReadLine(), out taskNumber) == true)
                {
                    if (taskNumber == 0)
                    {
                        Console.WriteLine("Exiting...");
                        break;
                    }
                    else
                    {
                        //Check to make sure the number is within the tasks range
                        if (taskNumber > 0 && taskNumber <= taskCount)
                        {
                            for (int i = taskNumber - 1; i < temp.Length - 1; i++)
                            {
                                temp[i] = temp[i + 1];
                            }
                            taskCount--;
                            Console.WriteLine($"Task number {taskNumber} is deleted successfully");

                            //Re-create the array with new size and assign new value of elements to it everytime an element is deleted
                            tasks = new string[taskCount];
                            for (int i = 0; i < taskCount; i++)
                            {
                                tasks[i] = temp[i];
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Invalid input! Task number {taskNumber} does not exist");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter a number");
                }

                Console.Write("\nEnter any key or ENTER to continue");
                Console.ReadLine();
                Console.Clear();
            }

            return tasks;
        }

        //Function for handling delete tasks (all or one) (Bonus Challenge)
        static string[] HandleDeletionOptions(string[] tasks, string[] temp)
        {
            bool runMenu = true;
            int deleteOption = 0;

            while (runMenu == true)
            {
                //Break the loop immediately if there is no task
                if (tasks.Length == 0)
                {
                    ViewAllTasks(tasks);
                    break;
                }

                Console.WriteLine("====Deletion Options====");
                Console.WriteLine("1. Delete a task");
                Console.WriteLine("2. Delete all tasks");
                Console.Write("\nEnter an option number (enter 0 to eixt): ");

                if (int.TryParse(Console.ReadLine(), out deleteOption) == true)
                {
                    switch (deleteOption)
                    {
                        case 1:
                            Console.Clear();
                            tasks = DeleteTask(tasks, temp);
                            Console.Clear();
                            continue; //Skip the rest of the loop to prevent ReadLine() duplication
                        case 2:
                            tasks = DeleteAllTasks(tasks, temp);
                            runMenu = false;
                            continue; //Skip the rest of the loop to prevent ReadLine() duplication
                        case 0:
                            Console.WriteLine("Exiting...");
                            runMenu = false;
                            continue; //Skip the rest of the loop to prevent ReadLine() duplication
                        default:
                            Console.WriteLine("Invalid option! Try again");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter a number");
                }

                Console.Write("\nEnter any key or ENTER to continue");
                Console.ReadLine();
                Console.Clear();
            }

            return tasks;
        }

        //Function for modifying tasks (Bonus Challenge)
        static void ModifyTask(string[] tasks, string[] temp)
        {
            int taskNumber = 0;
            string taskModification = "";

            while (true)
            {
                ViewAllTasks(tasks);

                //Break the loop immediately if there is no task
                if (tasks.Length == 0)
                {
                    break;
                }

                Console.Write("Enter a task number for modifying (enter 0 to exit): ");

                if (int.TryParse(Console.ReadLine(), out taskNumber) == true)
                {
                    if (taskNumber == 0)
                    {
                        Console.WriteLine("Exiting...");
                        break;
                    }
                    else
                    {
                        if (taskNumber > 0 && taskNumber <= tasks.Length)
                        {
                            Console.WriteLine($"Task {taskNumber}: {tasks[taskNumber - 1]}");
                            Console.Write("Modification: ");
                            taskModification = Console.ReadLine();
                            tasks[taskNumber - 1] = taskModification;
                            temp[taskNumber - 1] = taskModification; //Modify the temp array too in case users want to add new task
                        }
                        else
                        {
                            Console.WriteLine($"Invalid input! Task number {taskNumber} does not exist");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter a number");
                }

                Console.Write("\nEnter any key or ENTER to continue");
                Console.ReadLine();
                Console.Clear();
            }
        }

        //Function for viewing incomplete tasks only
        static void ViewIncompleteTasks(string[] tasks)
        {
            int count = 0; //A variables to count how many incomplete tasks are there

            //Dont run the counting logic if there is no task
            if (tasks.Length == 0)
            {
                ViewAllTasks(tasks);
            }
            else
            {
                for (int i = 0; i < tasks.Length; i++)
                {
                    if (tasks[i].Contains("[DONE]") == false)
                    {
                        Console.WriteLine($"{i + 1}. {tasks[i]}");
                        count++;
                    }
                }

                //If there is no incomplete task, display all tasks completed
                if (count == 0)
                {
                    Console.WriteLine("All tasks completed! Great job");
                }
            }
        }

        //Function for exporting tasks into a file
        static void ExportTasks(string[] tasks)
        {
            string projectFolder = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName; //Get the current path
            string filePath = Path.Combine(projectFolder, "tasks.txt");

            File.WriteAllLines(filePath, tasks);

            Console.WriteLine("All tasks exported to tasks.txt successfully");
        }

        //Function for importing tasks from a file
        static string[] ImportTask(string[] tasks, string[] temp)
        {
            int taskCount = 0;
            string projectFolder = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName; //Get the current path
            string filePath = Path.Combine(projectFolder, "tasks.txt");

            //Using a loop to count the number of line in the file, prevent exceeding 10 tasks into the system
            foreach(string line in File.ReadLines(filePath))
            {
                if(taskCount >= 10)
                {
                    Console.WriteLine("File exceeds the limit of tasks! Only the first 10 tasks are imported");
                    break;
                }

                temp[taskCount] = line;
                taskCount++;
            }

            //Transfer tasks from temp array to tasks array
            tasks = new string[taskCount];
            for(int i = 0; i < taskCount; i++)
            {
                tasks[i] = temp[i];
            }

            Console.WriteLine("Tasks imported successfully");

            return tasks;
        }

        //Main
        static void Main(string[] args)
        {
            DisplayMenu();
        }
    }
}
