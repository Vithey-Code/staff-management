namespace API.Constants;

public class ApplicationConstants
{
    public static class Message
    {
        public const string Recieved = "The record has been retrieved successfully.";
        public const string Disabled = "System does not allow disable your own account.";
        public const string Saved = "The resource was created successfully.";
        public const string Responsed = "The record has been responsed successfully.";
        public const string Failed = "Failed to save record";
        public const string Updated = "The record has been updated successfully.";
        public const string Deleted = "The record has been deleted successfully.";
        public const string NotFound = "The requested resource does not exist.";
        public const string Found = "The requested resource was found.";
        public const string birthdayValidate = "The birthday can not be same current date or bigger current date.";
        public const string Exists =
            "The record you are trying to create already exists. Please try again with a different value.";
    }
    
    
    public static class Cache
    {
        public const string GetAllStaffsCacheKey = "all-staffs";
    }
}