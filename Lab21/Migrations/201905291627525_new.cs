namespace Lab21.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        UserName = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Items", t => t.Name)
                .ForeignKey("dbo.Customers", t => t.UserName)
                .Index(t => t.UserName)
                .Index(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserName", "dbo.Customers");
            DropForeignKey("dbo.Orders", "Name", "dbo.Items");
            DropIndex("dbo.Orders", new[] { "Name" });
            DropIndex("dbo.Orders", new[] { "UserName" });
            DropTable("dbo.Orders");
        }
    }
}
