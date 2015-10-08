namespace NinjaNye.SearchExtensions.Tests.Integration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChildren : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestModels", "TestModel_Id", c => c.Guid());
            CreateIndex("dbo.TestModels", "TestModel_Id");
            AddForeignKey("dbo.TestModels", "TestModel_Id", "dbo.TestModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestModels", "TestModel_Id", "dbo.TestModels");
            DropIndex("dbo.TestModels", new[] { "TestModel_Id" });
            DropColumn("dbo.TestModels", "TestModel_Id");
        }
    }
}
