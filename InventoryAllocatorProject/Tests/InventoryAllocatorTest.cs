using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProject.Classes;

namespace TestProject.Tests
{
    [TestClass]
    public class InventoryAllocatorTest
    {
        /// <summary>
        /// Positive test case, perfect order. Exact match.
        /// </summary>
        [TestMethod]
        public void ExactMatchTest()
        {
            //test data
            var order = new Dictionary<string, int>
            {
                {"apple", 1}
            };
            var inventoryDistribution = new List<Warehouse>
            {
                new Warehouse { Name = "owd", Inventory = new Dictionary<string, int>{ {"apple", 1} } }
            };

            var ia = new InventoryAllocator();
            var result = ia.FulfillOrder(order, inventoryDistribution);
            var expected = new List<Warehouse>
            {
                new Warehouse { Name = "owd", Inventory = new Dictionary<string, int> { {"apple", 1} } }
            };
            
            
            //assertions
            //should be just one
            Assert.AreEqual(expected.Count, result.Count);
            Assert.AreEqual(expected[0].Name, result[0].Name);
            CollectionAssert.AreEquivalent(expected[0].Inventory, result[0].Inventory);
        }

        /// <summary>
        /// Positive test case, perfect order. Exact match for each order of items.
        /// </summary>
        [TestMethod]
        public void MultipleOrderExactMatchTest()
        {
            //test data
            var order = new Dictionary<string, int>
            {
                {"apple", 1},
                {"banana", 1},
                {"orange", 1}
            };
            var inventoryDistribution = new List<Warehouse>
            {
                new Warehouse { Name = "owd", Inventory = new Dictionary<string, int>{ {"apple", 1}, {"banana", 1}, {"orange", 1} } }
            };

            var ia = new InventoryAllocator();
            var result = ia.FulfillOrder(order, inventoryDistribution);
            var expected = new List<Warehouse>
            {
                new Warehouse { Name = "owd", Inventory = new Dictionary<string, int>{ {"apple", 1}, {"banana", 1}, {"orange", 1} } }
            };


            //assertions
            //should be just one
            Assert.AreEqual(expected.Count, result.Count);
            Assert.AreEqual(expected[0].Name, result[0].Name);
            CollectionAssert.AreEquivalent(expected[0].Inventory, result[0].Inventory);
        }

        /// <summary>
        /// Positive test case, spread inventory allocation across warehouses.
        /// Should split an item across warehouses if that is the only way to completely ship an item.
        /// </summary>
        [TestMethod]
        public void SpreadInventoryTest()
        {
            //test data
            var order = new Dictionary<string, int>
            {
                {"apple", 10}
            };
            var inventoryDistribution = new List<Warehouse>
            {
                new Warehouse { Name = "owd", Inventory = new Dictionary<string, int>{ {"apple", 5} } },
                new Warehouse { Name = "dm", Inventory = new Dictionary<string, int>{ {"apple", 5} } }
            };

            var ia = new InventoryAllocator();
            var result = ia.FulfillOrder(order, inventoryDistribution);
            var expected = new List<Warehouse>
            {
                new Warehouse { Name = "owd", Inventory = new Dictionary<string, int>{ {"apple", 5} } },
                new Warehouse { Name = "dm", Inventory = new Dictionary<string, int>{ {"apple", 5} } }
            };

            //assertions
            //should be just 2 warehouses
            Assert.AreEqual(expected.Count, result.Count);
            Assert.AreEqual(expected[0].Name, result[0].Name);
            Assert.AreEqual(expected[1].Name, result[1].Name);
            CollectionAssert.AreEquivalent(expected[0].Inventory, result[0].Inventory);
            CollectionAssert.AreEquivalent(expected[1].Inventory, result[1].Inventory);
        }

        /// <summary>
        /// Positive test case, spread inventory allocation across warehouses for multiple orders.
        /// Should split an item across warehouses if that is the only way to completely ship an item.
        /// </summary>
        [TestMethod]
        public void MultipleOrderSpreadInventoryTest()
        {
            //test data
            var order = new Dictionary<string, int>
            {
                {"apple", 1},
                {"banana", 2},
                {"orange", 2}
            };
            var inventoryDistribution = new List<Warehouse>
            {
                new Warehouse { Name = "owd", Inventory = new Dictionary<string, int>{ {"apple", 1}, {"banana", 1}, {"orange", 1} } },
                new Warehouse { Name = "dm", Inventory = new Dictionary<string, int>{ {"apple", 1}, {"banana", 1}, {"orange", 1} } }
            };

            var ia = new InventoryAllocator();
            var result = ia.FulfillOrder(order, inventoryDistribution);
            var expected = new List<Warehouse>
            {
                new Warehouse { Name = "owd", Inventory = new Dictionary<string, int>{ {"apple", 1}, {"banana", 1}, {"orange", 1} } },
                new Warehouse { Name = "dm", Inventory = new Dictionary<string, int>{ {"banana", 1}, {"orange", 1} } }
            };


            //assertions
            //should be just one
            Assert.AreEqual(expected.Count, result.Count);
            Assert.AreEqual(expected[0].Name, result[0].Name);
            Assert.AreEqual(expected[1].Name, result[1].Name);
            CollectionAssert.AreEquivalent(expected[0].Inventory, result[0].Inventory);
            CollectionAssert.AreEquivalent(expected[1].Inventory, result[1].Inventory);
        }

        /// <summary>
        /// Negative test case, not enough inventory!
        /// </summary>
        [TestMethod]
        public void NotEnoughInventoryTest()
        {
            //test data
            var order = new Dictionary<string, int>
            {
                {"apple", 1}
            };
            var inventoryDistribution = new List<Warehouse>
            {
                new Warehouse { Name = "owd", Inventory = new Dictionary<string, int>{ {"apple", 0} } }
            };

            var ia = new InventoryAllocator();
            var result = ia.FulfillOrder(order, inventoryDistribution);
            var expected = new List<Warehouse>(); 

            //assertions
            //should be just empty list
            Assert.AreEqual(expected.Count, result.Count);
        }

        /// <summary>
        /// Negative test case, not enough inventory!
        /// </summary>
        [TestMethod]
        public void MultipleOrderNotEnoughInventoryTest()
        {
            //test data
            var order = new Dictionary<string, int>
            {
                {"apple", 2},
                {"banana", 2},
                {"orange", 2}
            };
            var inventoryDistribution = new List<Warehouse>
            {
                new Warehouse { Name = "owd", Inventory = new Dictionary<string, int>{ {"apple", 0}, {"banana", 1}, {"orange", 1} } },
                new Warehouse { Name = "dm", Inventory = new Dictionary<string, int>{ {"apple", 1}, {"banana", 1}, {"orange", 1} } }
            };

            var ia = new InventoryAllocator();
            var result = ia.FulfillOrder(order, inventoryDistribution);
            var expected = new List<Warehouse>();


            //assertions
            //should be just zero
            Assert.AreEqual(expected.Count, result.Count);
        }

    }
}
