namespace Lab21.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        itemID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                        price = c.Double(nullable: false),
                        quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.itemID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        userName = c.String(nullable: false, maxLength: 128),
                        password = c.String(),
                        email = c.String(),
                        balance = c.Double(),
                    })
                .PrimaryKey(t => t.userName);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Items");
        }
    }
}
