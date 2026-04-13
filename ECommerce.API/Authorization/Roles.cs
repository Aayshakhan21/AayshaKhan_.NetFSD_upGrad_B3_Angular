namespace ECommerce.API.Authorization
{
    
    // Centralized role name constants used across the application.
    
    public static class Roles
    {
        public const string Admin    = "Admin";
        public const string Customer = "Customer";

        
        public const string AdminOrCustomer = "Admin,Customer";
    }
}
