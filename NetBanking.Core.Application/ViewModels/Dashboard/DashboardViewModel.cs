﻿namespace NetBanking.Core.Application.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public int AllTransaction { get; set; }
        public decimal AllPayments { get; set; }
        public int AllPaymentsNumber { get; set; }
        public int ActiveClients { get; set; }
        public int InactiveClients { get; set; }
        public int AssignedProduct { get; set; }
    }
}
