using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Chinook.Domain.Responses;
using Chinook.Domain.Converters;
using Chinook.Domain.Entities;

namespace Chinook.Domain.Supervisor
{
    public partial class ChinookSupervisor
    {
        public async Task<List<MediaTypeResponse>> GetAllMediaTypeAsync(
            CancellationToken ct = default(CancellationToken))
        {
            var mediaTypes = MediaTypeCoverter.ConvertList(await _mediaTypeRepository.GetAllAsync(ct));
            return mediaTypes;
        }

        public async Task<MediaTypeResponse> GetMediaTypeByIdAsync(int id,
            CancellationToken ct = default(CancellationToken))
        {
            var mediaTypeViewModel = MediaTypeCoverter.Convert(await _mediaTypeRepository.GetByIdAsync(id, ct));
            mediaTypeViewModel.Tracks = await GetTrackByMediaTypeIdAsync(mediaTypeViewModel.MediaTypeId, ct);
            return mediaTypeViewModel;
        }

        public async Task<MediaTypeResponse> AddMediaTypeAsync(MediaTypeResponse newMediaTypeViewModel,
            CancellationToken ct = default(CancellationToken))
        {
            var mediaType = new MediaType
            {
                Name = newMediaTypeViewModel.Name
            };

            mediaType = await _mediaTypeRepository.AddAsync(mediaType, ct);
            newMediaTypeViewModel.MediaTypeId = mediaType.MediaTypeId;
            return newMediaTypeViewModel;
        }

        public async Task<bool> UpdateMediaTypeAsync(MediaTypeResponse mediaTypeViewModel,
            CancellationToken ct = default(CancellationToken))
        {
            var mediaType = await _mediaTypeRepository.GetByIdAsync(mediaTypeViewModel.MediaTypeId, ct);

            if (mediaType == null) return false;
            mediaType.MediaTypeId = mediaTypeViewModel.MediaTypeId;
            mediaType.Name = mediaTypeViewModel.Name;

            return await _mediaTypeRepository.UpdateAsync(mediaType, ct);
        }

        public async Task<bool> DeleteMediaTypeAsync(int id, CancellationToken ct = default(CancellationToken)) => await _mediaTypeRepository.DeleteAsync(id, ct);
    }
}