﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Chinook.Domain.DbInfo;
using Chinook.Domain.Repositories;
using Chinook.Domain.Entities;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Chinook.DataDapper.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DbInfo _dbInfo;

        public EmployeeRepository(DbInfo dbInfo)
        {
            _dbInfo = dbInfo;
        }

        private IDbConnection Connection => new SqlConnection(_dbInfo.ConnectionStrings);

        public void Dispose()
        {
            
        }
        
        private async Task<bool> EmployeeExists(int id, CancellationToken ct = default)
        {
            return await GetByIdAsync(id, ct) != null;
        }

        public async Task<List<Employee>> GetAllAsync(CancellationToken ct = default(CancellationToken))
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();
                return Connection.QueryAsync<Employee>("Select * From Employee").Result.ToList();
            }
        }

        public async Task<Employee> GetByIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            using (var cn = Connection)
            {
                cn.Open();
                return cn.QueryFirstOrDefaultAsync<Employee>("Select * From Employee WHERE Id = @Id", new {id}).Result;
            }
        }

        public async Task<Employee> GetReportsToAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            using (var cn = Connection)
            {
                cn.Open();
                return cn.QueryFirstOrDefaultAsync<Employee>("Select * From Employee WHERE Id = @Id", new {id}).Result;
            }
        }

        public async Task<Employee> AddAsync(Employee newEmployee, CancellationToken ct = default(CancellationToken))
        {
            using (var cn = Connection)
            {
                cn.Open();

                newEmployee.EmployeeId = cn.InsertAsync(
                    new Employee
                    {
                        LastName = newEmployee.LastName,
                        FirstName = newEmployee.FirstName,
                        Title = newEmployee.Title,
                        ReportsTo = newEmployee.ReportsTo,
                        BirthDate = newEmployee.BirthDate,
                        HireDate = newEmployee.HireDate,
                        Address = newEmployee.Address,
                        City = newEmployee.City,
                        State = newEmployee.State,
                        Country = newEmployee.Country,
                        PostalCode = newEmployee.PostalCode,
                        Phone = newEmployee.Phone,
                        Fax = newEmployee.Fax,
                        Email = newEmployee.Email
                    }).Result;
            }

            return newEmployee;
        }

        public async Task<bool> UpdateAsync(Employee employee, CancellationToken ct = default(CancellationToken))
        {
            if (!await EmployeeExists(employee.EmployeeId, ct))
                return false;

            try
            {
                using (var cn = Connection)
                {
                    cn.Open();
                    return cn.UpdateAsync(employee).Result;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            try
            {
                using (var cn = Connection)
                {
                    cn.Open();
                    return cn.DeleteAsync(new Employee {EmployeeId = id}).Result;
                }  
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Employee>> GetDirectReportsAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            using (var cn = Connection)
            {
                cn.Open();
                return cn.QueryAsync<Employee>("Select * From Employee WHERE ArtistId = @Id", new { id }).Result.ToList();
            }
        }
    }
}