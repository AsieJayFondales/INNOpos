// Models/HomeViewModel.cs
//v6

//v9
using System;
using System.Collections.Generic; // For List<T>
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
#nullable disable

using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace InnoSpend.Models
{

    public class HomeViewModel
    {
        public string ReturnUrl { get; set; }

        public List<TransactionActivity> TransactionActivities { get; set; }
        public List<Category> Categories { get; set; }
        public List<RecentOrder> RecentOrders { get; set; }

        public HomeViewModel()
        {
            // Initialize the lists in the constructor
            TransactionActivities = new List<TransactionActivity>();
            Categories = new List<Category>();
            RecentOrders = new List<RecentOrder>();
        }
    }

    public class TransactionActivity
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }

    //function transfered to Category.cs
   //03-12-2025 v10prem
    public class Categories // change Category to Categories to prevent errors v10.0.1
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public int ItemCount { get; set; }
    }

    public class RecentOrder
    {
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}