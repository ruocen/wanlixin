namespace WLX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserCheckMobile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserCheckMobile",
                c => new
                    {
                        CheckID = c.String(nullable: false, maxLength: 32),
                        UserID = c.String(maxLength: 32),
                        Mobile = c.String(maxLength: 20),
                        UpdateTime = c.DateTime(nullable: false),
                        CheckCode = c.String(maxLength: 20),
                        GetCodeTimes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CheckID);
            
            AddColumn("dbo.Customer", "MobileIsCheck", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "MobileIsCheck");
            DropTable("dbo.UserCheckMobile");
        }
    }
}
