namespace NinjaNye.SearchExtensions.Tests.Integration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStringThree : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestModels", "StringThree", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TestModels", "StringThree");
        }
    }
}
