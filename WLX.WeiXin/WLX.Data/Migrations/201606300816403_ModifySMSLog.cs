namespace WLX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifySMSLog : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SMSLog", "ReturnStr", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SMSLog", "ReturnStr", c => c.String(maxLength: 50));
        }
    }
}
