using NBPNeo4J.DTOs;
using NBPNeo4J.Models;
using NBPNeo4J.Repositories;

namespace NBPNeo4J.Services
{
    public interface IPartsService
    {
        Task<ReturnPartDTO> CreatePart(CreatePartDTO createPartDTO);
        Task<ReturnPartDTO> UpdatePart(string serialCode, CreatePartDTO createPartDTO);
        Task DeletePart(string serialCode);
        Task<List<ReturnPartDTO>> GetAllParts();

        // Napravi DTO za PartCategory
        Task<List<PartCategoryDto>> GetAllPartCategories();
        Task<List<ReturnPartDTO>> GetAllPartsOfCategory(string categoryId);
    }
    public class PartsService : IPartsService
    {
        private readonly IPartsRepository _partsRepository;
        public PartsService(IPartsRepository partsRepository)
        {
            _partsRepository = partsRepository;
        }
        public async Task<ReturnPartDTO> CreatePart(CreatePartDTO createPartDTO)
        {
            var part = new Part
            {
                SerialCode = createPartDTO.SerialCode,
                Name = createPartDTO.Name,
                Description = createPartDTO.Description,
                Image = createPartDTO.Image,
                Price = createPartDTO.Price,
                PartCategoryId = createPartDTO.PartCategoryId
            };

            Part createdPart = await _partsRepository.CreatePartAsync(part, createPartDTO.PartCategoryId);

            ReturnPartDTO returnPartDTO = new ReturnPartDTO();
            returnPartDTO.SerialCode = createdPart.SerialCode;
            returnPartDTO.Name = createdPart.Name;
            returnPartDTO.Description = createdPart.Description;
            returnPartDTO.Image = createdPart.Image;
            returnPartDTO.Price = createdPart.Price;
            returnPartDTO.PartCategory.Id = createdPart.PartCategoryId;

            return returnPartDTO;
        }

        public async Task DeletePart(string serialCode)
        {
            await _partsRepository.DeletePartAsync(serialCode);
        }

        public async Task<List<PartCategoryDto>> GetAllPartCategories()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ReturnPartDTO>> GetAllParts()
        {
            List<Part> parts = await _partsRepository.GetAllPartsAsync();
            List<ReturnPartDTO> returnParts = new();
            foreach(Part part in parts)
            {
                ReturnPartDTO returnPart = new ReturnPartDTO
                {
                    SerialCode = part.SerialCode,
                    Name = part.Name,
                    Description = part.Description,
                    Image = part.Image,
                    Price = part.Price,
                    PartCategoryId = part.PartCategoryId
                };
                returnParts.Add(returnPart);
            }

            return returnParts;
        }

        public async Task<List<ReturnPartDTO>> GetAllPartsOfCategory(string categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnPartDTO> UpdatePart(string serialCode, CreatePartDTO createPartDTO)
        {
            var part = new Part
            {
                SerialCode = serialCode,
                Name = createPartDTO.Name,
                Description = createPartDTO.Description,
                Image = createPartDTO.Image,
                Price = createPartDTO.Price,
                PartCategoryId = createPartDTO.PartCategoryId
            };

            Part updatedPart = await _partsRepository.UpdatePartInformationAsync(part);

            ReturnPartDTO returnPartDTO = new ReturnPartDTO();
            returnPartDTO.SerialCode = updatedPart.SerialCode;
            returnPartDTO.Name = updatedPart.Name;
            returnPartDTO.Description = updatedPart.Description;
            returnPartDTO.Image = updatedPart.Image;
            returnPartDTO.PartCategoryId = updatedPart.PartCategoryId;

            return returnPartDTO;
        }
    }
}
