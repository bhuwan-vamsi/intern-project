using Microsoft.AspNetCore.Mvc;
using APIPractice.Services;
using APIPractice.Models;
using APIPractice.DTOs;
using AutoMapper;

namespace APIPractice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ItemService _itemService;
        private readonly IMapper _mapper;

        public InventoryController(ItemService itemService, IMapper mapper)
        {
            _itemService = itemService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> Get()
        {
            var items = await _itemService.GetAllItemsAsync();
            var itemsDto = _mapper.Map<IEnumerable<ItemDto>>(items);
            return Ok(itemsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetById(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
                return NotFound();

            var itemDto = _mapper.Map<ItemDto>(item);
            return Ok(itemDto);
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> Create(CreateItemDto createItemDto)
        {
            var item = _mapper.Map<Item>(createItemDto);
            await _itemService.AddItemAsync(item);
            var itemDto = _mapper.Map<ItemDto>(item);
            return CreatedAtAction(nameof(GetById), new { id = itemDto.Id }, itemDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateItemDto updateDto)
        {
            var existingItem = await _itemService.GetItemByIdAsync(id);
            if (existingItem == null)
                return NotFound();

            _mapper.Map(updateDto, existingItem);
            await _itemService.UpdateItemAsync(existingItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
                return NotFound();

            await _itemService.DeleteItemAsync(id);
            return NoContent();
        }
    }
}
