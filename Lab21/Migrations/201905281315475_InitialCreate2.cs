namespace Lab21.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Color", c => c.String());
            AddColumn("dbo.Customers", "Pineapple", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Pineapple");
            DropColumn("dbo.Customers", "Color");
        }
    }
}
