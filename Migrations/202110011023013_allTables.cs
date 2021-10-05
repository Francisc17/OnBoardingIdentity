namespace OnBoardingIdentity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationProjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(nullable: false),
                        Budget = c.Int(nullable: false),
                        ProjectManager_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ProjectManager_Id, cascadeDelete: false)
                .Index(t => t.ProjectManager_Id);
            
            CreateTable(
                "dbo.ApplicationTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskName = c.String(nullable: false),
                        Deadline = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                        TaskResponsible_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationProjects", t => t.Project_Id, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.TaskResponsible_Id, cascadeDelete: false)
                .Index(t => t.Project_Id)
                .Index(t => t.TaskResponsible_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationTasks", "TaskResponsible_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationTasks", "Project_Id", "dbo.ApplicationProjects");
            DropForeignKey("dbo.ApplicationProjects", "ProjectManager_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationTasks", new[] { "TaskResponsible_Id" });
            DropIndex("dbo.ApplicationTasks", new[] { "Project_Id" });
            DropIndex("dbo.ApplicationProjects", new[] { "ProjectManager_Id" });
            DropTable("dbo.ApplicationTasks");
            DropTable("dbo.ApplicationProjects");
        }
    }
}
