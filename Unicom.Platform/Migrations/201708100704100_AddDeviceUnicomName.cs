namespace Unicom.Platform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeviceUnicomName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmsDevices", "UnicomName", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmsDevices", "UnicomName");
        }
    }
}
