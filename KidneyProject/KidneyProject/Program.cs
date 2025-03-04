using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

internal class Program
{
    public class PatientList : LinkedList { }
    public class DonorList : LinkedList { }
    public static PatientList patientList = new PatientList();
    public static DonorList donorList = new DonorList();

    static void Main(string[] args)
    {
        Console.WriteLine("========================");
        Console.WriteLine("Kidney Management System");
        Console.WriteLine("========================");

        while (true)
        {
            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. Add a Patient");
            Console.WriteLine("2. Add a Donor");
            Console.WriteLine("3. Display All Patients");
            Console.WriteLine("4. Display All Donors");
            Console.WriteLine("5. Match Donor to Patient");
            Console.WriteLine("6. Delete a Patient");
            Console.WriteLine("7. Delete a Donor");
            Console.WriteLine("8. Exit");
            Console.Write("Enter Your Choice: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1": AddPatient(); break;
                case "2": AddDonor(); break;
                case "3": DisplayPatients(); break;
                case "4": DisplayDonors(); break;
                case "5": MatchDonor(); break;
                case "6": DeletePatient(); break;
                case "7": DeleteDonor(); break;
                case "8": ExitProgram(); break;
                default: Console.WriteLine("Invalid choice! Please try again."); break;
            }
        }
    }

    static void AddPatient()
    {
        Console.WriteLine("\n===== Add a Patient =====");
        var (id, name, bloodType, age, waitingTime) = GetUserInput(isDonor: false);
        patientList.Add(id, name, bloodType, age, waitingTime);
    }

    static void AddDonor()
    {
        Console.WriteLine("\n===== Add a Donor =====");
        var (id, name, bloodType, age, _) = GetUserInput(isDonor: true);
        donorList.Add(id, name, bloodType, age, 0);
    }

    static (string id, string name, string bloodType, int age, int waitingTime) GetUserInput(bool isDonor)
    {
        Console.Write("Enter ID: ");
        string id = Console.ReadLine()!;

        Console.Write("Enter Name: ");
        string name = Console.ReadLine()!;

        string bloodType = GetValidBloodType();
        int age = GetValidAge();

        int waitingTime;
        if (isDonor)
        {
            waitingTime = 0;
        }
        else
        {
            waitingTime = GetValidWaitingTime();
        }
        return (id, name, bloodType, age, waitingTime);
    }

  
    static string GetValidBloodType()
    {
        string[] validBloodTypes = { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
        while (true)
        {
            Console.Write("Enter Blood Type (A+, A-, B+, B-, AB+, AB-, O+, O-): ");
            string bloodType = Console.ReadLine()!;

            bool isValid = false;
            for (int i = 0; i < validBloodTypes.Length; i++)
            {
                if (validBloodTypes[i] == bloodType)
                {
                    isValid = true;
                    break;
                }
            }

            if (isValid)
            {
                return bloodType;
            }
            else
            {
                Console.WriteLine("Invalid blood type. Please enter a valid blood type.");
            }
        }
    }

    static int GetValidAge()
    {
        while (true)
        {
            Console.Write("Enter Age (0-100): ");
            if (int.TryParse(Console.ReadLine(), out int age) && age >= 0 && age <= 100)
            {
                return age;
            }
            Console.WriteLine("Invalid age. Please enter a number between 0 and 100.");
        }
    }

    static int GetValidWaitingTime()
    {
        while (true)
        {
            Console.Write("Enter Waiting Time (in days, 0-1000): ");
            if (int.TryParse(Console.ReadLine(), out int waitingTime) && waitingTime >= 0 && waitingTime <= 1000)
            {
                return waitingTime;
            }
            Console.WriteLine("Invalid waiting time. Please enter a number between 0 and 1000.");
        }
    }

    static void DisplayPatients()
    {
        patientList.DisplayPatients();
    }

    static void DisplayDonors()
    {
        donorList.DisplayDonors();
    }



    static void MatchDonor()
    {
        Console.WriteLine("\n===== Match Donor to Patient =====");
        Console.Write("Enter Donor ID: ");
        string donorId = Console.ReadLine()!;

        if (!donorList.Exists(donorId)) // Use the instance
        {
            Console.WriteLine("Donor not found!");
            return;
        }

        Node? donorNode = donorList.Find(donorId); // Use the instance
        if (donorNode == null)
        {
            Console.WriteLine("Donor not found!");
            return;
        }

        // Find patients with compatible blood types
        var allPatients = patientList.FindAllPatients(); // Use the instance
        var compatiblePatients = allPatients
            .Where(patient => BloodTypeCompatibility.CanDonateTo(donorNode.BloodType, patient.BloodType))
            .ToList();

        if (compatiblePatients.Count == 0)
        {
            Console.WriteLine("No compatible patients found for this donor.");
            return;
        }

        // Prompt the user to choose a sorting algorithm
        Console.WriteLine("\nChoose a Sorting Algorithm:");
        Console.WriteLine("1. Merge Sort");
        Console.WriteLine("2. Quick Sort");
        Console.WriteLine("3. Insertion Sort");
        Console.Write("Enter Your Choice: ");
        string? sortChoice = Console.ReadLine();

        List<Node> sortedPatients;
        Stopwatch stopwatch = new Stopwatch();

        switch (sortChoice)
        {
            case "1": // Merge Sort
                stopwatch.Start();
                sortedPatients = patientList.MergeSort(compatiblePatients);
                stopwatch.Stop();
                Console.WriteLine($"MergeSort took {stopwatch.ElapsedMilliseconds} ms");
                break;

            case "2": // Quick Sort
                stopwatch.Start();
                sortedPatients = patientList.QuickSort(compatiblePatients);
                stopwatch.Stop();
                Console.WriteLine($"QuickSort took {stopwatch.ElapsedMilliseconds} ms");
                break;

            case "3": // Insertion Sort
                stopwatch.Start();
                sortedPatients = patientList.InsertionSort(compatiblePatients);
                stopwatch.Stop();
                Console.WriteLine($"InsertionSort took {stopwatch.ElapsedMilliseconds} ms");
                break;

            default:
                Console.WriteLine("Invalid choice! Using Merge Sort by default.");
                stopwatch.Start();
                sortedPatients = patientList.MergeSort(compatiblePatients);
                stopwatch.Stop();
                Console.WriteLine($"MergeSort took {stopwatch.ElapsedMilliseconds} ms");
                break;
        }

        // Display the best match
        var bestMatch = sortedPatients.First();
        Console.WriteLine("\nBest Match Found:");
        Console.WriteLine($"Patient ID: {bestMatch.Id}, Name: {bestMatch.Name}, Blood Type: {bestMatch.BloodType}, Age: {bestMatch.Age}, Waiting Time: {bestMatch.WaitingTime} days");
    }

    static void DeletePatient()
    {
        Console.WriteLine("\n===== Delete a Patient =====");
        Console.Write("Enter Patient ID: ");
        string id = Console.ReadLine()!;
        if (patientList.Delete(id))
        {
            Console.WriteLine($"Patient {id} deleted successfully.");
        }
        else
        {
            Console.WriteLine($"Patient {id} not found!");
        }
    }
   

    static void DeleteDonor()
    {
        Console.WriteLine("\n===== Delete a Donor =====");
        Console.Write("Enter Donor ID: ");
        string id = Console.ReadLine()!;
        if (donorList.Delete(id))
        {
            Console.WriteLine($"Donor{id} deleted successfully!");
        }
        else
        {
            Console.WriteLine($"Donor {id} not found!");
        }
    }

    static void ExitProgram()
    {
        Console.WriteLine("Exiting the system. Have a good day!");
        Environment.Exit(0);
    }
}
