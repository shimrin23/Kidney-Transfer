using System;
using System.Collections.Generic;
using static Program;
using System.Diagnostics;

public class LinkedList
{
    protected Node? head;

    public bool Add(string id, string name, string bloodType, int age, int waitingTime = 0)
    {
        
        if (Exists(id))
        {
            Console.WriteLine($"Error: ID {id} already exists. Please enter a unique ID.");
            return false;  
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
    }

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


    //QuickSort
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


 // MergeSort Algorithm
    public List<Node> MergeSort(List<Node> patients)

    {
        if (patients.Count <= 1)
            return patients;

        int mid = patients.Count / 2;
        List<Node> left = patients.GetRange(0, mid);
        List<Node> right = patients.GetRange(mid, patients.Count - mid);

        left = MergeSort(left);
        right = MergeSort(right);

        return Merge(left, right);
    }

    private List<Node> Merge(List<Node> left, List<Node> right)
    {
        List<Node> result = new List<Node>();
        int i = 0, j = 0;

        while (i < left.Count && j < right.Count)
        {
            if (left[i].GetPriorityScore() <= right[j].GetPriorityScore())
                result.Add(left[i++]);
            else
                result.Add(right[j++]);
        }

        while (i < left.Count) result.Add(left[i++]);
        while (j < right.Count) result.Add(right[j++]);

        return result;
    }

    private static void Merge(List<Node> arr, int left, int mid, int right)
    {
        List<Node> leftArray = arr.GetRange(left, mid - left + 1);
        List<Node> rightArray = arr.GetRange(mid + 1, right - mid);

        int i = 0, j = 0, k = left;
        while (i < leftArray.Count && j < rightArray.Count)
        {
            if (leftArray[i].GetPriorityScore() <= rightArray[j].GetPriorityScore())
                arr[k++] = leftArray[i++];
            else
                arr[k++] = rightArray[j++];
        }

        while (i < leftArray.Count) arr[k++] = leftArray[i++];
        while (j < rightArray.Count) arr[k++] = rightArray[j++];
    }
    

 //InsertionSort Algorithm
    public List<Node> InsertionSort(List<Node> patients)
    {     
        // Start measuring sorting time
        

        for (int i = 1; i < patients.Count; i++)
        {
            Node currentPatient = patients[i];
            int j = i - 1;

            while (j >= 0 && patients[j].GetPriorityScore() > currentPatient.GetPriorityScore())
            {
                patients[j + 1] = patients[j];
                j--;
            }
            patients[j + 1] = currentPatient;
        }

        // Stop measuring sorting time
        
        return patients;
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
