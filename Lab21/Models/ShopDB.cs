namespace Lab21
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
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
        public virtual DbSet<Customer> users { get; set; }
        public virtual DbSet<Item> items { get; set; }
    }

    public class Customer
    {
        [Key]
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public double? balance { get; set; }
        public Item[] purchased { get; set; }
        public DateTime? dob { get; set; }
        //@myself PUT MORE SHIT HERE
    }

    public class Item
    {
        [Key]
        public int itemID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
    }
}