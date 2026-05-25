using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Xml.Linq;

namespace MP2__LIBRARY_
{
    namespace MP2__LIBRARY_
    {
        internal class Program
        {


            static void Main(string[] args)
            {
                MainMenu(); // program starts here
            }

            //--------------------------------------------------------------------------------------------------------------------

            static void Header()
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("                        ");
                Console.WriteLine("                     .-'---'-.");
                Console.WriteLine("                    /         \\");
                Console.WriteLine("                   |    ^   ^  |");
                Console.WriteLine("                    \\    -    /");
                Console.WriteLine("                     `-.___.-'");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("╔══════════════════════════════════════════════════╗");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("║              THE EGGCELLENT LIBRARY              ║");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("╚══════════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.ResetColor();



                Console.WriteLine();
            }

            //--------------------------------------------------------------------------------------------------------------------

            static void MainMenu()
            {
                while (true) // infinite loop until user exits
                {
                    Console.Clear();
                    Header();

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("\n-------             MAIN MENU                -------");
                    Console.ResetColor();

                    Console.WriteLine("╔══════════════════════════════════════════════════╗");
                    Console.WriteLine("║                   1. Login                       ║");
                    Console.WriteLine("║                   2. Exit                        ║");
                    Console.WriteLine("╚══════════════════════════════════════════════════╝");
                    Console.Write("\n>>> Select option (1/2): ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Login(); //jumps to the login method
                            break;
                        case "2":
                            return; // quits :(

                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid choice! Please type in a valid number.");
                            Console.ResetColor();
                            Console.ReadKey();
                            break;
                    }
                }
            }

            //--------------------------------------------------------------------------------------------------------------------


            //--------------------------------------------------------------------------------------------------------------------

            static void Login()
            {
                Console.Clear();
                Header();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("   Note: Your Name and Password is Case Sensitive!\n");
                Console.ResetColor();


                Console.WriteLine("════════════════════════════════════════════════════");
                Console.Write("Name:   ");
                string name = Console.ReadLine();

                Console.Write("Role:   ");
                string role = Console.ReadLine().ToUpper(); //caps it so that it can be typed in any way and it will still work (student, STUDENT, sTuDeNt, etc) (i keep forgetting this)

                Console.Write("Password:  ");
                string pass = Console.ReadLine();
                Console.WriteLine("════════════════════════════════════════════════════");


                bool found = false; // basically this is false at first then becomes true if the user logs in correctly (it goes to the if (found) part) and if it stays false it goes to the else part which is invalid credentials

                foreach (string line in File.ReadAllLines("list2.txt")) //checks the txt file exists, if it doesnt it says no users registered yet and RETURNS back to the main menu 
                {
                    string[] data = line.Split(',');

                    if (data.Length == 3 &&
                        data[0] == name &&
                        data[1] == role &&
                        data[2] == pass) //basically means this line has exactly 3 parts separated by commas // zak, student, 123 // sumth like that and if those parts match the input of the user then it founds the user and breaks out of the loop
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Login successful!");
                    Console.ResetColor();
                    Console.ReadKey();

                    // directs user depending on role
                    if (role == "STUDENT")
                    {
                        STUDENTLIBRARY(name); //if they put in the role of student tehy get directed to the method of studentlibrary
                    }
                    else
                    {
                        LIBRARIANLIBRARY(name); //if they put in the role of librarian they get directed to the method of librarianlibrary
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid credentials.");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }

            static int GetValidNumber(string message, int min, int max) // helper method to safely get a number input from the user within a specified range (used for menu selections)
            {
                int value;
                while (true)
                {
                    Console.Write(message);
                    string input = Console.ReadLine(); //

                    if (int.TryParse(input, out value) && value >= min && value <= max) // TryParse to prevent crashes from invalid input, and OUT is how it gives the converted number
                                                                                        // also checks if the number is within the specified range (so like GetValidNumber("Choose option: ", 1, 5); THATS the min and max so it only accepts numbers between 1 and 5) 
                    {
                        return value; // if it's valid it returns the number and exits the loop, if not it shows an error message and asks again
                    }

                    Console.WriteLine($"Invalid! Enter a number between {min} and {max}.");

                }
            }

            //--------------------------------------------------------------------------------------------------------------------



            //-----------------------------------------
            // BOOK DATA (parallel lists = same index = same book)
            //-----------------------------------------
            static List<string> bookTitles = new List<string> // list of book titles
        {
            "Cool Book",
            "How to 5F",
            "That time I became a Ghost for a few months",
            "The Very Hungry Caterpillar",
            "The RamRam Guide",
            "The TRUE Minecraft Guide",
            "Zestfulness",
            "Being a Patriot"


        };

            static List<string> bookAuthors = new List<string> // parallel list to bookTitles
        {
            "Marco Reniva",
            "Gian Topacio",
            "Shan Quiambao",
            "Eric Carle",
            "Aldin Osena",
            "Riyannah Santos",
            "Keeyan Segismundo",
            "Amrisse Tagoc, Ronald Alano, Zak Guevarra, and Tyrone Maruzzo"


        };

            static List<bool> bookBorrowed = new List<bool> // true = borrowed, false = available
        {
            false, false, false, false, false, false, false, false
        };

            static List<string> bookBorrower = new List<string>
        {
            null, null, null, null, null, null, null, null // who borrowed it
        };

            //-----------------------------------------
            // REQUEST DATA (each index = one request)
            //-----------------------------------------
            static List<string> reqStudent = new List<string>(); // who requested it, this is parallel to reqBook and reqStatus so they all have the same index for the same request (so like reqStudent[0] requested reqBook[0] and its status is reqStatus[0])!!!!
            static List<string> reqBook = new List<string>(); // what book was requested
            static List<string> reqStatus = new List<string>();
            // status = Pending / Approved / Declined / Returned

            static Queue<int> borrowQueue = new Queue<int>(); //que for the borrowing of books
            static Queue<int> roomQueue = new Queue<int>(); //que for study room
            static Queue<int> reqNums = new Queue<int>();




            //--------------------------------------------------------------------------------------------------------------------



            //--------------------------------------------------------------------------------------------------------------------
            // STUDENT SIDE  
            //--------------------------------------------------------------------------------------------------------------------


            static void STUDENTLIBRARY(string name)
            {
                while (true) // infinite loop until student logs out
                {
                    Console.Clear();
                    Header();

                    Console.WriteLine("        Welcome " + name + "! You signed up as STUDENT!");
                    Console.WriteLine("════════════════════════════════════════════════════");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\n1. View Books");
                    Console.Write("\n2. Borrow Book");
                    Console.Write("\n3. My Requests");
                    Console.Write("\n4. Return Book");
                    Console.Write("\n5. Search Books");
                    Console.Write("\n6. View Books (Sorted)");
                    Console.Write("\n7. View Books (Sorted with Bubble Sort)");
                    Console.Write("\n8. Book a seat in Room A");
                    Console.Write("\n9. Logout\n"); // goes back home!
                    Console.ResetColor();

                    Console.WriteLine("\n════════════════════════════════════════════════════");

                    Console.Write("\n>>> Select option: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            ViewBooks();
                            break;
                        case "2":
                            BorrowBook(name);
                            break;
                        case "3":
                            ViewRequests(name);
                            break;
                        case "4":
                            ReturnBook(name);
                            break;
                        case "5":
                            SearchBooks();
                            break;
                        case "6":
                            ViewBooksSorted();
                            break;
                        case "7":
                            BubbleSortBooks();
                            break;
                        case "8":
                            StudentStudy(name);
                            break;
                        case "9":
                            return;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid choice! Please type in a valid number.");
                            Console.ResetColor();
                            Console.ReadKey();
                            break;
                    }
                }
            }

            //--------------------------------------------------------------------------------------------------------------------

            static void ViewBooks() // shows all available books (borrowed = no show)
            {
                Console.Clear();
                Header();

                Console.WriteLine("════════════════════════════════════════════════════");

                // loops throooo books
                for (int i = 0; i < bookTitles.Count; i++)
                {
                    if (!bookBorrowed[i]) // only show available
                        Console.WriteLine($"{i + 1}. {bookTitles[i]} - {bookAuthors[i]}"); //shows smth like "awesome book - zak"
                }

                Console.WriteLine("════════════════════════════════════════════════════");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nPress any button to return!");
                Console.ResetColor();
                Console.ReadKey();
            }

            //--------------------------------------------------------------------------------------------------------------------

            static void BorrowBook(string student) // shows available books, lets student select, and creates a pending request, 
                                                   //NEEDS QUEUE, if student borrows a book it needs to be stored in a queue
            {
                Console.Clear();
                Header();

                Console.WriteLine("Would you like to borrow a book?");
                string borrow;

                //ASKING IF YOU WANT 2 BORROW
                while (true)
                {
                    Console.Write("\n>>> Select option(Y/N): ");
                    borrow = Console.ReadLine().ToUpper();
                    if (borrow == "Y")
                    {
                        break;
                    }
                    if (borrow == "N")
                    {
                        return;
                    }
                    Console.WriteLine("Please type Y or N.");
                }

                while (true) //basically loops until u wanna get out with "2" and returns to the student menu OR goes back out if youve alr chosen a book
                {
                    Console.Clear();
                    Header();

                    List<int> available = new List<int>(); // stores real indices of available books
                    int display = 1;

                    Console.WriteLine("════════════════════════════════════════════════════");
                    Console.WriteLine("   Available Books:\n");

                    // SHOW BOOKS
                    for (int i = 0; i < bookTitles.Count; i++) //goes thru the books in the list
                    {
                        if (!bookBorrowed[i]) // only show books that are not taken yet
                        {
                            Console.WriteLine($"{display}. {bookTitles[i]} - {bookAuthors[i]}");
                            available.Add(i); //stores teh actual index of each book
                            display++;
                        }
                    }

                    Console.WriteLine("════════════════════════════════════════════════════");

                    // NO BOOKS
                    if (available.Count == 0) // no books left to show
                    {
                        Console.WriteLine("\nNo available books.");
                        Console.ReadKey();
                        return;
                    }

                    int choice = GetValidNumber("\nSelect book: ", 1, available.Count);
                    int realIndex = available[choice - 1]; // converts menu number back to real book index

                    // CHECK IF ALREADY REQUESTED (ONLY ONCE)
                    for (int i = 0; i < reqStudent.Count; i++)
                    {
                        if (reqStudent[i] == student && reqBook[i] == bookTitles[realIndex] && reqStatus[i] != "Returned" && reqStatus[i] != "Declined") //ask if reqstudent = student and reqbook = booktitles, and if its finished or not (returned or devlined
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You've already requested this book!");
                            Console.ResetColor();
                            Console.ReadKey();
                            return; //goes back to student menu knowing u alr borrowed said book
                        }
                    }

                    // ADD REQUEST
                    reqStudent.Add(student); // saves who requested it
                    reqBook.Add(bookTitles[realIndex]); // saves what book was requested
                    reqStatus.Add("Pending"); //requesting js starts as waiting

                    int reqIndex = reqStudent.Count - 1;

                    borrowQueue.Enqueue(reqIndex); // does the fifo n adds request into queue

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Request added! You are #{borrowQueue.Count} in queue."); //shows line position!
                    Console.ResetColor();
                    break;
                }
            }




            static void ViewRequests(string student)
            {
                Console.Clear();
                Header();
                Console.WriteLine("════════════════════════════════════════════════════");

                Console.WriteLine("Your Current Queue: \n");
                // shows only that student's requests
                int display = 1; //js for numbers
                for (int i = 0; i < reqStudent.Count; i++)
                {
                    if (reqStudent[i] == student)
                        Console.WriteLine($"{display}. {reqBook[i]} - {reqStatus[i]}");
                    display++;
                }

                Console.WriteLine("════════════════════════════════════════════════════");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nPress any button to return!");
                Console.ResetColor();

                Console.ReadKey();
            }

            //--------------------------------------------------------------------------------------------------------------------

            static void ReturnBook(string student) // check if the student has any approved books, show only those, and then process the return
            {
                Console.Clear();
                Header();

                // Checkz if the student ACTUALLY has any approved books to return
                bool hasApprovedBooks = false;
                for (int i = 0; i < reqStudent.Count; i++)
                {
                    if (reqStudent[i] == student && reqStatus[i] == "Approved")
                    {
                        hasApprovedBooks = true;
                        break;
                    }
                }

                // If no books found, tell the user and js go out
                if (!hasApprovedBooks)
                {
                    Console.WriteLine("════════════════════════════════════════════════════");

                    Console.WriteLine("You have no approved books to return at this time.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nPress any button to return!");
                    Console.ResetColor();

                    Console.WriteLine("════════════════════════════════════════════════════");

                    Console.ReadKey();
                    return;
                }

                // they HAVE books. Show them:
                Console.WriteLine("--- Your Approved Books ---");
                for (int i = 0; i < reqStudent.Count; i++)
                {
                    if (reqStudent[i] == student && reqStatus[i] == "Approved")
                    {
                        Console.WriteLine($"{i + 1}. {reqBook[i]}");
                    }
                }

                // Use your safety helper instead of int.Parse
                Console.WriteLine("════════════════════════════════════════════════════");
                int choice = GetValidNumber("\nSelect the number to return: ", 1, reqStudent.Count) - 1; // minus 1 to convert back to 0-based index

                // Verify the selection belongs to the student before processing
                if (reqStudent[choice] == student && reqStatus[choice] == "Approved")
                {
                    for (int i = 0; i < bookTitles.Count; i++)
                    {
                        if (bookTitles[i] == reqBook[choice]) // finds the book in the library list that matches the request
                        {
                            bookBorrowed[i] = false; // marks it as available again
                            bookBorrower[i] = null; // clears the borrower info
                            break;
                        }
                    }

                    reqStatus[choice] = "Returned";
                    Console.WriteLine("════════════════════════════════════════════════════");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nSuccessfully returned: {reqBook[choice]}");
                    Console.ResetColor();



                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid selection. That request is not an approved book."); // this shouldn't happen if the user inputs correctly but just in case
                    Console.ResetColor();
                }

                Console.WriteLine("\nPress any key...");
                Console.ReadKey();
            }



            static void StudentStudy(string student)
            {
                Console.Clear();
                Header();



                Console.WriteLine("════════════════════════════════════════════════════");
                Console.WriteLine("Would you like to reserve a seat?");
                string borrow;

                //ASKING IF YOU WANT 2 BORROW
                while (true)
                {
                    Console.Write("\nRoom Occupancy: " + roomQueue.Count);
                    Console.Write("\n>>> Select option(Y/N): ");
                    borrow = Console.ReadLine().ToUpper();
                    if (borrow == "Y")
                    {
                        break;
                    }
                    if (borrow == "N")
                    {
                        return; //goes back to student menu
                    }
                    Console.WriteLine("Please type Y or N.");
                }

                if (roomQueue.Count == 7)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The room queue is currently full!");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }

                for (int i = 0; i < reqStudent.Count; i++)
                {
                    if (reqStudent[i] == student) //ask if reqstudent = student
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nYou've already queued before!");
                        Console.ResetColor();
                        Console.ReadKey();
                        return; //goes back to student menu knowing u alr borrowed said book
                    }
                }

                while (true)
                {
                    reqStudent.Add(student); // saves who requested to join

                    int reqIndex = reqStudent.Count - 1;

                    roomQueue.Enqueue(reqIndex); // does the fifo n adds request into queue

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Request added! You are #{roomQueue.Count} in queue."); //shows line position!
                    Console.ResetColor();
                    return;
                }


            }























            //--------------------------------------------------------------------------------------------------------------------
            // LIBRARIAN SIDE
            //--------------------------------------------------------------------------------------------------------------------

            static void LIBRARIANLIBRARY(string name)
            {
                while (true)
                {
                    Console.Clear();
                    Header();

                    Console.WriteLine("        Welcome " + name + "! You logged in as LIBRARIAN!");
                    Console.WriteLine("════════════════════════════════════════════════════");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("1. Add Book");
                    Console.Write("\n2. View Books");
                    Console.Write("\n3. Requests");
                    Console.Write("\n4. Process Request");
                    Console.Write("\n5. Register Student/Librarian");
                    Console.Write("\n6. Search Books");
                    Console.Write("\n7. View Books (Sorted)");
                    Console.Write("\n8. Bubble Sort Books");
                    Console.Write("\n9. Room Request");
                    Console.Write("\n10. Logout\n");
                    Console.ResetColor();

                    Console.WriteLine("════════════════════════════════════════════════════");

                    Console.Write("\n>>> Select option: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddBook();
                            break;
                        case "2":
                            ViewBooks();
                            break;
                        case "3":
                            ViewQueue(); //they need to peek or dequeue the person
                            break;
                        case "4":
                            Process();
                            break;
                        case "5":
                            Register();
                            break;
                        case "6":
                            SearchBooks();
                            break;
                        case "7":
                            ViewBooksSorted();
                            break;
                        case "8":
                            BubbleSortBooks();
                            break;
                        case "9":
                            LibrarianStudy();
                            break;
                        case "10":
                            return;
                        default:
                            Console.WriteLine("Invalid choice! Please type in a valid number.");
                            Console.ReadKey();
                            break;
                    }
                }
            }

            //--------------------------------------------------------------------------------------------------------------------

            static void AddBook()
            {
                Console.Clear();
                Header();

                Console.WriteLine("════════════════════════════════════════════════════");
                Console.Write("Add Book Title: ");
                string titlez = Console.ReadLine();

                Console.Write("Add Author Name:");
                string author = Console.ReadLine();
                Console.WriteLine("════════════════════════════════════════════════════");


                // addz new book to all lists
                bookTitles.Add(titlez); // adds the new book's title  to the respective lists
                bookAuthors.Add(author); // adds the new book's author to the respective lists
                bookBorrowed.Add(false); // new book is available by default so false
                bookBorrower.Add(null); // no borrower yet

                Console.ReadKey();
            }

            //--------------------------------------------------------------------------------------------------------------------

            static void Register()
            {
                Console.Clear();
                Header();

                Console.Write("Enter name: ");
                string name = Console.ReadLine();

                string role;
                while (true) // keeps asking until valid
                {
                    Console.Write("Role (Student/Librarian): ");
                    role = Console.ReadLine().ToUpper(); //caps it so that it can be typed in any way and it will still work (student, STUDENT, sTuDeNt, etc) (i keep forgetting this) 

                    if (role == "STUDENT" || role == "LIBRARIAN")
                        break; //finally jumps to enter password if the role is valid

                    Console.WriteLine("Invalid role.");
                }

                Console.Write("Password: ");
                string pass = Console.ReadLine();

                // CHECKS IF THERE IS DUPES
                if (File.Exists("list2.txt"))
                {
                    foreach (string line in File.ReadAllLines("list2.txt")) //if the files exists it goes through each line and checks if the name, role, and pass are the same as the one being registere
                    {
                        string[] data = line.Split(',');

                        if (data.Length == 3 && data[0] == name) //basically means this line has exactly 3 parts separated by commas zak, student, 123 sumth like that
                        {
                            Console.WriteLine("User already exists!");
                            Console.ReadKey();
                            return;
                        }
                    }
                }

                // saves user into file
                File.AppendAllText("list2.txt", name + "," + role + "," + pass + "\n"); //THE CREATION OF THE TXT FILEEEE and added a line of the new user 

                Console.WriteLine("Registered!");
                Console.ReadKey();
            }

            //--------------------------------------------------------------------------------------------------------------------------

            static void ViewQueue()
            {
                Console.Clear();
                Header();

                if (borrowQueue.Count == 0)
                {
                    Console.WriteLine("Queue is empty.");
                    Console.ReadKey();
                    return;
                }

                int pos = 1;

                foreach (int i in borrowQueue)
                {
                    Console.WriteLine($"{pos}. {reqStudent[i]} -> {reqBook[i]} ({reqStatus[i]})");
                    pos++;
                }

                Console.ReadKey();
            }


            //--------------------------------------------------------------------------------------------------------------------------------

            static void Process() //4
            {
                Console.Clear();
                Header();

                // checks if queue is empty
                if (borrowQueue.Count == 0)
                {
                    Console.WriteLine("No requests to process.");
                    Console.ReadKey();
                    return;
                }

                // gets the FIRST person in queue only (FIFO)
                int choice = borrowQueue.Peek();

                Console.WriteLine("════════════════════════════════════════════════════");
                Console.WriteLine("Current Request:\n");

                Console.WriteLine($"Student: {reqStudent[choice]}");
                Console.WriteLine($"Book: {reqBook[choice]}");
                Console.WriteLine($"Status: {reqStatus[choice]}");

                Console.WriteLine("════════════════════════════════════════════════════");

                Console.Write("\nAction: [A]pprove or [D]ecline: ");
                string libdecision = Console.ReadLine().ToUpper();

                if (libdecision == "A")
                {
                    reqStatus[choice] = "Approved";

                    for (int i = 0; i < bookTitles.Count; i++)
                    {
                        if (bookTitles[i] == reqBook[choice]) // finds the book in the library list that matches the request
                        {
                            bookBorrowed[i] = true; // marks it as borrowed
                            bookBorrower[i] = reqStudent[choice]; // saves who borrowed it
                            break;
                        }
                    }

                    borrowQueue.Dequeue(); // removes FIRST person from queue after processing

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Approved!");
                    Console.ResetColor();
                }

                else if (libdecision == "D")
                {
                    reqStatus[choice] = "Declined";

                    borrowQueue.Dequeue(); // removes FIRST person from queue after processing

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Declined.");
                    Console.ResetColor();
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid action.");
                    Console.ResetColor();
                }

                Console.ReadKey();
            }


            static void SearchBooks()
            {
                Console.Clear();
                Header();

                Console.WriteLine("Search Books");
                Console.WriteLine("════════════════════════════════════════════════════");
                Console.Write("Enter keyword (title or author): ");
                string keyword = Console.ReadLine().ToLower();

                bool found = false;

                Console.WriteLine("\nResults:");
                Console.WriteLine("════════════════════════════════════════════════════");

                for (int i = 0; i < bookTitles.Count; i++)
                {
                    if (bookTitles[i].ToLower().Contains(keyword) ||
                        bookAuthors[i].ToLower().Contains(keyword)) //finds either the title name or author name for the user
                    {
                        string status;
                        if (bookBorrowed[i] == true)
                        {
                            status = "Borrowed";
                        }
                        else
                        {
                            status = "Available";
                        }
                        Console.WriteLine($"{bookTitles[i]} - {bookAuthors[i]} ({status})");
                        found = true;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("No matching books found.");
                }

                Console.WriteLine("════════════════════════════════════════════════════");
                Console.ReadKey();
            }

            //--------------------------------------------------------------------------------------------------------------------------------

            static void ViewBooksSorted() //alphabetically sorted
            {
                Console.Clear();
                Header();

                Console.WriteLine("Books (Sorted by Title)");
                Console.WriteLine("════════════════════════════════════════════════════");

                List<string> availableBooks = new List<string>();
                int pos = 1;

                // collect available books
                for (int i = 0; i < bookTitles.Count; i++)
                {
                    if (!bookBorrowed[i])
                    {
                        availableBooks.Add(bookTitles[i] + " - " + bookAuthors[i]);

                    }
                }

                // sort directly (A-Z)
                availableBooks.Sort();

                foreach (string book in availableBooks)
                {
                    Console.WriteLine($"{pos}. {book}");
                    pos++;
                }

                Console.WriteLine("════════════════════════════════════════════════════");
                Console.ReadKey();
            }

            //--------------------------------------------------------------------------------------------------------------------------------


            static void BubbleSortBooks() //sorts the original view books method into alphabetical form
            {
                // outer loop = how many passes we do (keeps repeating until sorted)
                for (int i = 0; i < bookTitles.Count - 1; i++)
                {
                    // inner loop = compares each pair of books
                    for (int j = 0; j < bookTitles.Count - i - 1; j++)
                    {
                        // Compare current book with the next book alphabetically
                        if (bookTitles[j].CompareTo(bookTitles[j + 1]) > 0)
                        {
                            // if current book comes AFTER next book, swap them

                            // swap titles
                            string tempTitle = bookTitles[j];
                            bookTitles[j] = bookTitles[j + 1];
                            bookTitles[j + 1] = tempTitle;

                            // swap authors (so they stay matched with titles)
                            string tempAuthor = bookAuthors[j];
                            bookAuthors[j] = bookAuthors[j + 1];
                            bookAuthors[j + 1] = tempAuthor;

                            // swap borrowed status (important so data doesn't get messed up)
                            bool tempBorrowed = bookBorrowed[j];
                            bookBorrowed[j] = bookBorrowed[j + 1];
                            bookBorrowed[j + 1] = tempBorrowed;

                            // swap borrower name (who borrowed the book)
                            string tempBorrower = bookBorrower[j];
                            bookBorrower[j] = bookBorrower[j + 1];
                            bookBorrower[j + 1] = tempBorrower;
                        }
                    }
                }

                // after sorting is done, show message to user
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Books are now sorted using Bubble Sort!");
                Console.ResetColor();

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }

            //--------------------------------------------------------------------------------------------------------------------------------

            private static void LibrarianStudy()
            {
                Console.Clear();
                Header();
                if (roomQueue.Count == 0)
                {
                    Console.WriteLine("No room requests to process.");
                    Console.ReadKey();
                    return;
                }

                int choice = roomQueue.Peek();
                Console.WriteLine("════════════════════════════════════════════════════");
                Console.Write("\nWaiting Student: " + reqStudent[choice]);

                Console.Write("\n\nNumber of Students initially in Room Queue: " + reqStudent.Count()); //so librarian knows number and when to know when to decline
                Console.Write("\nStudents Currently in Room Queue: " + roomQueue.Count());

                Console.Write("\n\nNumber of Students in Room: " + reqNums.Count());

                Console.WriteLine("\n════════════════════════════════════════════════════");
                Console.Write("\nAction: [A]pprove or [D]ecline: ");
                string libdecision = Console.ReadLine().ToUpper();

                if (libdecision == "A")
                {
                    int reqIndex = reqStudent.Count - 1;
                    if (reqNums.Count == 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("The Room is Now Full!");
                        Console.ResetColor();
                        Console.Write("\nEnter 1 to Return Back to Main Menu: ");
                        string input = Console.ReadLine();

                        switch (input)
                        {
                            case "1":
                                return;
                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid choice! Please type in a valid number.");
                                Console.ResetColor();
                                Console.ReadKey();
                                break;
                        }
                    }
                    else
                    {
                        reqNums.Enqueue(reqIndex);
                        roomQueue.Dequeue();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Approved!");
                        Console.ResetColor();
                    }


                }
                else if (libdecision == "D")
                {
                    roomQueue.Dequeue();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Declined.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid action.");
                    Console.ResetColor();
                }
                Console.ReadKey();

            }
        }
    }
}