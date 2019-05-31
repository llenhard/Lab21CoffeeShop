namespace Lab21
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;

    public class ShopDB : DbContext
    {
        // Your context has been configured to use a 'ShopDB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Lab21.ShopDB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ShopDB' 
        // connection string in the application configuration file.
        public ShopDB() : base("name=ShopDB")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected virtual void ThrowEnhancedValidationException(DbEntityValidationException e)
        {
            var errorMessages = e.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            var fullErrorMessage = string.Join("; ", errorMessages);
            var exceptionMessage = string.Concat(e.Message, " The validation errors are: ", fullErrorMessage);
            throw new DbEntityValidationException(exceptionMessage, e.EntityValidationErrors);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                ThrowEnhancedValidationException(e);
            }

            return 0;
        }
    }
    
    public class Customer
    {
        [Key]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public double? Balance { get; set; }
        public DateTime? Dob { get; set; }
        public string Color { get; set; }
        public bool Pineapple { get; set; }
    }

    public class Item
    {
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }

    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("User")]
        public string UserName { get; set; }
        [ForeignKey("Item")]
        public string Name { get; set; }

        public virtual Customer User { get; set; }
        public virtual Item Item { get; set; }
    }
}