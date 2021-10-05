namespace OnBoardingIdentity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectsChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationProjects", "ProjectManager_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationProjects", new[] { "ProjectManager_Id" });
            AlterColumn("dbo.ApplicationProjects", "ProjectManager_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.ApplicationProjects", "ProjectManager_Id");
            AddForeignKey("dbo.ApplicationProjects", "ProjectManager_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationProjects", "ProjectManager_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationProjects", new[] { "ProjectManager_Id" });
            AlterColumn("dbo.ApplicationProjects", "ProjectManager_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.ApplicationProjects", "ProjectManager_Id");
            AddForeignKey("dbo.ApplicationProjects", "ProjectManager_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
