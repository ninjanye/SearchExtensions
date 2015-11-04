namespace NinjaNye.SearchExtensions.Tests.Integration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChildTestModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StringOne = c.String(),
                        StringTwo = c.String(),
                        StringThree = c.String(),
                        IntegerOne = c.Int(nullable: false),
                        IntegerTwo = c.Int(nullable: false),
                        IntegerThree = c.Int(nullable: false),
                        TestModel_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TestModels", t => t.TestModel_Id)
                .Index(t => t.TestModel_Id);
            
            CreateTable(
                "dbo.TestModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StringOne = c.String(),
                        StringTwo = c.String(),
                        StringThree = c.String(),
                        IntegerOne = c.Int(nullable: false),
                        IntegerTwo = c.Int(nullable: false),
                        IntegerThree = c.Int(nullable: false),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChildTestModels", "TestModel_Id", "dbo.TestModels");
            DropIndex("dbo.ChildTestModels", new[] { "TestModel_Id" });
            DropTable("dbo.TestModels");
            DropTable("dbo.ChildTestModels");
        }
    }
}
