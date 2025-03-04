public static class BloodTypeCompatibility
{
    public static bool CanDonateTo(string donorBloodType, string recipientBloodType)
    {
        switch (donorBloodType)
        {
            case "A+":
                return recipientBloodType == "A+" || recipientBloodType == "AB+";
            case "A-":
                return recipientBloodType == "A+" || recipientBloodType == "A-" || recipientBloodType == "AB+" || recipientBloodType == "AB-";
            case "B+":
                return recipientBloodType == "B+" || recipientBloodType == "AB+";
            case "B-":
                return recipientBloodType == "B+" || recipientBloodType == "B-" || recipientBloodType == "AB+" || recipientBloodType == "AB-";
            case "AB+":
                return recipientBloodType == "AB+";
            case "AB-":
                return recipientBloodType == "AB+" || recipientBloodType == "AB-";
            case "O+":
                return recipientBloodType == "O+" || recipientBloodType == "A+" || recipientBloodType == "B+" || recipientBloodType == "AB+";
            case "O-":
                return true;
            default:
                return false;
        }
    }
}