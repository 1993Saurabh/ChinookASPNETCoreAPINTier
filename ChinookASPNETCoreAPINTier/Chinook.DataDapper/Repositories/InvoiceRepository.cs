﻿using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Chinook.Domain.Repositories;
using Chinook.Domain.Entities;

namespace Chinook.DataDapper.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Invoice>> GetAllAsync(CancellationToken ct = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public async Task<Invoice> GetByIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Invoice>> GetByCustomerIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public async Task<Invoice> AddAsync(Invoice newInvoice, CancellationToken ct = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Invoice invoice, CancellationToken ct = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }
    }
}