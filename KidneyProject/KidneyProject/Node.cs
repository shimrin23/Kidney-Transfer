public class Node
{
    public string Id { get; }
    public string Name { get; }
    public string BloodType { get; }
    public int Age { get; }
    public int WaitingTime { get; }
    public Node? Next { get; set; }

    public Node(string id, string name, string bloodType, int age, int waitingTime)
    {
        Id = id;
        Name = name;
        BloodType = bloodType;
        Age = age;
        WaitingTime = waitingTime;
        Next = null;
    }

    public int GetPriorityScore()
    {
        int agePriority;

        if (Age <= 25)
        {
            agePriority = 1;
        }
        else if (Age <= 55)
        {
            agePriority = 2;
        }
        else
        {
            agePriority = 3;
        }
        return agePriority * 1000 - WaitingTime;
    }
}
