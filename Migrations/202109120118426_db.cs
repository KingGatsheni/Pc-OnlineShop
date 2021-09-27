namespace OnlineShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        UserName = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.AccountId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        StuffId = c.Long(nullable: false),
                        Firstname = c.String(),
                        Id = c.String(),
                        CellNo = c.String(),
                        Email = c.String(),
                        EmployeeRole = c.String(),
                        Address = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        PaymentMethod = c.String(),
                        SaleId = c.Int(),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RepairId = c.Int(),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Sales", t => t.SaleId)
                .ForeignKey("dbo.Repairs", t => t.RepairId)
                .Index(t => t.EmployeeId)
                .Index(t => t.SaleId)
                .Index(t => t.RepairId);
            
            CreateTable(
                "dbo.Repairs",
                c => new
                    {
                        RepairId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        RepairStatus = c.Boolean(nullable: false),
                        RepairTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Bookon = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RepairId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        CellNo = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CustomerId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        SaleId = c.Int(nullable: false, identity: true),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        CustomerId = c.Int(),
                        EmployeeId = c.Int(),
                    })
                .PrimaryKey(t => t.SaleId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.CustomerId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.SalesItems",
                c => new
                    {
                        SaleItemId = c.Int(nullable: false, identity: true),
                        SaleId = c.Int(nullable: false),
                        InventoryId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ItemPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.SaleItemId)
                .ForeignKey("dbo.Inventories", t => t.InventoryId, cascadeDelete: true)
                .ForeignKey("dbo.Sales", t => t.SaleId, cascadeDelete: true)
                .Index(t => t.SaleId)
                .Index(t => t.InventoryId);
            
            CreateTable(
                "dbo.Inventories",
                c => new
                    {
                        InventoryId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        Category = c.String(),
                        CostPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Markup = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SellingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        ImageName = c.String(),
                    })
                .PrimaryKey(t => t.InventoryId);
            
            CreateTable(
                "dbo.RepairLists",
                c => new
                    {
                        RepairListId = c.Int(nullable: false, identity: true),
                        RepairId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        RepairPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemName = c.String(),
                        ItemFault = c.String(),
                    })
                .PrimaryKey(t => t.RepairListId)
                .ForeignKey("dbo.Repairs", t => t.RepairId, cascadeDelete: true)
                .Index(t => t.RepairId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RepairLists", "RepairId", "dbo.Repairs");
            DropForeignKey("dbo.Payments", "RepairId", "dbo.Repairs");
            DropForeignKey("dbo.Repairs", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.SalesItems", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.SalesItems", "InventoryId", "dbo.Inventories");
            DropForeignKey("dbo.Payments", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.Sales", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Sales", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Repairs", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Accounts", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RepairLists", new[] { "RepairId" });
            DropIndex("dbo.SalesItems", new[] { "InventoryId" });
            DropIndex("dbo.SalesItems", new[] { "SaleId" });
            DropIndex("dbo.Sales", new[] { "EmployeeId" });
            DropIndex("dbo.Sales", new[] { "CustomerId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Customers", new[] { "ApplicationUserId" });
            DropIndex("dbo.Repairs", new[] { "EmployeeId" });
            DropIndex("dbo.Repairs", new[] { "CustomerId" });
            DropIndex("dbo.Payments", new[] { "RepairId" });
            DropIndex("dbo.Payments", new[] { "SaleId" });
            DropIndex("dbo.Payments", new[] { "EmployeeId" });
            DropIndex("dbo.Accounts", new[] { "EmployeeId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RepairLists");
            DropTable("dbo.Inventories");
            DropTable("dbo.SalesItems");
            DropTable("dbo.Sales");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Customers");
            DropTable("dbo.Repairs");
            DropTable("dbo.Payments");
            DropTable("dbo.Employees");
            DropTable("dbo.Accounts");
        }
    }
}
