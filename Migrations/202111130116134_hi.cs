namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WishItems",
                c => new
                    {
                        ItemId = c.Int(nullable: false),
                        UserId = c.String(),
                        ItemName = c.String(),
                        ItemImage = c.String(),
                        ItemPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.WishLists",
                c => new
                    {
                        WishId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 500),
                        ProductId = c.Int(nullable: false),
                        ItemName = c.String(nullable: false, maxLength: 100),
                        ItemImage = c.String(nullable: false, maxLength: 100),
                        ItemPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.WishId);
            
            DropTable("dbo.WishItems");
        }
    }
}
