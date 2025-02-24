// Services/CustomerService.cs
using InnoSpend.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoSpend.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GenerateCustomerCode()
        {
            string year = DateTime.Now.Year.ToString().Substring(2);
            int sequence = _context.Customers
                .Where(c => c.CustomerCode.StartsWith(year))
                .Count() + 1;
            return $"{year}{sequence:D5}";
        }

        public void UpdateLastActivity(int customerId)
        {
            var customer = _context.Customers.Find(customerId);
            if (customer != null)
            {
                customer.LastActivityDate = DateTime.Now;
                _context.SaveChanges();
            }
        }

        public void AddLoyaltyPoints(int customerId, int points)
        {
            var customer = _context.Customers.Find(customerId);
            if (customer != null && customer.IsLoyaltyMember)
            {
                customer.LoyaltyPoints += points;
                _context.SaveChanges();
            }
        }

        public async Task<bool> DeleteInactiveCustomers()
        {
            var twoYearsAgo = DateTime.Now.AddYears(-2);
            var inactiveCustomers = await _context.Customers
                .Where(c => c.LastActivityDate < twoYearsAgo)
                .ToListAsync();

            foreach (var customer in inactiveCustomers)
            {
                customer.IsActive = false;
                // Don't actually delete, just mark as inactive for GDPR compliance
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}