using System;

namespace Unalive_WebManagement.Models;

public partial class CharacterPvP
{
    public int CharacterTacticId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;
}
