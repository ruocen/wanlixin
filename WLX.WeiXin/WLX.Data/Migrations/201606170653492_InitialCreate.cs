namespace WLX.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerID = c.String(nullable: false, maxLength: 32),
                        Name = c.String(maxLength: 20),
                        Sex = c.String(maxLength: 50),
                        OpenId = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 20),
                        Province = c.String(maxLength: 50),
                        City = c.String(maxLength: 50),
                        BabySituation = c.String(maxLength: 50),
                        BabyBirthday = c.DateTime(),
                        CreateDate = c.DateTime(),
                        Remark = c.String(maxLength: 200),
                        IsApproved = c.Boolean(nullable: false),
                        LamaLevel = c.Int(nullable: false),
                        PicUrl = c.String(maxLength: 500),
                        WeixinID = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CustomerID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Customer");
        }
    }
}
