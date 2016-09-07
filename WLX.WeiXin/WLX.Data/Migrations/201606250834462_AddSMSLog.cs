namespace WLX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSMSLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SMSLog",
                c => new
                    {
                        SMSID = c.String(nullable: false, maxLength: 32),
                        UserId = c.String(maxLength: 32),
                        OpenId = c.String(maxLength: 50),
                        Message = c.String(maxLength: 50),
                        SMSTYPE = c.Int(),
                        ReturnStr = c.String(maxLength: 50),
                        SendTime = c.DateTime(),
                        MobilePhone = c.String(maxLength: 50),
                        IsSuccess = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SMSID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SMSLog");
        }
    }
}
