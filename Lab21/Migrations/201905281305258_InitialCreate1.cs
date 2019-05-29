namespace Lab21.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Users", newName: "Customers");
            AddColumn("dbo.Customers", "dob", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "dob");
            RenameTable(name: "dbo.Customers", newName: "Users");
        }
    }
}
