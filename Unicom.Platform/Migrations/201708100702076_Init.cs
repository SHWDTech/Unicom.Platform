namespace Unicom.Platform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmsDevices",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Name = c.String(unicode: false),
                        IpAddr = c.String(unicode: false),
                        MacAddr = c.String(unicode: false),
                        Port = c.String(unicode: false),
                        Version = c.String(unicode: false),
                        ProjectCode = c.String(unicode: false),
                        Longitude = c.String(unicode: false),
                        Latitude = c.String(unicode: false),
                        StartDate = c.DateTime(nullable: false, precision: 0),
                        EndDate = c.DateTime(nullable: false, precision: 0),
                        InstallDate = c.DateTime(nullable: false, precision: 0),
                        OnlineStatus = c.Boolean(nullable: false),
                        VideoUrl = c.String(unicode: false),
                        IsTransfer = c.Boolean(nullable: false),
                        IsHandlerValues = c.Boolean(nullable: false),
                        TpMax = c.Double(nullable: false),
                        TpMin = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.EmsDistricts",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.EmsProjectCategories",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.EmsProjectPeriods",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.EmsProjects",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        RegisterCode = c.String(unicode: false),
                        Name = c.String(unicode: false),
                        District = c.String(unicode: false),
                        ProjectType = c.Int(nullable: false),
                        ProjectCategory = c.Int(nullable: false),
                        ProjectPeriod = c.Int(nullable: false),
                        Region = c.Int(nullable: false),
                        Street = c.String(unicode: false),
                        Longitude = c.String(unicode: false),
                        Latitude = c.String(unicode: false),
                        Contractors = c.String(unicode: false),
                        Superintendent = c.String(unicode: false),
                        Telephone = c.String(unicode: false),
                        Address = c.String(unicode: false),
                        SiteArea = c.Single(nullable: false),
                        BuildingArea = c.Single(nullable: false),
                        StartDate = c.DateTime(nullable: false, precision: 0),
                        EndDate = c.DateTime(nullable: false, precision: 0),
                        Stage = c.String(unicode: false),
                        IsCompleted = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.EmsProjectTypes",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.EmsRegions",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmsRegions");
            DropTable("dbo.EmsProjectTypes");
            DropTable("dbo.EmsProjects");
            DropTable("dbo.EmsProjectPeriods");
            DropTable("dbo.EmsProjectCategories");
            DropTable("dbo.EmsDistricts");
            DropTable("dbo.EmsDevices");
        }
    }
}
