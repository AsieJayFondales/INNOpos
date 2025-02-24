namespace InnoSpend.Services
{
    public interface ICustomerService
    {
        string GenerateCustomerCode();
        void UpdateLastActivity(int customerId);
        void AddLoyaltyPoints(int customerId, int points);
        Task<bool> DeleteInactiveCustomers();
    }
}
