using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.Extensions.Configuration;

namespace API.Data
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DataContext _context;
        private readonly IBidRepository _bidRepository;
        private readonly IConfiguration _config;
        public PaymentRepository(DataContext context,IBidRepository bidRepository, IConfiguration config)
        {
            _config = config;
            _bidRepository = bidRepository;
            _context = context;
        }

        public Task<Payment> CreatePaymentIntent(int paymentId)
        {
            throw new NotImplementedException();
        }
    }
}