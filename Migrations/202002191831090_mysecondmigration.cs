namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mysecondmigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MakeupProducts", "HasPic", c => c.Int(nullable: false));
            AddColumn("dbo.MakeupProducts", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MakeupProducts", "PicExtension");
            DropColumn("dbo.MakeupProducts", "HasPic");
        }
    }
}
