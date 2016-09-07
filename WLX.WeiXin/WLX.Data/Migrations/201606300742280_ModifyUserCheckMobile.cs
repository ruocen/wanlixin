namespace WLX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyUserCheckMobile : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Customer");
            DropPrimaryKey("dbo.SMSLog");
            DropPrimaryKey("dbo.UserCheckMobile");
            AddColumn("dbo.Customer", "ID", c => c.String(nullable: false, maxLength: 32));
            AddColumn("dbo.SMSLog", "ID", c => c.String(nullable: false, maxLength: 32));
            AddColumn("dbo.UserCheckMobile", "ID", c => c.String(nullable: false, maxLength: 32));
            AddPrimaryKey("dbo.Customer", "ID");
            AddPrimaryKey("dbo.SMSLog", "ID");
            AddPrimaryKey("dbo.UserCheckMobile", "ID");
            DropColumn("dbo.Customer", "CustomerID");
            DropColumn("dbo.SMSLog", "SMSID");
            DropColumn("dbo.UserCheckMobile", "CheckID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserCheckMobile", "CheckID", c => c.String(nullable: false, maxLength: 32));
            AddColumn("dbo.SMSLog", "SMSID", c => c.String(nullable: false, maxLength: 32));
            AddColumn("dbo.Customer", "CustomerID", c => c.String(nullable: false, maxLength: 32));
            DropPrimaryKey("dbo.UserCheckMobile");
            DropPrimaryKey("dbo.SMSLog");
            DropPrimaryKey("dbo.Customer");
            DropColumn("dbo.UserCheckMobile", "ID");
            DropColumn("dbo.SMSLog", "ID");
            DropColumn("dbo.Customer", "ID");
            AddPrimaryKey("dbo.UserCheckMobile", "CheckID");
            AddPrimaryKey("dbo.SMSLog", "SMSID");
            AddPrimaryKey("dbo.Customer", "CustomerID");
        }
    }
}
