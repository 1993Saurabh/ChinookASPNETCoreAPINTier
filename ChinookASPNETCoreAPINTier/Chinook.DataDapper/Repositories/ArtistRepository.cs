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
    public class ArtistRepository : IArtistRepository
    {
        private readonly DbInfo _dbInfo;

        public ArtistRepository(DbInfo dbInfo)
        {
            _dbInfo = dbInfo;
        }

        private IDbConnection Connection => new SqlConnection(_dbInfo.ConnectionStrings);
        
        public void Dispose()
        {
            
        }
        
        private async Task<bool> ArtistExists(int id, CancellationToken ct = default)
        {
            return await GetByIdAsync(id, ct) != null;
        }

        public async Task<List<Artist>> GetAllAsync(CancellationToken ct = default(CancellationToken))
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();
                return Connection.QueryAsync<Artist>("Select * From Artist").Result.ToList();
            }
        }

        public async Task<Artist> GetByIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            using (var cn = Connection)
            {
                cn.Open();
                return cn.QueryFirstOrDefaultAsync<Artist>("Select * From Artist WHERE Id = @Id", new {id}).Result;
            }
        }

        public async Task<Artist> AddAsync(Artist newArtist, CancellationToken ct = default(CancellationToken))
        {
            using (var cn = Connection)
            {
                cn.Open();

                newArtist.ArtistId = cn.InsertAsync(new Artist {Name = newArtist.Name}).Result;
            }

            return newArtist;
        }

        public async Task<bool> UpdateAsync(Artist artist, CancellationToken ct = default(CancellationToken))
        {
            if (!await ArtistExists(artist.ArtistId, ct))
                return false;

            try
            {
                using (var cn = Connection)
                {
                    cn.Open();
                    return cn.UpdateAsync(artist).Result;
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
                    return cn.DeleteAsync(new Artist {ArtistId = id}).Result;
                }  
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}