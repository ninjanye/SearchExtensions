namespace NinjaNye.SearchExtensions.Tests.Integration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIntegerProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestModels", "IntegerOne", c => c.Int(nullable: false));
            AddColumn("dbo.TestModels", "IntegerTwo", c => c.Int(nullable: false));
            AddColumn("dbo.TestModels", "IntegerThree", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TestModels", "IntegerThree");
            DropColumn("dbo.TestModels", "IntegerTwo");
            DropColumn("dbo.TestModels", "IntegerOne");
        }
    }
}
