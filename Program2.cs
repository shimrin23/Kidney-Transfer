using System;
using System.Collections.Generic;
using System.IO;

namespace HospitalManagementSystem
{
    internal class Program
    {
        // Data structures for patients and donors
        static LinkedList<Patient> patients = new LinkedList<Patient>();
        static LinkedList<Donor> donors = new LinkedList<Donor>();

        static void Main(string[] args)
        {
            LoadData();

            while (true)
            {
                Console.WriteLine("\n==============================");
                Console.WriteLine("   Hospital Management Menu");
                Console.WriteLine("==============================");
                Console.WriteLine("1. Add Patient");
                Console.WriteLine("2. Add Donor");
                Console.WriteLine("3. Match Donor to Patient");
                Console.WriteLine("4. View All Patients");
                Console.WriteLine("5. View All Donors");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddPatient();
                        break;
                    case "2":
                        AddDonor();
                        break;
                    case "3":
                        MatchDonorToPatient();
                        break;
                    case "4":
                        ViewAllPatients();
                        break;
                    case "5":
                        ViewAllDonors();
                        break;
                    case "6":
                        SaveData();
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void AddPatient()
        {
            Console.Write("Enter Patient Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Blood Type: ");
            string bloodType = Console.ReadLine();

            Console.Write("Enter Age: ");
            int age = int.Parse(Console.ReadLine());

            Console.Write("Enter Waiting Time (in days): ");
            int waitingTime = int.Parse(Console.ReadLine());

            patients.AddLast(new Patient(name, bloodType, age, waitingTime));
            Console.WriteLine("Patient added successfully.");
        }

        static void AddDonor()
        {
            Console.Write("Enter Donor Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Blood Type: ");
            string bloodType = Console.ReadLine();

            Console.Write("Enter Age: ");
            int age = int.Parse(Console.ReadLine());

            donors.AddLast(new Donor(name, bloodType, age));
            Console.WriteLine("Donor added successfully.");
        }

        static void MatchDonorToPatient()
        {
            if (donors.Count == 0 || patients.Count == 0)
            {
                Console.WriteLine("No donors or patients available for matching.");
                return;
            }

            Console.Write("Enter Donor Name: ");
            string donorName = Console.ReadLine();

            Donor donor = null;
            foreach (var d in donors)
            {
                if (d.Name.Equals(donorName, StringComparison.OrdinalIgnoreCase))
                {
                    donor = d;
                    break;
                }
            }

            if (donor == null)
            {
                Console.WriteLine("Donor not found.");
                return;
            }

            Patient bestMatch = null;
            foreach (var patient in patients)
            {
                if (patient.BloodType.Equals(donor.BloodType, StringComparison.OrdinalIgnoreCase))
                {
                    if (bestMatch == null ||
                        Math.Abs(patient.Age - donor.Age) < Math.Abs(bestMatch.Age - donor.Age) ||
                        (Math.Abs(patient.Age - donor.Age) == Math.Abs(bestMatch.Age - donor.Age) &&
                         patient.WaitingTime > bestMatch.WaitingTime))
                    {
                        bestMatch = patient;
                    }
                }
            }

            if (bestMatch != null)
            {
                Console.WriteLine($"Best match for donor {donor.Name} is patient {bestMatch.Name} ({bestMatch.BloodType}, Age {bestMatch.Age}, Waiting Time {bestMatch.WaitingTime} days).");
            }
            else
            {
                Console.WriteLine("No suitable patient found for this donor.");
            }
        }

        static void ViewAllPatients()
        {
            Console.WriteLine("\nAll Patients:");
            foreach (var patient in patients)
            {
                Console.WriteLine(patient);
            }
        }

        static void ViewAllDonors()
        {
            Console.WriteLine("\nAll Donors:");
            foreach (var donor in donors)
            {
                Console.WriteLine(donor);
            }
        }

        static void SaveData()
        {
            using (StreamWriter sw = new StreamWriter("patients.csv"))
            {
                foreach (var patient in patients)
                {
                    sw.WriteLine(patient.ToCsv());
                }
            }

            using (StreamWriter sw = new StreamWriter("donors.csv"))
            {
                foreach (var donor in donors)
                {
                    sw.WriteLine(donor.ToCsv());
                }
            }
        }

        static void LoadData()
        {
            if (File.Exists("patients.csv"))
            {
                foreach (var line in File.ReadAllLines("patients.csv"))
                {
                    try
                    {
                        patients.AddLast(Patient.FromCsv(line));
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Error parsing patient data: {ex.Message}");
                    }
                }
            }

            if (File.Exists("donors.csv"))
            {
                foreach (var line in File.ReadAllLines("donors.csv"))
                {
                    try
                    {
                        donors.AddLast(Donor.FromCsv(line));
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Error parsing donor data: {ex.Message}");
                    }
                }
            }
        }

        class Patient
        {
            public string Name { get; }
            public string BloodType { get; }
            public int Age { get; }
            public int WaitingTime { get; }

            public Patient(string name, string bloodType, int age, int waitingTime)
            {
                Name = name;
                BloodType = bloodType;
                Age = age;
                WaitingTime = waitingTime;
            }

            public string ToCsv() => $"{Name},{BloodType},{Age},{WaitingTime}";

            public static Patient FromCsv(string csv)
            {
                var parts = csv.Split(',');
                if (parts.Length != 4)
                {
                    throw new FormatException("Invalid CSV format for Patient.");
                }

                if (!int.TryParse(parts[2], out int age))
                {
                    throw new FormatException($"Invalid Age: '{parts[2]}' could not be parsed as an integer.");
                }

                if (!int.TryParse(parts[3], out int waitingTime))
                {
                    throw new FormatException($"Invalid WaitingTime: '{parts[3]}' could not be parsed as an integer.");
                }

                return new Patient(parts[0], parts[1], age, waitingTime);
            }

            public override string ToString() => $"Name: {Name}, Blood Type: {BloodType}, Age: {Age}, Waiting Time: {WaitingTime} days";
        }

        class Donor
        {
            public string Name { get; }
            public string BloodType { get; }
            public int Age { get; }

            public Donor(string name, string bloodType, int age)
            {
                Name = name;
                BloodType = bloodType;
                Age = age;
            }

            public string ToCsv() => $"{Name},{BloodType},{Age}";

            public static Donor FromCsv(string csv)
            {
                var parts = csv.Split(',');
                if (parts.Length != 3)
                {
                    throw new FormatException("Invalid CSV format for Donor.");
                }

                if (!int.TryParse(parts[2], out int age))
                {
                    throw new FormatException($"Invalid Age: '{parts[2]}' could not be parsed as an integer.");
                }

                return new Donor(parts[0], parts[1], age);
            }

            public override string ToString() => $"Name: {Name}, Blood Type: {BloodType}, Age: {Age}";
        }
    }
}
