// See https://aka.ms/new-console-template for more information
using System;
using System.ComponentModel.Design;
using System.Xml.Serialization;
namespace Applied_Activity_5
{
    class Program
    {
        //Global Variables
        static string playerName;
        static int playerHealth = 100;
        static bool hasKey = false;
        static bool hasTorch = false;
        static bool treasureFound = false;
        static string[] inventory = new string[3];
        static int itemCount = 0;
        static bool catPow = false;

        //Function to log status into a csv file
        static void LogStatus()
        {
            string projectFolder = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName; //Get the current path
            string filePath = Path.Combine(projectFolder, playerName + ".csv");
            string items = string.Join(",", inventory);
            string[] status = {$"Player Health,{playerHealth}",
                               $"Has Key,{hasKey}",
                               $"Has Torch,{hasTorch}",
                               $"Treasure Found,{treasureFound}",
                               $"Number of Item,{itemCount}",
                               $"Inventory,{items}"};

            File.WriteAllLines(filePath, status);

            Console.WriteLine("Status Logged successfully!");
        }

        //Function to shuffle array at the start of each scene, making sure the pattern is not the same every replay (Please comment the function in each scene if you dont want to try)
        static void ShuffleEventArray(string[] eventArray)
        {
            Random randomEvent = new Random();
            for (int i = eventArray.Length - 1; i > 0; i--)
            {
                int j = randomEvent.Next(0, i + 1);
                string temp = eventArray[i];
                eventArray[i] = eventArray[j];
                eventArray[j] = temp;
            }
        }

        //Every event has a tag to describe its purpose, this function is to handle those tags
        static void HandleEventTags(string events, int damge, string key)
        {
            if (events.Contains("[DMG]"))
            {
                Console.WriteLine($"You lost {damge} hp");
                playerHealth -= damge;
            }
            else if (events.Contains("[LOS]"))
            {
                playerHealth = 0;
            }
            else if (events.Contains("[KEY]"))
            {
                if (itemCount <= 3)
                {
                    Console.WriteLine($"The {key} added to your inventory");
                    hasKey = true;
                    if (events.Contains("TORCH"))
                    {
                        hasTorch = true;
                    }
                    inventory[itemCount] = key;
                    itemCount++;
                }
                else
                {
                    Console.WriteLine("Inventory is full, can not add more item");
                }
            }
            else if (events.Contains("[HP]"))
            {
                if (itemCount <= 3)
                {
                    Console.WriteLine("Health Potion added to inventory");
                    inventory[itemCount] = "Health Potion";
                    itemCount++;
                }
                else
                {
                    Console.WriteLine("Inventory is full, can not add more item");
                }
            }
            else if (events.Contains("[WIN]"))
            {
                treasureFound = true;
                playerHealth = 0;
            }
            else if (events.Contains("[SEC]"))
            {
                bool verify = false;
                for (int i = 0; i < itemCount; i++)
                {
                    if (inventory[i] == "The Holy Bell")
                    {
                        verify = true;
                        break;
                    }
                }

                if (verify)
                {
                    Console.Write("Press enter to continue... ");
                    Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("You have The Torch and The Holy Bell at the same time. Entering the Hidden Twin Waterfall...");
                    HiddenScene_TwinWaterfall();
                }
            }
            else if (events.Contains("[TRE]"))
            {
                treasureFound = true;
                playerHealth = 0;
            }
            else
            {
                Console.WriteLine("Be brave and choose another path Adventurer!");
            }
        }

        //Function for displaying status
        static void DisplayStatus()
        {
            Console.WriteLine("====================================");
            Console.WriteLine($"Player Name: {playerName}");
            Console.WriteLine($"HP: {playerHealth}");
            Console.WriteLine("Inventory items:");

            if (itemCount == 0)
            {
                Console.WriteLine("No item in inventory yet!");
            }
            else
            {
                for (int i = 0; i < itemCount; i++)
                {
                    Console.WriteLine($"\tItem {i + 1}: {inventory[i]}");
                }
            }

            Console.WriteLine("====================================");
        }

        //Function for starting the game, explaining the rules and getting player name (The game truly starts here)
        static void StartGame()
        {
            Console.WriteLine("======================================");
            Console.WriteLine("Welcome to the Lost Island Adventurer");
            Console.WriteLine("======================================");
            Console.WriteLine("Rules of the game:");
            Console.WriteLine("1. The game events are shuffled every replay, do not expect to have the same pattern everytime");
            Console.WriteLine("2. Enter a number of your choice, DO NOT enter text");
            Console.WriteLine("3. There will be events that will make you lose or win the game IMMEDIATELY! Be aware");
            Console.WriteLine("4. You will lose the game if your health drop to or below 0, or you encounter a special event");
            Console.WriteLine("5. You will need a special 'key' to access the next area, try to find it");
            Console.WriteLine("6. ENJOY YOUR JOURNEY ADVENTURER");

            Console.Write("\nEnter your player name to start the game: ");
            playerName = Console.ReadLine();
            Console.Clear();

            Scene1_Beach();
        }

        //Function for using items in inventory
        static void UseItem()
        {
            int itemChoice;

            while (true)
            {
                Console.Write("Enter the number of the item you wan to use (0 to exit): ");

                if (int.TryParse(Console.ReadLine(), out itemChoice))
                {
                    if (itemChoice == 0)
                    {
                        break;
                    }
                    else
                    {
                        if (itemChoice <= itemCount)
                        {
                            if (inventory[itemChoice - 1].Equals("Health Potion"))
                            {
                                if (playerHealth == 100)
                                {
                                    Console.WriteLine("Player health is full, can not recover more! Item dispended");
                                }
                                else
                                {
                                    Console.WriteLine("Health Potion used! Player get 10hp back");
                                    playerHealth += 10;
                                }
                            }
                            else if (inventory[itemChoice - 1].Equals("Hello Kitty"))
                            {
                                Console.WriteLine("Grant self the power of Hello Kitty! Get ready to explore the truth");
                                catPow = true;
                            }
                            else
                            {
                                Console.WriteLine("This item can not be used directly!");
                                continue;
                            }

                            for (int i = itemChoice - 1; i < itemCount - 1; i++)
                            {
                                inventory[i] = inventory[i + 1];
                            }
                            itemCount--;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid input! Item number {itemChoice} does not exist");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter an integer");
                }
            }
        }

        //Function for using item automatically
        static void AutoUseItem(string itemName)
        {
            int index = Array.IndexOf(inventory, itemName);

            for (int i = index; i < itemCount - 1; i++)
            {
                inventory[i] = inventory[i + 1];
            }
            itemCount--;

            Console.WriteLine($"Item {itemName} used!");
        }

        //Scene 1: Beach
        static void Scene1_Beach()
        {
            string[] events = {"[DMG]The sun is magnificient, tanned too hard",
                               "[DMG]The ocean is so breathtaking, forgot to breath",
                               "[LOS]Being charmed by a mermaid - Have fun at the bottom of the ocean adventurer!",
                               "[KEY]Found the SMILING BRANCH! You can now access The Jungle"}; //Array saves events and tags
            int choice;
            bool runEvent = true;

            //ShuffleEventArray(events);

            while (runEvent)
            {
                DisplayStatus();
                Console.WriteLine("\n********** Area: The Beach **********");
                Console.WriteLine("A beautiful beach, should take some rest eh?");
                Console.WriteLine("\n1. Explore the beach");
                Console.WriteLine("2. Go around the shore");
                Console.WriteLine("3. Build a sand castle");
                Console.WriteLine("4. Go to a nearby cliff");
                Console.WriteLine("5. Use an item in the inventory");
                Console.WriteLine("6. Head to The Jungle");
                Console.WriteLine("7. Log Status");
                Console.Write("\nEnter your choice (enter 0 to restart the game): ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            string ev = events[choice - 1].Substring(events[choice - 1].IndexOf(']') + 1);
                            Console.WriteLine(ev);
                            HandleEventTags(events[choice - 1], 5, "Smiling Branch");
                            events[choice - 1] = "You have been through this path!";
                            break;
                        case 5:
                            UseItem();
                            break;
                        case 6:
                            if (hasKey)
                            {
                                AutoUseItem("Smiling Branch");
                                Console.WriteLine("Entering The Jungle...");
                                Console.Write("Press enter to continue... ");
                                Console.ReadLine();
                                Console.Clear();
                                hasKey = false;
                                Scene2_Jungle();
                                runEvent = false;
                            }
                            else
                            {
                                Console.WriteLine("You must find a SMILING BRANCH to enter this area");
                                Console.Write("Press enter to continue... ");
                                Console.ReadLine();
                                Console.Clear();
                            }

                            continue;
                        case 7:
                            LogStatus();
                            break;
                        case 0:
                            Console.WriteLine("Exiting the game...");
                            runEvent = false;
                            break;
                        default:
                            Console.WriteLine($"Invalid input! Option number {choice} does not exist");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter an integer");
                }

                if (playerHealth <= 0)
                {
                    EndGame();
                    runEvent = false;
                }

                Console.Write("Press enter to continue... ");
                Console.ReadLine();
                Console.Clear();
            }
        }

        //Scene 2: Jungle
        static void Scene2_Jungle()
        {
            string[] events = {"[DMG]Walk through a group of trees, hit by a leaf",
                               "[DMG]There is a fell tree, stepped on branch",
                               "[HP]Received a HEALTH POTION from a monkey, it seems to want your banana",
                               "[KEY]Found the MIST WALKER! You can now enter The Village"};
            int choice;
            bool runEvent = true;

            //ShuffleEventArray(events);

            while (runEvent)
            {
                DisplayStatus();
                Console.WriteLine("A huge jungle, easy to get lost! Be careful");
                Console.WriteLine("\n********** Area: The Jungle **********");
                Console.WriteLine("1. Go toward the North");
                Console.WriteLine("2. Go toward the South");
                Console.WriteLine("3. Go toward the East");
                Console.WriteLine("4. Go toward the West");
                Console.WriteLine("5. Use an item in the inventory");
                Console.WriteLine("6. Head to The Village");
                Console.WriteLine("7. Log Status");
                Console.Write("\nEnter your choice (enter 0 to restart the game): ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            string ev = events[choice - 1].Substring(events[choice - 1].IndexOf(']') + 1);
                            Console.WriteLine(ev);
                            HandleEventTags(events[choice - 1], 5, "Mist Walker");
                            events[choice - 1] = "You have been through this path!";
                            break;
                        case 5:
                            UseItem();
                            break;
                        case 6:
                            if (hasKey)
                            {
                                AutoUseItem("Mist Walker");
                                Console.WriteLine("Entering The Village...");
                                Console.Write("Press enter to continue... ");
                                Console.ReadLine();
                                Console.Clear();
                                hasKey = false;
                                Scene3_Village();
                                runEvent = false;
                            }
                            else
                            {
                                Console.WriteLine("You must find a MIST WALKER to enter this area");
                                Console.Write("Press enter to continue... ");
                                Console.ReadLine();
                                Console.Clear();
                            }

                            continue;
                        case 7:
                            LogStatus();
                            break;
                        case 0:
                            Console.WriteLine("Exiting the game...");
                            runEvent = false;
                            break;
                        default:
                            Console.WriteLine($"Invalid input! Option number {choice} does not exist");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter an integer");
                }

                if (playerHealth <= 0)
                {
                    EndGame();
                    runEvent = false;
                }

                Console.Write("Press enter to continue... ");
                Console.ReadLine();
                Console.Clear();
            }
        }

        //Scene 3: Village
        static void Scene3_Village()
        {
            string[] events = {"[DMG]The farmers do not like you, being chased by them",
                               "[DMG]Everyone in the village are hard-working, feeling like a failure",
                               "[WIN]Meet the love of your life - You realize that she is the most precious treasure you could ever ask for. HAPPY MARRIAGE!",
                               "[KEY]Found THE TORCH! You can now enter The Cave"};
            int choice;
            bool runEvent = true;

            //ShuffleEventArray(events);

            while (runEvent)
            {
                DisplayStatus();
                Console.WriteLine("\n********** Area: The Village **********");
                Console.WriteLine("A peaceful village with dedicated farmers. Look at them and stop being lazy");
                Console.WriteLine("\n1. Go toward the farms");
                Console.WriteLine("2. Go toward the piggery");
                Console.WriteLine("3. Go toward the houses");
                Console.WriteLine("4. Go toward the lake");
                Console.WriteLine("5. Use an item in the inventory");
                Console.WriteLine("6. Head to The Cave");
                Console.WriteLine("7. Log Status");
                Console.Write("\nEnter your choice (enter 0 to restart the game): ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            string ev = events[choice - 1].Substring(events[choice - 1].IndexOf(']') + 1);
                            Console.WriteLine(ev);
                            HandleEventTags(events[choice - 1], 10, "The Torch");
                            events[choice - 1] = "You have been through this path!";
                            break;
                        case 5:
                            UseItem();
                            break;
                        case 6:
                            if (hasTorch)
                            {
                                Console.WriteLine("Entering The Cave...");
                                Console.Write("Press enter to continue... ");
                                Console.ReadLine();
                                Console.Clear();
                                hasKey = false;
                                Scene4_Cave();
                                runEvent = false;
                            }
                            else
                            {
                                Console.WriteLine("You must find THE TORCH to enter this area");
                                Console.Write("Press enter to continue... ");
                                Console.ReadLine();
                                Console.Clear();
                            }

                            continue;
                        case 7:
                            LogStatus();
                            break;
                        case 0:
                            Console.WriteLine("Exiting the game...");
                            runEvent = false;
                            break;
                        default:
                            Console.WriteLine($"Invalid input! Option number {choice} does not exist");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter an integer");
                }

                if (playerHealth <= 0)
                {
                    EndGame();
                    runEvent = false;
                }

                Console.Write("Press enter to continue... ");
                Console.ReadLine();
                Console.Clear();
            }
        }

        //Scene 4: The Cave
        static void Scene4_Cave()
        {
            string[] events = {"[DMG]Your crush is kissing your best friend! EMOTIONAL DAMGE",
                               "[DMG]Yousef is marking your exam! You feel scared",
                               "[SEC]Nothing happens...",
                               "[KEY]Found the HOLY BELL! You can now enter The Fountain"};
            int choice;
            bool runEvent = true;

            //ShuffleEventArray(events);

            while (runEvent)
            {
                DisplayStatus();
                Console.WriteLine("\n********** Area: The Cave **********");
                Console.WriteLine("A dark cave with multiple doorways, what secret could be hidden in here...");
                Console.WriteLine("\n1. Go toward the frist door");
                Console.WriteLine("2. Go toward the second door");
                Console.WriteLine("3. Go toward the third door");
                Console.WriteLine("4. Go toward the fourth door");
                Console.WriteLine("5. Use an item in the inventory");
                Console.WriteLine("6. Head to The Fountain");
                Console.WriteLine("7. Log Status");
                Console.Write("\nEnter your choice (enter 0 to restart the game): ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            string ev = events[choice - 1].Substring(events[choice - 1].IndexOf(']') + 1);
                            Console.WriteLine(ev);
                            HandleEventTags(events[choice - 1], 20, "The Holy Bell");
                            events[choice - 1] = "You have been through this path!";
                            break;
                        case 5:
                            UseItem();
                            break;
                        case 6:
                            if (hasTorch && hasKey)
                            {
                                AutoUseItem("The Holy Bell");
                                Console.WriteLine("Entering The Fountain...");
                                Console.Write("Press enter to continue... ");
                                Console.ReadLine();
                                Console.Clear();
                                hasKey = false;
                                Scene5_Fountain();
                                runEvent = false;
                            }
                            else
                            {
                                Console.WriteLine("You must find THE HOLY BELL to enter this area");
                                Console.Write("Press enter to continue... ");
                                Console.ReadLine();
                                Console.Clear();
                            }

                            continue;
                        case 7:
                            LogStatus();
                            break;
                        case 0:
                            Console.WriteLine("Exiting the game...");
                            runEvent = false;
                            break;
                        default:
                            Console.WriteLine($"Invalid input! Option number {choice} does not exist");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter an integer");
                }

                if (playerHealth <= 0)
                {
                    EndGame();
                    runEvent = false;
                }

                Console.Write("Press enter to continue... ");
                Console.ReadLine();
                Console.Clear();
            }
        }

        //Scene 5: The Fountain
        static void Scene5_Fountain()
        {
            string[] events = {"[DMG]Oops, a Mimic. Fight it to gain access to the treasure",
                               "[TRE]Treasure Found!! Congratulation"};
            int choice;
            bool runEvent = true;

            //ShuffleEventArray(events);

            while (runEvent)
            {
                DisplayStatus();
                Console.WriteLine("\n********** Area: The Fountain **********");
                Console.WriteLine("A Holy Fountain. The treasure should be in a palace near here, but it is not easy to earn for sure");
                Console.WriteLine("\n1. Go toward the frist chest");
                Console.WriteLine("2. Go toward the second chest");
                Console.WriteLine("3. Use an item in the inventory");
                Console.WriteLine("4. Log Status");
                Console.Write("\nEnter your choice (enter 0 to restart the game): ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                        case 2:
                            string ev = events[choice - 1].Substring(events[choice - 1].IndexOf(']') + 1);
                            Console.WriteLine(ev);
                            HandleEventTags(events[choice - 1], 40, "");
                            events[choice - 1] = "You have been through this path!";
                            if (playerHealth > 0)
                            {
                                treasureFound = true;
                                EndGame();
                                runEvent = false;
                                continue;
                            }
                            break;
                        case 3:
                            UseItem();
                            break;
                        case 4:
                            LogStatus();
                            break;
                        case 0:
                            Console.WriteLine("Exiting the game...");
                            runEvent = false;
                            break;
                        default:
                            Console.WriteLine($"Invalid input! Option number {choice} does not exist");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter an integer");
                }

                if (playerHealth <= 0)
                {
                    EndGame();
                    runEvent = false;
                }

                Console.Write("Press enter to continue... ");
                Console.ReadLine();
                Console.Clear();
            }
        }

        //Scene 6: Hidden Twin Waterfall
        static void HiddenScene_TwinWaterfall()
        {
            string[] events = {"[LOS]You were tricked by the MeowMeow and can not resist the cuteness of it - Strenghthen your mind and good luck next time adventurer",
                               "[KEY]You have found the secret Hello Kitty potion!"};
            int choice;
            bool runEvent = true;

            //ShuffleEventArray(events);

            while (runEvent)
            {
                DisplayStatus();
                Console.WriteLine("\n********** Area: The Twin Waterfall **********");
                Console.WriteLine("Once upon a time, this place has been ocupied by a twin of gods with the shape of cat. Legends say there is a power that could reveal the truth of the world here");
                Console.WriteLine("\n1. Go toward the frist waterfall");
                Console.WriteLine("2. Go toward the second waterfall");
                Console.WriteLine("3. Head to The Fountain");
                Console.WriteLine("4. Use an item in the inventory");
                Console.WriteLine("5. Log Status");
                Console.Write("\nEnter your choice (enter 0 to restart the game): ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                        case 2:
                            string ev = events[choice - 1].Substring(events[choice - 1].IndexOf(']') + 1);
                            Console.WriteLine(ev);
                            HandleEventTags(events[choice - 1], 20, "Hello Kitty");
                            events[choice - 1] = "You have been through this path!";
                            if (events[choice - 1].Contains("[SEC]"))
                            {
                                continue;
                            }
                            break;
                        case 4:
                            UseItem();
                            break;
                        case 3:
                            if (hasTorch && hasKey)
                            {
                                AutoUseItem("The Holy Bell");
                                Console.WriteLine("Entering The Fountain...");
                                Console.Write("Press enter to continue... ");
                                Console.ReadLine();
                                Console.Clear();
                                hasKey = false;
                                Scene5_Fountain();
                                runEvent = false;
                            }
                            else
                            {
                                Console.WriteLine("You must find THE HOLY BELL to enter this area");
                                Console.Write("Press enter to continue... ");
                                Console.ReadLine();
                                Console.Clear();
                            }
                            continue;
                        case 5:
                            LogStatus();
                            break;
                        case 0:
                            Console.WriteLine("Exiting the game...");
                            runEvent = false;
                            break;
                        default:
                            Console.WriteLine($"Invalid input! Option number {choice} does not exist");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter an integer");
                }

                if (playerHealth <= 0)
                {
                    EndGame();
                    runEvent = false;
                }

                Console.Write("Press enter to continue... ");
                Console.ReadLine();
                Console.Clear();
            }
        }

        //End Game
        static void EndGame()
        {
            Console.WriteLine();
            if (treasureFound)
            {
                if (catPow)
                {
                    Console.WriteLine("You wake up and realize that there is no way you got a nerve to go on such an adventure, eveything was just a dream. You found the true ending!");
                }
                else
                {
                    Console.WriteLine("You found the treasure! Congratulation upon winning the game");
                }
            }
            else
            {
                Console.WriteLine("Your journey has come to an end. You lost!");
            }
        }

        static void Main(string[] args)
        {
            bool playAgain = true;

            while (playAgain)
            {
                //Reset player status
                playerHealth = 100;
                hasKey = false;
                hasTorch = false;
                treasureFound = false;
                catPow = false;
                itemCount = 0;
                inventory = new string[3];

                //Game flow: Start -> Beach -> Jungle -> Village -> Cave -> Waterfall (Optional) -> Fountain
                StartGame();

                Console.Write("\nDo you want to replay the game? (yes/no): ");
                string choice = Console.ReadLine().ToLower();
                if (choice == "no" || choice == "n")
                {
                    Console.WriteLine("Thank you for playing! Exiting the game...");
                    playAgain = false;
                    continue;
                }

                Console.Write("Press enter to continue... ");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
