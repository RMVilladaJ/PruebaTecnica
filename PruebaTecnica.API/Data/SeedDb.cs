using Microsoft.EntityFrameworkCore;
using PruebaTecnica.API.Helpers;
using PruebaTecnica.Shared.Entities;
using PruebaTecnica.Shared.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnica.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private List<User> userList = new List<User>();

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureDeletedAsync(); // Borra y crea la BD (solo en desarrollo)
            await _context.Database.EnsureCreatedAsync();

            await CheckRolesAsync(); // Validar roles de usuario
            await CheckUsersAsync(); // Crear usuarios
            var suppliers = await CheckSuppliersAsync(); // Crear proveedores
            var products = await CheckProductsAsync(suppliers); // Crear productos
            await CheckSalesAsync(products); // Crear ventas
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Seller.ToString());
        }

        private async Task CheckUsersAsync()
        {
            User admin = await CheckUserAsync("123456", "Rosa", "Villada", "admin@gmail.com", "3017993879", UserType.Admin);
            User seller1 = await CheckUserAsync("123456", "Rosa 1", "Villada Seller", "seller01@gmail.com", "30179938791", UserType.Seller);
            User seller2 = await CheckUserAsync("123456", "Rosa 2", "Villada Seller", "seller02@gmail.com", "30179938792", UserType.Seller);

            userList.Add(admin);
            userList.Add(seller1);
            userList.Add(seller2);
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Document = document,
                    FullName = firstName,
                    SurName = lastName,
                    Email = email,
                    UserName = email,
                    EmailConfirmed = true,
                    PhoneNumber = phone,
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }
            return user;
        }

        private async Task<List<Supplier>> CheckSuppliersAsync()
        {
            if (!_context.Suppliers.Any())
            {
                var suppliers = new List<Supplier>
                {
                    new Supplier { NameSupplier = "Exito", NIT = "123456789", FixedPhone = "1234567", CellPhone = "3001234567", Email = "Exito@example.com" },
                    new Supplier { NameSupplier = "ARA", NIT = "987654321", FixedPhone = "7654321", CellPhone = "3017654321", Email = "ARA@example.com" },
                    new Supplier { NameSupplier = "Olimpica", NIT = "567891234", FixedPhone = "1112233", CellPhone = "3025678912", Email = "Olimpica@example.com" },
                    new Supplier { NameSupplier = "Jumbo", NIT = "789123456", FixedPhone = "9998887", CellPhone = "3037891234", Email = "D1@example.com" },
                    new Supplier { NameSupplier = "D1", NIT = "234567890", FixedPhone = "6667775", CellPhone = "3042345678", Email = "La Vaquita@example.com" },
                    new Supplier { NameSupplier = "La Vaquita", NIT = "876543210", FixedPhone = "5554443", CellPhone = "3058765432", Email = "proveedorf@example.com" },
                    new Supplier { NameSupplier = "Corabastos", NIT = "345678901", FixedPhone = "3332221", CellPhone = "3063456789", Email = "Corabastos@example.com" },
                    new Supplier { NameSupplier = "Surtimax", NIT = "456789012", FixedPhone = "2221119", CellPhone = "3074567890", Email = "Surtimax@example.com" }
                };

                _context.Suppliers.AddRange(suppliers);
                await _context.SaveChangesAsync();
            }

            return await _context.Suppliers.ToListAsync();
        }

        private async Task<List<Product>> CheckProductsAsync(List<Supplier> suppliers)
        {
            if (!_context.Products.Any())
            {
                List<Product> products = new()
                {
                    new Product { Code = "P01", NameProduct = "Ariel", Photo = "p01.jpg", Price = 9000, CreateDate = new DateTime(), SupplierId = suppliers[0].Id },
                    new Product { Code = "P02", NameProduct = "Ricostillas", Photo = "p02.jpg", Price = 9000, CreateDate = new DateTime(), SupplierId = suppliers[1].Id },
                    new Product { Code = "P03", NameProduct = "H2O", Photo = "p03.jpg", Price = 9000, CreateDate = new DateTime(), SupplierId = suppliers[2].Id },
                    new Product { Code = "P04", NameProduct = "Winny Pants", Photo = "p04.jpg", Price = 9000, CreateDate = new DateTime(), SupplierId = suppliers[3].Id },
                    new Product { Code = "P05", NameProduct = "Nutribella", Photo = "p05.jpg", Price = 9000, CreateDate = new DateTime(), SupplierId = suppliers[4].Id },
                    new Product { Code = "P06", NameProduct = "Deseo", Photo = "p06.jpg", Price = 9000, CreateDate = new DateTime(), SupplierId = suppliers[5].Id },
                    new Product { Code = "P07", NameProduct = "Nugets pietran", Photo = "p07.jpg", Price = 9000, CreateDate = new DateTime(), SupplierId = suppliers[6].Id },
                    new Product { Code = "P08", NameProduct = "Svelty", Photo = "p08.jpg", Price = 9000, CreateDate = new DateTime(), SupplierId = suppliers[6].Id },
                    new Product { Code = "P09", NameProduct = "Deditos Zenu", Photo = "p09.jpg", Price = 9000, CreateDate = new DateTime(), SupplierId = suppliers[0].Id },
                    new Product { Code = "P10", NameProduct = "Empanadas Carne", Photo = "p10.jpg", Price = 9000, CreateDate = new DateTime(), SupplierId = suppliers[1].Id },
                    new Product { Code = "P11", NameProduct = "Maiz en grano", Photo = "p11.jpg", Price = 9000, CreateDate = new DateTime(), SupplierId = suppliers[2].Id },
                    new Product { Code = "P12", NameProduct = "Chorizo de cerdo", Photo = "p12.jpg", Price = 9000, CreateDate = new DateTime(), SupplierId = suppliers[3].Id },
                    new Product { Code = "P12", NameProduct = "Nuggets pollo", Photo = "p13.jpg", Price = 9000, CreateDate = new DateTime(), SupplierId = suppliers[3].Id },
                    new Product { Code = "P12", NameProduct = "Chorizo de cerdo", Photo = "p14.jpg", Price = 9000, CreateDate = new DateTime(), SupplierId = suppliers[3].Id },
                    new Product { Code = "P12", NameProduct = "Nuggets pollo", Photo = "p15.jpg", Price = 9000, CreateDate = new DateTime(), SupplierId = suppliers[3].Id },

                };

                Console.WriteLine(products.Count);



                _context.Products.AddRange(products);
                await _context.SaveChangesAsync();

            }

            return await _context.Products.ToListAsync();
        }

        private async Task CheckSalesAsync(List<Product> products)
        {
            if (!_context.Sales.Any())
            {
                Random random = new Random();
                var sales = new List<Sale>();

                for (int i = 0; i < 10; i++)
                {
                    var product = products[random.Next(products.Count)];
                    var user = userList[random.Next(userList.Count)];

                    int quantity = random.Next(1, 5);
                    sales.Add(new Sale
                    {
                        Quantity = quantity,
                        tax = 0.19,
                        FinalPrice = product.Price * quantity * 1.19,
                        SaleDate = DateTime.UtcNow,
                        ProductId = product.Id,
                        UserId = user.Id
                    });
                }

                _context.Sales.AddRange(sales);
                await _context.SaveChangesAsync();
            }
        }
    }
}
