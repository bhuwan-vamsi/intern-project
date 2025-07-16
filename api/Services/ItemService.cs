using APIPractice.Models;
using APIPractice.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIPractice.Services
{
    public class ItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _itemRepository.GetAllItemsAsync();
        }
        public Task<Item?> GetItemByIdAsync(int id) => _itemRepository.GetItemByIdAsync(id);
        public Task AddItemAsync(Item item) => _itemRepository.AddItemAsync(item);
        public Task UpdateItemAsync(Item item) => _itemRepository.UpdateItemAsync(item);
        public Task DeleteItemAsync(int id) => _itemRepository.DeleteItemAsync(id);
    }
}
