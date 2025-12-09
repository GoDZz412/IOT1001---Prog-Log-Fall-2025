// See https://aka.ms/new-console-template for more information
using System;
using System.Data;

namespace Applied_Activity_3
{
    class BillPayment
    {
        //Function for displaying all the invoices
        static void DisplayInvoices(decimal finalFees, decimal sharedFees, int numberOfPeople, string[] sharedBillNames)
        {
            int i = 0;
            while (i < numberOfPeople)
            {
                Console.Write($"Enter the name of person {i + 1} who is involved: ");
                string name = Console.ReadLine();

                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Invalid input! The name can not be empty");
                }
                else
                {
                    sharedBillNames[i] = name;
                    i++;
                }
            }

            Console.WriteLine("\nInvoices:\n");

            for (int j = 0; j < numberOfPeople; j++)
            {
                Console.WriteLine(sharedBillNames[j]);
                Console.WriteLine($"Total amount: ${finalFees}");
                Console.WriteLine($"Split into {numberOfPeople} people - Shared amount: ${sharedFees}\n");
            }
        }

        //Function to generate invoice as a .txt file
        static void GenerateInvoices(decimal finalFees, decimal sharedFees, int numberOfPeople, string[] sharedBillNames)
        {
            string projectFolder = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName; //Get the current path

            for (int i = 0; i < numberOfPeople; i++)
            {
                string filePath = Path.Combine(projectFolder, sharedBillNames[i] + ".txt"); //Path "../Applied Activity 3/<file>"
                File.WriteAllText(filePath, sharedBillNames[i] + "\n"
                                            + $"Total amount: ${finalFees}\n"
                                            + $"Split into {numberOfPeople} people - Shared amount: ${sharedFees}\n");
            }

            Console.WriteLine("Invoices generated successfully!");
        }

        //Main
        static void Main(string[] args)
        {
            //Declare tipsPercentage, billFees, finalFees and numberOfPeople
            decimal tipsPercentage;
            decimal billFees;
            decimal finalFees;
            decimal sharedFees;
            int numberOfPeople;
            string[] sharedBillNames; //Additional array for storing people names

            //Enter the value for bill fees
            Console.Write("Enter the bill amount: ");
            string billFeesStr = Console.ReadLine(); //A temporary string variable for storing the input;

            //Check if billFees is a number
            if (decimal.TryParse(billFeesStr, out billFees) == true)
            {
                //Check if billFees is greater or equal than 0
                if (billFees >= 0)
                {
                    //If billFees is greater or equal than 0, input the tipsPercentage
                    Console.Write("Enter the tips percentage: ");
                    string tipsPercentageStr = Console.ReadLine(); //A temporary string variable for storing the input

                    //Check if tipsPercentage is a number
                    if (decimal.TryParse(tipsPercentageStr, out tipsPercentage) == true)
                    {
                        //Check if tipsPercentage is greater or equal than 0
                        if (tipsPercentage >= 0)
                        {
                            //If the tipsPercentage is greater or equal than 0, input the numberOfPeople
                            Console.Write("Enter the number of people: ");
                            string numberOfPeopleStr = Console.ReadLine();

                            //Check if numberOfPeople is a number
                            if (int.TryParse(numberOfPeopleStr, out numberOfPeople) == true)
                            {
                                //Check if the numberOfPeople is greater than 0
                                if (numberOfPeople > 0)
                                {
                                    //If the numberOfPeople, billFees and tipsPercentage has been checked properly, calculate the finalFees and output
                                    finalFees = billFees + (billFees * tipsPercentage / 100);
                                    sharedFees = finalFees / (decimal)numberOfPeople;
                                    Console.WriteLine($"You should leave a tip of ${billFees * tipsPercentage / 100}");
                                    Console.WriteLine($"Total amount to pay: ${finalFees}");
                                    Console.WriteLine($"Each person share is: ${sharedFees}");

                                    sharedBillNames = new string[numberOfPeople];

                                    DisplayInvoices(finalFees, sharedFees, numberOfPeople, sharedBillNames);

                                    Console.Write("Do you want to generate the invoices? (yes/no): ");
                                    string choice = Console.ReadLine();
                                    choice = choice.ToLower();

                                    if (choice == "y" || choice == "yes")
                                    {
                                        GenerateInvoices(finalFees, sharedFees, numberOfPeople, sharedBillNames);
                                    }
                                    else
                                    {
                                        Console.WriteLine("No invoice generated! Exting...");
                                    }
                                }
                                else
                                {
                                    //If numberOfPeople is less or equal than 0, prompt an error and terminate the program
                                    Console.WriteLine("Error: Inavlid number of people! Please enter a number greater than 0");
                                }
                            }
                            else
                            {
                                //If numberOfPeople is not a number, prompt an error and terminate the program
                                Console.WriteLine("Error: Invalid number of people! Please enter a number");
                            }
                        }
                        else
                        {
                            //If the tipsPercentage is less than 0, prompt an error and terminate the program
                            Console.WriteLine("Error: Inavlid tips percentage value! Please enter a positive number");
                        }
                    }
                    else
                    {
                        //If tipsPercentage is not a number, prompt an error and terminate the program
                        Console.WriteLine("Error: Invalid tips percentage value! Please enter a number");
                    }
                }
                else
                {
                    //If billFees is less than 0, prompt an error and terminate the program
                    Console.WriteLine("Error: Invalid bill fees value! Please enter a positive value");
                }
            }
            else
            {
                //If billFees is not a number, prompt an error and terminate the program
                Console.WriteLine("Error: Invalid bill fees! Please enter a number");
            }
        }
    }
}
