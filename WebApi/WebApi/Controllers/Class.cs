using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MySql;
using MySqlX.XDevAPI;


namespace webapi.Controllers
{
    public class Class
    {
    }


    // 定義實體類
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    // 定義DbContext類
    public class MyDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "server=localhost;port=3306;database=localhost;user=root;password=700?NcFIk%Ge";
            ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);

            optionsBuilder.UseMySql(connectionString, serverVersion);
        }
    }

    // 定義接口
    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
    }

    // 實現接口的類
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext _dbContext;

        public ProductRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
        }

        public List<Product> GetAllProducts()
        {
            return _dbContext.Products.ToList();
        }

        public Product GetProductById(int id)
        {
            return _dbContext.Products.Find(id);
        }

        public void AddProduct(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            Console.WriteLine("產品已添加");
        }

        public void UpdateProduct(Product product)
        {
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
            Console.WriteLine("產品已更新");
        }

        public void DeleteProduct(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
                Console.WriteLine("產品已刪除");
            }



        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            // 創建DbContext對象
            using (var dbContext = new MyDbContext())
            {
                // 創建ProductRepository對象
                var productRepository = new ProductRepository(dbContext);

                // 添加產品
                var product1 = new Product { Name = "產品1", Price = 10.99m };
                productRepository.AddProduct(product1);

                // 獲取所有產品
                var products = productRepository.GetAllProducts();
                foreach (var product in products)
                {
                    Console.WriteLine($"產品ID: {product.Id}, 名稱: {product.Name}, 價格: {product.Price}");
                }

                // 更新產品
                var product2 = productRepository.GetProductById(1);
                if (product2 != null)
                {
                    product2.Name = "更新後的產品";
                    product2.Price = 19.99m;
                    productRepository.UpdateProduct(product2);
                }

                // 刪除產品
                productRepository.DeleteProduct(1);
            }
        }
    }
}
