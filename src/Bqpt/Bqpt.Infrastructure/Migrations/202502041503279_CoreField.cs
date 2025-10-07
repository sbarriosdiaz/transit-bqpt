namespace Bqpt.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CoreField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VendorPartBid", "Core", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.VendorPartBidHistory", "Core", c => c.Decimal(precision: 18, scale: 2));
            AlterStoredProcedure(
                "dbo.VendorPartBid_Insert",
                p => new
                    {
                        QuantityAvailable = p.Int(),
                        ItemsPerUnit = p.Int(),
                        UnitPrice = p.Decimal(precision: 18, scale: 2),
                        Core = p.Decimal(precision: 18, scale: 2),
                        EstimateDeliveryDate = p.DateTime(),
                        Comment = p.String(maxLength: 1024, unicode: false),
                        IsSelected = p.Boolean(),
                        VendorId = p.Int(),
                        BidQuotePartId = p.Int(),
                        DomainKey = p.String(maxLength: 48, unicode: false),
                        DisplayOrder = p.Byte(),
                        IsDeleted = p.Boolean(),
                        DeletedBy = p.String(maxLength: 48, unicode: false),
                        DeletedOn = p.DateTime(),
                        IsActive = p.Boolean(),
                        CreatedBy = p.String(maxLength: 48, unicode: false),
                        CreatedOn = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 48, unicode: false),
                        UpdatedOn = p.DateTime(),
                        DeactivatedBy = p.String(maxLength: 48, unicode: false),
                        DeactivatedOn = p.DateTime(),
                        Description = p.String(maxLength: 512, unicode: false),
                    },
                body:
                    @"INSERT [dbo].[VendorPartBid]([QuantityAvailable], [ItemsPerUnit], [UnitPrice], [Core], [EstimateDeliveryDate], [Comment], [IsSelected], [VendorId], [BidQuotePartId], [DomainKey], [DisplayOrder], [IsDeleted], [DeletedBy], [DeletedOn], [IsActive], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeactivatedBy], [DeactivatedOn], [Description])
                      VALUES (@QuantityAvailable, @ItemsPerUnit, @UnitPrice, @Core, @EstimateDeliveryDate, @Comment, @IsSelected, @VendorId, @BidQuotePartId, @DomainKey, @DisplayOrder, @IsDeleted, @DeletedBy, @DeletedOn, @IsActive, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @DeactivatedBy, @DeactivatedOn, @Description)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[VendorPartBid]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[VendorPartBid] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.VendorPartBid_Update",
                p => new
                    {
                        Id = p.Int(),
                        QuantityAvailable = p.Int(),
                        ItemsPerUnit = p.Int(),
                        UnitPrice = p.Decimal(precision: 18, scale: 2),
                        Core = p.Decimal(precision: 18, scale: 2),
                        EstimateDeliveryDate = p.DateTime(),
                        Comment = p.String(maxLength: 1024, unicode: false),
                        IsSelected = p.Boolean(),
                        VendorId = p.Int(),
                        BidQuotePartId = p.Int(),
                        DomainKey = p.String(maxLength: 48, unicode: false),
                        DisplayOrder = p.Byte(),
                        IsDeleted = p.Boolean(),
                        DeletedBy = p.String(maxLength: 48, unicode: false),
                        DeletedOn = p.DateTime(),
                        IsActive = p.Boolean(),
                        CreatedBy = p.String(maxLength: 48, unicode: false),
                        CreatedOn = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 48, unicode: false),
                        UpdatedOn = p.DateTime(),
                        DeactivatedBy = p.String(maxLength: 48, unicode: false),
                        DeactivatedOn = p.DateTime(),
                        Description = p.String(maxLength: 512, unicode: false),
                    },
                body:
                    @"UPDATE [dbo].[VendorPartBid]
                      SET [QuantityAvailable] = @QuantityAvailable, [ItemsPerUnit] = @ItemsPerUnit, [UnitPrice] = @UnitPrice, [Core] = @Core, [EstimateDeliveryDate] = @EstimateDeliveryDate, [Comment] = @Comment, [IsSelected] = @IsSelected, [VendorId] = @VendorId, [BidQuotePartId] = @BidQuotePartId, [DomainKey] = @DomainKey, [DisplayOrder] = @DisplayOrder, [IsDeleted] = @IsDeleted, [DeletedBy] = @DeletedBy, [DeletedOn] = @DeletedOn, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedOn] = @CreatedOn, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn, [DeactivatedBy] = @DeactivatedBy, [DeactivatedOn] = @DeactivatedOn, [Description] = @Description
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.VendorPartBidHistory_Insert",
                p => new
                    {
                        QuantityAvailable = p.Int(),
                        ItemsPerUnit = p.Int(),
                        UnitPrice = p.Decimal(precision: 18, scale: 2),
                        Core = p.Decimal(precision: 18, scale: 2),
                        EstimateDeliveryDate = p.DateTime(),
                        Comment = p.String(maxLength: 1024, unicode: false),
                        VendorId = p.Int(),
                        BidQuotePartId = p.Int(),
                        BidQuoteId = p.Int(),
                        IsSelected = p.Boolean(),
                        LoggedInEmailAddress = p.String(unicode: false),
                        VendorPartBidId = p.Int(),
                        VendorPartBidNoteId = p.Int(),
                        DomainKey = p.String(maxLength: 48, unicode: false),
                        DisplayOrder = p.Byte(),
                        IsDeleted = p.Boolean(),
                        DeletedBy = p.String(maxLength: 48, unicode: false),
                        DeletedOn = p.DateTime(),
                        IsActive = p.Boolean(),
                        CreatedBy = p.String(maxLength: 48, unicode: false),
                        CreatedOn = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 48, unicode: false),
                        UpdatedOn = p.DateTime(),
                        DeactivatedBy = p.String(maxLength: 48, unicode: false),
                        DeactivatedOn = p.DateTime(),
                        Description = p.String(maxLength: 512, unicode: false),
                    },
                body:
                    @"INSERT [dbo].[VendorPartBidHistory]([QuantityAvailable], [ItemsPerUnit], [UnitPrice], [Core], [EstimateDeliveryDate], [Comment], [VendorId], [BidQuotePartId], [BidQuoteId], [IsSelected], [LoggedInEmailAddress], [VendorPartBidId], [VendorPartBidNoteId], [DomainKey], [DisplayOrder], [IsDeleted], [DeletedBy], [DeletedOn], [IsActive], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeactivatedBy], [DeactivatedOn], [Description])
                      VALUES (@QuantityAvailable, @ItemsPerUnit, @UnitPrice, @Core, @EstimateDeliveryDate, @Comment, @VendorId, @BidQuotePartId, @BidQuoteId, @IsSelected, @LoggedInEmailAddress, @VendorPartBidId, @VendorPartBidNoteId, @DomainKey, @DisplayOrder, @IsDeleted, @DeletedBy, @DeletedOn, @IsActive, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @DeactivatedBy, @DeactivatedOn, @Description)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[VendorPartBidHistory]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[VendorPartBidHistory] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.VendorPartBidHistory_Update",
                p => new
                    {
                        Id = p.Int(),
                        QuantityAvailable = p.Int(),
                        ItemsPerUnit = p.Int(),
                        UnitPrice = p.Decimal(precision: 18, scale: 2),
                        Core = p.Decimal(precision: 18, scale: 2),
                        EstimateDeliveryDate = p.DateTime(),
                        Comment = p.String(maxLength: 1024, unicode: false),
                        VendorId = p.Int(),
                        BidQuotePartId = p.Int(),
                        BidQuoteId = p.Int(),
                        IsSelected = p.Boolean(),
                        LoggedInEmailAddress = p.String(unicode: false),
                        VendorPartBidId = p.Int(),
                        VendorPartBidNoteId = p.Int(),
                        DomainKey = p.String(maxLength: 48, unicode: false),
                        DisplayOrder = p.Byte(),
                        IsDeleted = p.Boolean(),
                        DeletedBy = p.String(maxLength: 48, unicode: false),
                        DeletedOn = p.DateTime(),
                        IsActive = p.Boolean(),
                        CreatedBy = p.String(maxLength: 48, unicode: false),
                        CreatedOn = p.DateTime(),
                        UpdatedBy = p.String(maxLength: 48, unicode: false),
                        UpdatedOn = p.DateTime(),
                        DeactivatedBy = p.String(maxLength: 48, unicode: false),
                        DeactivatedOn = p.DateTime(),
                        Description = p.String(maxLength: 512, unicode: false),
                    },
                body:
                    @"UPDATE [dbo].[VendorPartBidHistory]
                      SET [QuantityAvailable] = @QuantityAvailable, [ItemsPerUnit] = @ItemsPerUnit, [UnitPrice] = @UnitPrice, [Core] = @Core, [EstimateDeliveryDate] = @EstimateDeliveryDate, [Comment] = @Comment, [VendorId] = @VendorId, [BidQuotePartId] = @BidQuotePartId, [BidQuoteId] = @BidQuoteId, [IsSelected] = @IsSelected, [LoggedInEmailAddress] = @LoggedInEmailAddress, [VendorPartBidId] = @VendorPartBidId, [VendorPartBidNoteId] = @VendorPartBidNoteId, [DomainKey] = @DomainKey, [DisplayOrder] = @DisplayOrder, [IsDeleted] = @IsDeleted, [DeletedBy] = @DeletedBy, [DeletedOn] = @DeletedOn, [IsActive] = @IsActive, [CreatedBy] = @CreatedBy, [CreatedOn] = @CreatedOn, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn, [DeactivatedBy] = @DeactivatedBy, [DeactivatedOn] = @DeactivatedOn, [Description] = @Description
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.VendorPartBidHistory", "Core");
            DropColumn("dbo.VendorPartBid", "Core");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
