// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HospitalManagementSystem
{
    /* PROGRAMMED BY QQUYXT - GITHUB : @QQUYXT */
    internal class Program
    {
        static LinkedList<Patient> patients = new LinkedList<Patient>();
        static LinkedList<Donor> donors = new LinkedList<Donor>();

        static void Main(string[] args)
        {
            LoadData();
            MainMenu();
        }

        static void MainMenu()
        {
        menuTab:
            Console.Write(@"
|==========================|
|   KIDNEY MANAGEMENT SYS  |
|==========================|
|     :: MAIN MENU ::      |
|1. Add Patient            |
|2. Add Donor              |
|3. Match Donor            |
|4. Display All Patients   |
|5. Display All Donors     |
|6. Exit                   |
|==========================|");
            Console.WriteLine("\n");
            Console.Write("\nPlease Enter Your Selection: ");
            string? user_choice = Console.ReadLine();
            int.TryParse(user_choice, out int choice);

            switch (choice)
            {
                case 1:
                    AddPatient();
                    break;
                case 2:
                    AddDonor();
                    break;
                case 3:
                    MatchDonor();
                    break;
                case 4:
                    DisplayPatients();
                    break;
                case 5:
                    DisplayDonors();
                    break;
                case 6:
                    SaveData();
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("\nInvalid choice! Try again.");
                    break;
            }
            goto menuTab;
        }

        static void AddPatient()
        {
            Console.Write("Enter Patient ID: ");
            string id = Console.ReadLine();
            Console.Write("Enter Patient Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Blood Type: ");
            string bloodType = Console.ReadLine();
            Console.Write("Enter Age: ");
            int.TryParse(Console.ReadLine(), out int age);
            Console.Write("Enter Waiting Time (in days): ");
            int.TryParse(Console.ReadLine(), out int waitingTime);

            patients.AddLast(new Patient(id, name, bloodType, age, waitingTime));
            Console.WriteLine("Patient added successfully!\n");
        }

        static void AddDonor()
        {
            Console.Write("Enter Donor ID: ");
            string id = Console.ReadLine();
            Console.Write("Enter Donor Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Blood Type: ");
            string bloodType = Console.ReadLine();
            Console.Write("Enter Age: ");
            int.TryParse(Console.ReadLine(), out int age);

            donors.AddLast(new Donor(id, name, bloodType, age));
            Console.WriteLine("Donor added successfully!\n");
        }

        static void MatchDonor()
        {
            Console.Write("Enter Donor ID to Match: ");
            string donorId = Console.ReadLine();
            Donor donor = donors.FirstOrDefault(d => d.Id == donorId);
            if (donor == null)
            {
                Console.WriteLine("Donor not found!\n");
                return;
            }

            var sortedPatients = patients
                .Where(p => p.BloodType == donor.BloodType)
                .OrderBy(p => GetAgeCategoryDifference(p.Age, donor.Age))
                .ThenByDescending(p => p.WaitingTime);

            if (!sortedPatients.Any())
            {
                Console.WriteLine("No suitable patients found for this donor!\n");
                return;
            }

            var bestMatch = sortedPatients.First();
            Console.WriteLine("Best Match Found:\n" +
                              $"Patient ID: {bestMatch.Id}, Name: {bestMatch.Name}, Age: {bestMatch.Age}, Waiting Time: {bestMatch.WaitingTime} days\n");
        }

        static void DisplayPatients()
        {
            foreach (var patient in patients)
            {
                Console.WriteLine($"ID: {patient.Id}, Name: {patient.Name}, Blood Type: {patient.BloodType}, Age: {patient.Age}, Waiting Time: {patient.WaitingTime} days");
            }
        }

        static void DisplayDonors()
        {
            foreach (var donor in donors)
            {
                Console.WriteLine($"ID: {donor.Id}, Name: {donor.Name}, Blood Type: {donor.BloodType}, Age: {donor.Age}");
            }
        }

        static int GetAgeCategoryDifference(int age1, int age2)
        {
            return Math.Abs((age1 / 10) - (age2 / 10));
        }

        static void LoadData()
        {
            if (File.Exists("patients.csv"))
            {
                foreach (var line in File.ReadAllLines("patients.csv"))
                {
                    var data = line.Split(',');
                    patients.AddLast(new Patient(data[0], data[1], data[2], int.Parse(data[3]), int.Parse(data[4])));
                }
            }

            if (File.Exists("donors.csv"))
            {
                foreach (var line in File.ReadAllLines("donors.csv"))
                {
                    var data = line.Split(',');
                    donors.AddLast(new Donor(data[0], data[1], data[2], int.Parse(data[3])));
                }
            }
        }

        static void SaveData()
        {
            File.WriteAllLines("patients.csv", patients.Select(p => $"{p.Id},{p.Name},{p.BloodType},{p.Age},{p.WaitingTime}"));
            File.WriteAllLines("donors.csv", donors.Select(d => $"{d.Id},{d.Name},{d.BloodType},{d.Age}"));
        }
    }

    class Patient
    {
        public string Id { get; }
        public string Name { get; }
        public string BloodType { get; }
        public int Age { get; }
        public int WaitingTime { get; }

        public Patient(string id, string name, string bloodType, int age, int waitingTime)
        {
            Id = id;
            Name = name;
            BloodType = bloodType;
            Age = age;
            WaitingTime = waitingTime;
        }
    }

    class Donor
    {
        public string Id { get; }
        public string Name { get; }
        public string BloodType { get; }
        public int Age { get; }

        public Donor(string id, string name, string bloodType, int age)
        {
            Id = id;
            Name = name;
            BloodType = bloodType;
            Age = age;
        }
    }
}
