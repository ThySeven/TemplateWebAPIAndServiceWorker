using Planning_Service.Models;
using Planning_Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Planning_Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliverysController : ControllerBase
{
    private readonly DeliveryService _deliveryService;
    private readonly IMemoryCache _memoryCache;

    public DeliverysController(DeliverysService deliveryService, IMemoryCache memoryCache)
    {
        _deliveryService = deliveryService;
        _memoryCache = memoryCache;
    }

    [HttpGet]
    public async Task<List<Delivery>> Get() =>
        await _deliveryService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Delivery>> Get(string id)
    {
        var delivery = await _deliveryService.GetAsync(id);

        if (delivery is null)
        {
            return NotFound();
        }

        return delivery;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Delivery newDelivery)
    {
        await _deliveryService.CreateAsync(newDelivery);

        return CreatedAtAction(nameof(Get), new { id = newDelivery.Id }, newDelivery);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Delivery updatedDelivery)
    {
        var delivery = await _deliveryService.GetAsync(id);

        if (delivery is null)
        {
            return NotFound();
        }

        updatedDelivery.Id = delivery.Id;

        await _deliveryService.UpdateAsync(id, updatedDelivery);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var delivery = await _deliveryService.GetAsync(id);

        if (delivery is null)
        {
            return NotFound();
        }

        await _deliveryService.RemoveAsync(id);

        return NoContent();
    }

    private Delivery GetDeliveryFromCache(int deliveryId)
    {
        Delivery delivery = null;
        _memoryCache.TryGetValue(deliveryId, out delivery);
        return delivery;
    }

    private void RemoveFromCache(int deliveryId)
    {
        _memoryCache.Remove(deliveryId);
    }
}