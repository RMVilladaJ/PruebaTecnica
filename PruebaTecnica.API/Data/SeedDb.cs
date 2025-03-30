using Microsoft.EntityFrameworkCore;
using PruebaTecnica.API.Helpers;
using PruebaTecnica.Shared.Entidades;
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
        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }


        public async Task SeedAsync()
        {
            await _context.Database.EnsureDeletedAsync(); // 💥 Elimina la base de datos antes de crearla (para evitar errores)
            await _context.Database.EnsureCreatedAsync();
            await CheckRolesAsync(); //Validar roles de usuario
            await CheckUserAsync("123456", "Rosa", "Villada", "Villada@gmail.com", "3017993879", UserType.Admin);
            var suppliers = await CheckSuppliersAsync(); // Insertar y obtener proveedores
            var products = await CheckProductsAsync(suppliers); // Insertar y obtener productos
            await CheckSalesAsync(products); // Insertar ventas
        }
        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<User> CheckUserAsync(string document, string FullName, string SurName, string email, string phone, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {

                user = new User
                {

                    Document = document,
                    FullName = FullName,
                    SurName = SurName,
                    Email = email,
                    UserName = email,

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
                    new Supplier
                    {
                        NameSupplier = "Proveedor A",
                        NIT = "123456789",
                        FixedPhone = "1234567",
                        CellPhone = "3001234567",
                        Email = "proveedora@example.com"
                    },
                    new Supplier
                    {
                        NameSupplier = "Proveedor B",
                        NIT = "987654321",
                        FixedPhone = "7654321",
                        CellPhone = "3017654321",
                        Email = "proveedorb@example.com"
                    }
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
                if (suppliers == null || !suppliers.Any())
                {
                    Console.WriteLine("⚠️ No hay proveedores en la base de datos.");
                    return new List<Product>();
                }

                var products = new List<Product>();

                foreach (var supplier in suppliers)
                {
                    products.Add(new Product
                    {
                        Code = $"P{supplier.Id}01",
                        NameProduct = $"Producto de {supplier.NameSupplier}",
                        Photo = "https://www.google.com/imgres?q=mp4&imgurl=https%3A%2F%2Fwww.energysistem.com%2Fcdnassets%2Fproducts%2F45659%2Ffront_2000.webp%3F3%2F8%2Fa%2F5%2F38a5ad720858645ee79eaa433f0d9e3786605373_02_handy_mp4_player_45659.jpg&imgrefurl=https%3A%2F%2Fwww.energysistem.com%2Fhandy-reproductor-de-mp4-con-bluetooth-y-radio-fm-45659-mx&docid=4223oPfp6qBIhM&tbnid=kLXDFqcgQ7y2PM&vet=12ahUKEwj1idX7zbCMAxUMpLAFHYLwKfsQM3oECEYQAA..i&w=2000&h=2000&hcb=2&ved=2ahUKEwj1idX7zbCMAxUMpLAFHYLwKfsQM3oECEYQAA",
                        Price = 15000 + (supplier.Id * 5000), // Precio variable
                        CreateDate = DateTime.UtcNow,
                        SupplierId = supplier.Id
                    });
                }

                _context.Products.AddRange(products);
                await _context.SaveChangesAsync();
            }

            return await _context.Products.ToListAsync();
        }

        private async Task CheckSalesAsync(List<Product> products)
        {
            if (!_context.Sales.Any())
            {
                if (products == null || !products.Any())
                {
                    Console.WriteLine("⚠️ No hay productos en la base de datos.");
                    return;
                }

                var sales = new List<Sale>();

                foreach (var product in products)
                {
                    int quantity = new Random().Next(1, 5); // Generar cantidad aleatoria

                    sales.Add(new Sale
                    {
                        Quantity = quantity,
                        tax = 0.19,
                        FinalPrice = product.Price * 1.19 * quantity, // Precio con impuesto y cantidad
                        SaleDate = DateTime.UtcNow,
                        ProductId = product.Id,
                        UserId = 1
                    });
                }

                _context.Sales.AddRange(sales);
                await _context.SaveChangesAsync();
            }
        }
    }
}
