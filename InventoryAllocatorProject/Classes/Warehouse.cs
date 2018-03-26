using System.Collections.Generic;

namespace TestProject.Classes
{
    /// <summary>
    /// Represents a warehouse
    /// </summary>
    public class Warehouse
    {
        public string Name { get; set; }
        public Dictionary<string, int> Inventory { get; set; }
    }
}
