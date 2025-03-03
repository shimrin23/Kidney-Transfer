using System;
using System.Collections.Generic;
using static Program;

public class LinkedList
{
    protected Node? head;

    public bool Add(string id, string name, string bloodType, int age, int waitingTime = 0)
    {
        // Check if ID already exists before adding
        if (Exists(id))
        {
            Console.WriteLine($"Error: ID {id} already exists. Please enter a unique ID.");
            return false;  // Prevent duplicate entry
        }

        Node newNode = new Node(id, name, bloodType, age, waitingTime);

        if (head == null)
        {
            head = newNode;
        }
        else
        {
            Node? current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = newNode;
        }
        Console.WriteLine();
        Console.WriteLine($"ID {id} added successfully!");
        return true;  

    public bool Exists(string id)
    {
        Node? current = head;
        while (current != null)
        {
            if (current.Id == id) return true;
            current = current.Next;
        }
        return false;
    }

    public Node? Find(string id)
    {
        Node? current = head;
        while (current != null)
        {
            if (current.Id == id) return current;
            current = current.Next;
        }
        return null;
    }

    public List<Node> FindAllPatients()
    {
        List<Node> allPatients = new List<Node>();
        Node? current = head;
        while (current != null)
        {
            allPatients.Add(current);
            current = current.Next;
        }
        return allPatients;
    }

    public List<Node> QuickSort(List<Node> patients)
    {
        if (patients.Count <= 1)
            return patients;

        var pivot = patients[0];
        var left = new List<Node>();
        var right = new List<Node>();

        for (int i = 1; i < patients.Count; i++)
        {
            if (patients[i].GetPriorityScore() <= pivot.GetPriorityScore())
            {
                left.Add(patients[i]);
            }
            else
            {
                right.Add(patients[i]);
            }
        }

        left = QuickSort(left);
        right = QuickSort(right);

        var result = new List<Node>(left);
        result.Add(pivot);
        result.AddRange(right);

        return result;
    }

    public bool Delete(string id)
    {
        if (head == null)
        {
            Console.WriteLine("There is nothing to delete.");
            return false;
        }

        if (head.Id == id)
        {
            head = head.Next;
            //Console.WriteLine($"Patient/Donor {id} deleted successfully.");
            return true;
        }

        Node? current = head;
        while (current.Next != null && current.Next.Id != id)
        {
            current = current.Next;
        }

        if (current.Next == null)
        {
           // Console.WriteLine($"Patient/Donor {id} not found.");
            return false;
        }

        current.Next = current.Next.Next;
        Console.WriteLine($"Patient/Donor {id} deleted successfully.");
        return true;
    }

   
        public void DisplayPatients()
        {
            Console.WriteLine("=========================");
            Console.WriteLine(" DISPLAYING ALL PATIENTS ");
            Console.WriteLine("=========================");

            if (head == null)
            {
                Console.WriteLine("No patients to display.");
                return;
            }

            Node? current = head;
            while (current != null)
            {
                Console.WriteLine($"ID: {current.Id}, Name: {current.Name}, Blood Type: {current.BloodType}, Age: {current.Age}, Waiting Time: {current.WaitingTime} days");
                current = current.Next;
            }
        }

        public void DisplayDonors()
        {
            Console.WriteLine("========================");
            Console.WriteLine(" DISPLAYING ALL DONORS ");
            Console.WriteLine("========================");

            if (head == null)
            {
                Console.WriteLine("No donors to display.");
                return;
            }

            Node? current = head;
            while (current != null)
            {
                Console.WriteLine($"ID: {current.Id}, Name: {current.Name}, Blood Type: {current.BloodType}, Age: {current.Age}");
                current = current.Next;
            }
        }
    }
