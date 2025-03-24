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
        
        Task<List<ReturnPartDTO>> GetAllPartsOfCategory(string categoryId);

        Task<List<PartCategoryDTO>> GetAllPartCategories();
        Task<PartCategoryDTO> GetPartCategory(string categoryId);
        Task<PartCategoryDTO> CreateCategory(PartCategoryDTO createCategoryDTO);
        Task<PartCategoryDTO> UpdateCategory(string categoryId, PartCategoryDTO createCategoryDTO);
        Task DeleteCategory(string categoryId);
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

            
            var category = await _partsRepository.GetPartCategoryAsync(createdPart.PartCategoryId);

            ReturnPartDTO returnPartDTO = new ReturnPartDTO
            {
                SerialCode = createdPart.SerialCode,
                Name = createdPart.Name,
                Description = createdPart.Description,
                Image = createdPart.Image,
                Price = createdPart.Price,
                PartCategory = new PartCategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                }
            };

            return returnPartDTO;
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

            
            var category = await _partsRepository.GetPartCategoryAsync(updatedPart.PartCategoryId);

            ReturnPartDTO returnPartDTO = new ReturnPartDTO
            {
                SerialCode = updatedPart.SerialCode,
                Name = updatedPart.Name,
                Description = updatedPart.Description,
                Image = updatedPart.Image,
                Price = updatedPart.Price,
                PartCategory = new PartCategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                }
            };

            return returnPartDTO;
        }

        public async Task DeletePart(string serialCode)
        {
            await _partsRepository.DeletePartAsync(serialCode);
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
            var parts = await _partsRepository.GetAllPartsOfCategoryAsync(categoryId);

            List<ReturnPartDTO> partDTOs = new List<ReturnPartDTO>();

            foreach (var part in parts)
            {
                partDTOs.Add(new ReturnPartDTO
                {
                    SerialCode = part.SerialCode,
                    Name = part.Name,
                    Description = part.Description,
                    Image = part.Image,
                    Price = part.Price,
                    PartCategory = new PartCategoryDTO
                    {
                        Id = part.PartCategoryId
                    }
                });
            }

            return partDTOs;
        }

      


        public async Task<List<PartCategoryDTO>> GetAllPartCategories()
        {
            var categories = await _partsRepository.GetPartCategoriesAsync();

            List<PartCategoryDTO> categoryDtos = new List<PartCategoryDTO>();

            foreach (var category in categories)
            {
                categoryDtos.Add(new PartCategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                });
            }

            return categoryDtos;
        }

        public async Task<PartCategoryDTO> GetPartCategory(string categoryId)
        {
            var category = await _partsRepository.GetPartCategoryAsync(categoryId);
            if (category == null)
            {
                return null;
            }

            return new PartCategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<PartCategoryDTO> CreateCategory(PartCategoryDTO createCategoryDTO)
        {
            var category = new PartCategory
            {
                Name = createCategoryDTO.Name,
                Description = createCategoryDTO.Description
            };

            PartCategory createdCategory = await _partsRepository.CreatePartCategoryAsync(category);

            return new PartCategoryDTO
            {
                Id = createdCategory.Id,
                Name = createdCategory.Name,
                Description = createdCategory.Description
            };
        }

        public async Task<PartCategoryDTO> UpdateCategory(string categoryId, PartCategoryDTO createCategoryDTO)
        {
            var category = new PartCategory
            {
                Id = categoryId,
                Name = createCategoryDTO.Name,
                Description = createCategoryDTO.Description
            };

            PartCategory updatedCategory = await _partsRepository.UpdatePartCategoryAsync(category);

            return new PartCategoryDTO
            {
                Id = updatedCategory.Id,
                Name = updatedCategory.Name,
                Description = updatedCategory.Description
            };
        }

        public async Task DeleteCategory(string categoryId)
        {
            await _partsRepository.DeletePartCategoryAsync(categoryId);
        }

    }
}
