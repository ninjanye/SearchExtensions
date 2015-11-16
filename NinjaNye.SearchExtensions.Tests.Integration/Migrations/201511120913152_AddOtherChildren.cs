namespace NinjaNye.SearchExtensions.Tests.Integration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOtherChildren : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChildTestModels", "TestModel_Id1", c => c.Guid());
            CreateIndex("dbo.ChildTestModels", "TestModel_Id1");
            AddForeignKey("dbo.ChildTestModels", "TestModel_Id1", "dbo.TestModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChildTestModels", "TestModel_Id1", "dbo.TestModels");
            DropIndex("dbo.ChildTestModels", new[] { "TestModel_Id1" });
            DropColumn("dbo.ChildTestModels", "TestModel_Id1");
        }
    }
}
