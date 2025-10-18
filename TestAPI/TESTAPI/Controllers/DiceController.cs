using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DiceRollController : ControllerBase
{
    [HttpPost]
    public IActionResult SaveRoll([FromBody] DiceRoll roll)
    {
        // You can save to a database or log here
        Console.WriteLine($"Received roll: {roll.Roll1} + {roll.Roll2} = {roll.Sum} at {roll.Timestamp}");
        return Ok();
    }
}

public class DiceRoll
{
    public int Roll1 { get; set; }
    public int Roll2 { get; set; }
    public int Sum { get; set; }
    public DateTime Timestamp { get; set; }
}