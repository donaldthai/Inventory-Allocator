using System.Collections.Generic;
using System.Linq;

namespace TestProject.Classes
{

    public class InventoryAllocator
    {
        /// <summary>
        /// Fulfills an order based on the available information passed in.
        /// Returns a list of warehouses that will fulfill an order the cheapest way.
        /// </summary>
        /// <param name="order">Map of current orders</param>
        /// <param name="inventoryDistribution">List of warehouses with inventory</param>
        /// <returns>List of warehouses that can fulfill the order. Otherwise, empty list if they can't.</returns>
        public List<Warehouse> FulfillOrder(Dictionary<string, int> order, List<Warehouse> inventoryDistribution)
        {
            var result = new List<Warehouse>();
            //each itemname in order
            foreach (var itemName in order.Keys)
            {
                //# of inventory to fulfill
                //we can subtract the amount to 0, so we know when amount is enough
                var itemCount = order[itemName];
                //each warehouse
                foreach (var warehouse in inventoryDistribution)
                {
                    //check if the warehouse has item and inventory
                    if (warehouse.Inventory.ContainsKey(itemName) && warehouse.Inventory[itemName] > 0)
                    {
                        var itemCountAllocated = 0;
                        //check if there's more than inventory than the order amount
                        if (warehouse.Inventory[itemName] > itemCount)
                        {
                            //allocate just enough according to order
                            warehouse.Inventory[itemName] -= itemCount;
                            itemCountAllocated = itemCount;
                            //set item count to 0 since warehouse can fulfill all the order
                            itemCount = 0;
                        }
                        else
                        {
                            //otherwise, allocate all the inventory in the warehouse
                            itemCountAllocated = warehouse.Inventory[itemName];
                            itemCount -= warehouse.Inventory[itemName];
                            //allocate all warehouse inventory
                            warehouse.Inventory[itemName] = 0;
                        }

                        //allocate inventory amount to fulfill
                        if (result.Any(w => w.Name == warehouse.Name))
                        {
                            result.Find(w => w.Name == warehouse.Name).Inventory.Add(itemName, itemCountAllocated);
                        }
                        else //add warehouse if added already
                        {
                            result.Add(new Warehouse
                            {
                                Name = warehouse.Name,
                                Inventory = new Dictionary<string, int> { { itemName, itemCountAllocated } }
                            });
                        }
                    }

                    //check if current order has been fulfilled
                    if (itemCount == 0)
                    {
                        //no need to do additional work
                        break;
                    }
                }

                //check again at the end
                if (itemCount > 0)
                {
                    //warehouses can't fulfill
                    //return empty list
                    result = new List<Warehouse>(); 
                    break;
                }
            }

            return result;
        }
    }

}
