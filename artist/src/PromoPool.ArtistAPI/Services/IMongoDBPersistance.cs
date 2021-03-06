﻿using System;
using PromoPool.ArtistAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PromoPool.ArtistAPI.Services
{
    public interface IMongoDBPersistance
    {
        Task<IEnumerable<Artist>> FindAllArtistsAsync();
        Task<IEnumerable<Artist>> FindAllArtistsByNameAsync(string artistName);
        Task<Artist>  FindArtistByIdAsync(Guid id);
        Task<string> InsertOneArtistAsync(Artist artist);
        Task<bool> DeleteOneArtistAsync(Guid id);
        Task<bool> DeleteAllArtistsAsync();
        Task<Artist> UpdateArtistByIdAsync(Guid id, Artist artist);
    }

}
