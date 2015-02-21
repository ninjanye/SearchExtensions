namespace NinjaNye.SearchExtensions.Tests.Integration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateTimeColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestModels", "Start", c => c.DateTime(nullable: false));
            AddColumn("dbo.TestModels", "End", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TestModels", "End");
            DropColumn("dbo.TestModels", "Start");
        }
    }
}
